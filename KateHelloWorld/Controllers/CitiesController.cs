using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using KateHelloWorld.Models;
using Newtonsoft.Json;
using KateHelloWorldHelperLib.Models.Data;
using KateHelloWorldHelperLib.Models.Responses;
using Newtonsoft.Json.Linq;

namespace KateHelloWorld.Controllers
{
    public class CitiesController : ApiController
    {
        private KateHelloWorldEntities db = new KateHelloWorldEntities();

        // GET to retreive all cities
        public IHttpActionResult GetCities()
        {
            getCitiesResponse response = new getCitiesResponse();

            try
            {
                List<CityDto> cities = db.Cities.Include("cities").Select(i => new CityDto
                {
                    CityId = i.CityId,
                    CityName = i.CityName,
                    CityStateOrProvince = i.CityStateOrProvince,
                    CityCountry = i.CityCountry,
                    CityImgUri = i.CityImgUri,
                    CityRating = (float)(Math.Ceiling(i.RateCities.Where(r => r.CityId == i.CityId).Average(r => r.Rating) * 2) / 2)
                }).ToList();

                response.cities = new List<CityDto>();
                response.cities = cities;
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(response);
        }

        // GET to find greetings for a user for a city
        [Route("api/cities/{cityid}/users/{userid}/greetings")]
        [HttpGet]
        public IHttpActionResult GetGreetings(string cityid, string userid)
        {
            Guid cityIDGuid, userIDGuid;

            try
            {
                cityIDGuid = new Guid(cityid);
            }
            catch
            {
                return BadRequest(String.Format("CityId {0} is not a valid Guid.", cityid));
            }

            try
            {
                userIDGuid = new Guid(userid);
            }
            catch
            {
                return BadRequest(String.Format("UserId {0} is not a valid Guid.", userid));
            }

            getUserGreetingsResponse greetingResponse = new getUserGreetingsResponse();

            try
            {
                List<string> greetings = db.HelloCities.Where(i => i.CityId == cityIDGuid
                && i.UserId == userIDGuid).Select(i => i.Greeting).ToList();
                
                greetingResponse.greetings = new List<string>();
                greetingResponse.greetings = greetings;
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(greetingResponse);
        }
        
        // POST to create a greeting for a user for a city
        [Route("api/cities/{cityid}/users/{userid}/greetings")]
        [HttpPost]
        public async Task<IHttpActionResult> PostGreeting(string cityid, string userid, [FromBody]JObject greeting)
        {
            Guid cityIDGuid, userIDGuid;

            try
            {
                cityIDGuid = new Guid(cityid);
            }
            catch
            {
                return BadRequest(String.Format("CityId {0} is not a valid Guid.", cityid));
            }

            try
            {
                userIDGuid = new Guid(userid);
            }
            catch
            {
                return BadRequest(String.Format("UserId {0} is not a valid Guid.", userid));
            }

            string greetingPassed = (string)greeting["greeting"];

            if(String.IsNullOrWhiteSpace(greetingPassed))
            {
                return BadRequest("Greeting cannot be blank.");
            }

            if (greetingPassed.Length > 100 || greetingPassed.Length < 5)
            {
                return BadRequest("Greeting must be between 5 and 100 characters.");
            }

            HelloCity newGreeting = new HelloCity();
            newGreeting.CityId = cityIDGuid;
            newGreeting.UserId = userIDGuid;
            newGreeting.Greeting = greetingPassed;
            newGreeting.GreetingDateTime = DateTime.UtcNow;

            try
            {
                db.HelloCities.Add(newGreeting);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            
            return Ok();
        }

        // GET to find rating for a user for a city
        [Route("api/cities/{cityid}/users/{userid}/ratings")]
        [HttpGet]
        public IHttpActionResult GetRating(string cityid, string userid)
        {
            Guid cityIDGuid, userIDGuid;

            try
            {
                cityIDGuid = new Guid(cityid);
            }
            catch
            {
                return BadRequest(String.Format("CityId {0} is not a valid Guid.", cityid));
            }

            try
            {
                userIDGuid = new Guid(userid);
            }
            catch
            {
                return BadRequest(String.Format("UserId {0} is not a valid Guid.", userid));
            }

            getRatingForCity ratingResponse = new getRatingForCity();

            try
            {
                double rating = db.RateCities.First(i => i.CityId == cityIDGuid && i.UserId == userIDGuid).Rating;
                ratingResponse.rating = (float)rating;
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(ratingResponse);
        }

        // POST to create a rating for a user for a city
        [Route("api/cities/{cityid}/users/{userid}/ratings")]
        [HttpPost]
        public async Task<IHttpActionResult> PostRating(string cityid, string userid, [FromBody]JObject rating)
        {
            Guid cityIDGuid, userIDGuid;

            try
            {
                cityIDGuid = new Guid(cityid);
            }
            catch
            {
                return BadRequest(String.Format("CityId {0} is not a valid Guid.", cityid));
            }

            try
            {
                userIDGuid = new Guid(userid);
            }
            catch
            {
                return BadRequest(String.Format("UserId {0} is not a valid Guid.", userid));
            }

            string ratingString = (string)rating["rating"];
            float ratingPassed = 0;

            if (String.IsNullOrWhiteSpace(ratingString))
            {
                return BadRequest("Rating cannot be blank.");
            }

            if (!Single.TryParse(ratingString, out ratingPassed))
            {
                return BadRequest("Rating must be a number.");
            }
            
            if (ratingPassed < 1 || ratingPassed > 5)
            {
                return BadRequest("Rating must be between 1 and 5.");
            }

            if (RatingExists(cityIDGuid, userIDGuid))
            {
                return BadRequest("You have already rated this city.");
            }

            RateCity newRating = new RateCity();
            newRating.CityId = cityIDGuid;
            newRating.UserId = userIDGuid;
            newRating.Rating = ratingPassed;

            try
            {
                db.RateCities.Add(newRating);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Check to see if the user has already rated the city
        private bool RatingExists(Guid cityid, Guid userid)
        {
            return db.RateCities.Count(e => e.CityId == cityid && e.UserId == userid) > 0;
        }
    }
}