﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using KateHelloWorld.Models;
using KateHelloWorldHelperLib.Models.Data;
using KateHelloWorldHelperLib.Models.Responses;
using Newtonsoft.Json.Linq;
using KateHelloWorldHelperLib.Helpers.AppStrings;
using System.Text.RegularExpressions;

namespace KateHelloWorld.Controllers
{
    /// <summary>
    ///  This controller handles the operations a user can perform on a selected city, such as greet it or rate it.
    public class CitiesController : ApiController
    {
        private KateHelloWorldEntities db = new KateHelloWorldEntities();

        /// <summary>
        ///  This service gets the list of cities and their properties, including their overall rating.
        ///  The cities are returned in a JSON array.
        /// </summary>
        /// <returns>Returns the list of cities and their properties.</returns>
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

        /// <summary>
        ///  This service gets the list of greetings given by the user for a selected city.
        /// </summary>
        /// <param name="cityid">The Guid of the selected city.</param>
        /// <param name="userid">The Guid of the user display name.</param>
        /// <returns>Returns the list of greetings of the user for the selected city.</returns>
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
                return BadRequest(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidCityId"), cityid));
            }

            try
            {
                userIDGuid = new Guid(userid);
            }
            catch
            {
                return BadRequest(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidUserId"), userid));
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

        /// <summary>
        ///  This service posts the greeting given by the user for a selected city.
        /// </summary>
        /// <param name="cityid">The Guid of the selected city.</param>
        /// <param name="userid">The Guid of the user display name.</param>
        /// <param name="greeting">The greeting given by the user. Must be between 5 and 100 characters and 
        /// cannot contain special characters, such as '$'.</param>
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
                return BadRequest(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidCityId"), cityid));
            }

            try
            {
                userIDGuid = new Guid(userid);
            }
            catch
            {
                return BadRequest(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidUserId"), userid));
            }

            string greetingPassed = (string)greeting["greeting"];

            if(String.IsNullOrWhiteSpace(greetingPassed))
            {
                return BadRequest(ErrorMessageResources.errorMngr.GetString("ErrorBlankGreeting"));
            }

            if (greetingPassed.Length > 100 || greetingPassed.Length < 5)
            {
                return BadRequest(ErrorMessageResources.errorMngr.GetString("ErrorGreetingLength"));
            }

            Regex r = new Regex("^[a-zA-Z0-9 .!?,();:]*$");
            if (!r.IsMatch(greetingPassed))
            {
                return BadRequest(ErrorMessageResources.errorMngr.GetString("ErrorGreetingInvalidChars"));
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

        /// <summary>
        ///  This service gets the rating given by the user for a selected city. In its current implementation,
        ///  there is only one rating per user.
        /// </summary>
        /// <param name="cityid">The Guid of the selected city.</param>
        /// <param name="userid">The Guid of the user display name.</param>
        /// <returns>Returns the rating of the user for the selected city.</returns>
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
                return BadRequest(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidCityId"), cityid));
            }

            try
            {
                userIDGuid = new Guid(userid);
            }
            catch
            {
                return BadRequest(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidUserId"), userid));
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

        /// <summary>
        ///  This service posts/puts the rating given by the user for a selected city. I decided to put both verbs
        ///  in one method for the sake of brevity in this hello world project.
        /// </summary>
        /// <param name="cityid">The Guid of the selected city.</param>
        /// <param name="userid">The Guid of the user display name.</param>
        /// <param name="rating">The rating given by the user. Must be between 1 and 5.</param>
        [Route("api/cities/{cityid}/users/{userid}/ratings")]
        [HttpPost]
        [HttpPut]
        public async Task<IHttpActionResult> PostRating(string cityid, string userid, [FromBody]JObject rating)
        {
            Guid cityIDGuid, userIDGuid;

            try
            {
                cityIDGuid = new Guid(cityid);
            }
            catch
            {
                return BadRequest(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidCityId"), cityid));
            }

            try
            {
                userIDGuid = new Guid(userid);
            }
            catch
            {
                return BadRequest(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidUserId"), userid));
            }

            string ratingString = (string)rating["rating"];
            float ratingPassed = 0;

            if (String.IsNullOrWhiteSpace(ratingString))
            {
                return BadRequest(ErrorMessageResources.errorMngr.GetString("ErrorBlankRating"));
            }

            if (!Single.TryParse(ratingString, out ratingPassed))
            {
                return BadRequest(ErrorMessageResources.errorMngr.GetString("ErrorRatingNumberic"));
            }
            
            if (ratingPassed < 1 || ratingPassed > 5)
            {
                return BadRequest(ErrorMessageResources.errorMngr.GetString("ErrorRatingRange"));
            }

            RateCity updateRating = db.RateCities.Where(e => e.CityId == cityIDGuid && e.UserId == userIDGuid).FirstOrDefault();

            if (updateRating != null)
            {
                updateRating.Rating = ratingPassed;

                // Mark entity as modified and save
                db.Entry(updateRating).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else
            {
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
    }
}