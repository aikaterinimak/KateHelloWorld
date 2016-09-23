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
using KateHelloWorldHelperLib.Models.Responses;
using System.Text.RegularExpressions;

namespace KateHelloWorld.Controllers
{
    public class UsersController : ApiController
    {
        private KateHelloWorldEntities db = new KateHelloWorldEntities();

        // GET Guid associated with displayname, if exists
        //     Guid gets created if displayname does not exist
        [Route("api/users/{displayname}/")]
        [HttpGet]
        [HttpPost]
        public async Task<IHttpActionResult> GetUser(string displayname)
        {
            getUserGuidResponse getUser = new getUserGuidResponse();
            User aUser = db.Users.Where(x => x.UserDisplayName.ToLower() == displayname.ToLower()).FirstOrDefault();
            if (aUser != null)
            {
                getUser.UserId = aUser.UserId;
            }
            else
            {
                Regex r = new Regex("^[a-zA-Z0-9]*$");
                if (!r.IsMatch(displayname))
                {
                    return BadRequest("Display name must be alphanumeric.");
                }

                if(displayname.Length < 5 || displayname.Length > 15)
                {
                    return BadRequest("Display name must be between 5 and 15 characters.");
                }

                Guid newUserGuid = Guid.NewGuid();
                User newUser = new User();

                newUser.UserId = newUserGuid;
                newUser.UserDisplayName = displayname;

                try
                {
                    db.Users.Add(newUser);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return Ok(getUser);
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