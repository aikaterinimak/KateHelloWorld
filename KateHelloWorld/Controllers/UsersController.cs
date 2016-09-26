using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using KateHelloWorld.Models;
using KateHelloWorldHelperLib.Models.Responses;
using KateHelloWorldHelperLib.Helpers.AppStrings;
using System.Text.RegularExpressions;

namespace KateHelloWorld.Controllers
{
    /// <summary>
    ///  This controller handles the obtaining and creating of user display names and their associated Guids.
    /// </summary>
    /// <returns>Returns the Guid of the user display name.</returns>
    public class UsersController : ApiController
    {
        private KateHelloWorldEntities db = new KateHelloWorldEntities();

        /// <summary>
        ///  This service gets the GUID associated with a user display name. In real circumstances, OAuth would be used
        ///  for user login, but for the sake of brevity in the Hello World App, "users" are distinguished by display names.
        /// </summary>
        /// <param name="displayname">The desired display name of the user</param>
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
                    return BadRequest(ErrorMessageResources.errorMngr.GetString("ErrorAlphanumericDisplayName"));
                }

                if(displayname.Length < 5 || displayname.Length > 15)
                {
                    return BadRequest(ErrorMessageResources.errorMngr.GetString("ErrorDisplayNameConstraint"));
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