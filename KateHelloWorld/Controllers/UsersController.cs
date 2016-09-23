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