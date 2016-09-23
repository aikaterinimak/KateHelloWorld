using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KateHelloWorldHelperLib.Models.Responses;
using KateHelloWorld.Controllers;
using System.Web.Http;
using System.Web.Http.Results;

namespace KateHelloWorld.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTest
    {
        private static readonly string _USER_DISPLAYNAME = "TestUser";

        [TestMethod]
        public async Task GetUser()
        {
            // Arrange
            UsersController controller = new UsersController();

            //Act
            IHttpActionResult result = await controller.GetUser(_USER_DISPLAYNAME);
            var contentResult = result as OkNegotiatedContentResult<getUserGuidResponse>;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsNotNull(contentResult.Content.UserId);
            Assert.AreNotEqual(contentResult.Content.UserId.ToString(), "00000000-0000-0000-0000-000000000000");
            Assert.IsInstanceOfType(contentResult.Content, typeof(getUserGuidResponse));
        }
    }
}
