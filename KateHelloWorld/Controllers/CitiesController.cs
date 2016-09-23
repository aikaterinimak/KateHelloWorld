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

namespace KateHelloWorld.Controllers
{
    public class CitiesController : ApiController
    {
        private KateHelloWorldEntities db = new KateHelloWorldEntities();

        // GET: api/Cities
        public getCitiesResponse GetCities()
        {
            List<CityDto> cities = db.Cities.Include("cities").Select(i => new CityDto {
                CityId = i.CityId,
                CityName = i.CityName,
                CityStateOrProvince = i.CityStateOrProvince,
                CityCountry = i.CityCountry,
                CityImgUri = i.CityImgUri,
                CityRating = (float)(Math.Ceiling(i.RateCities.Where(r => r.CityId == i.CityId).Average(r => r.Rating) * 2)/2)
            }).ToList();

            getCitiesResponse response = new getCitiesResponse();
            response.cities = new List<CityDto>();
            response.cities = cities;

            return response;
        }

        // GET: api/cities/cityid/users/userid/greetings
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
                return Content(HttpStatusCode.BadRequest, String.Format("CityId {0} is not a valid Guid.", cityid));
            }

            try
            {
                userIDGuid = new Guid(userid);
            }
            catch
            {
                return Content(HttpStatusCode.BadRequest, String.Format("UserId {0} is not a valid Guid.", userid));
            }

            List<string> greetings = db.HelloCities.Where(i => i.CityId == cityIDGuid
            && i.UserId == userIDGuid).Select(i => i.Greeting).ToList();

            getUserGreetingsResponse greetingResponse = new getUserGreetingsResponse();
            greetingResponse.greetings = new List<string>();
            greetingResponse.greetings = greetings;

            return Ok(greetingResponse);
        }
        
        //// POST: api/Cities
        //[ResponseType(typeof(City))]
        //public async Task<IHttpActionResult> PostCity(City city)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Cities.Add(city);

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (CityExists(city.CityId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = city.CityId }, city);
        //}
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CityExists(Guid id)
        {
            return db.Cities.Count(e => e.CityId == id) > 0;
        }
    }
}