using KateHelloWorld.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using KateHelloWorldHelperLib.Models.Responses;

namespace KateHelloWorld.Tests.Controllers
{
    [TestClass]
    public class CitiesControllerTest
    {
        private static readonly string _USER_GUID = "98e57604-c91a-4bc4-a270-748eac19654b";
        private static readonly string _CITY_GUID = "30dd1c31-b5b1-4246-8f73-1876884ce2cd";

        [TestMethod]
        public void GetCities()
        {
            // Arrange
            CitiesController controller = new CitiesController();

            // Act
            IHttpActionResult result = controller.GetCities();
            var contentResult = result as OkNegotiatedContentResult<getCitiesResponse>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsNotNull(contentResult.Content.cities);
            Assert.IsInstanceOfType(contentResult.Content, typeof(getCitiesResponse));
            Assert.AreNotEqual(contentResult.Content.cities.Count, 0);
        }

        [TestMethod]
        public void GetGreetings()
        {
            // Arrange
            CitiesController controller = new CitiesController();

            // Act
            IHttpActionResult result = controller.GetGreetings(_CITY_GUID, _USER_GUID);
            var contentResult = result as OkNegotiatedContentResult<getUserGreetingsResponse>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsNotNull(contentResult.Content.greetings);
            Assert.IsInstanceOfType(contentResult.Content, typeof(getUserGreetingsResponse));
            Assert.AreNotEqual(contentResult.Content.greetings.Count, 0);
        }

        [TestMethod]
        public async Task PostGreeting()
        {
            // Arrange
            CitiesController controller = new CitiesController();
            JObject greeting = new JObject();
            greeting["greeting"] = "Hello, Test.";

            //***Test Valid Request***
            // Act
            IHttpActionResult result = await controller.PostGreeting(_CITY_GUID, _USER_GUID, greeting);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            //*************************

            //***Test Invalid Guid Request***
            // Act
            IHttpActionResult resultInvalidGuid = await controller.PostGreeting("invalid", _USER_GUID, greeting);

            // Assert
            Assert.IsNotNull(resultInvalidGuid);
            Assert.IsInstanceOfType(resultInvalidGuid, typeof(BadRequestErrorMessageResult));
            //*************************

            //***Test Invalid Greeting Request***
            // Act
            greeting["greeting"] = "";
            IHttpActionResult resultInvalidGreeting = await controller.PostGreeting(_CITY_GUID, _USER_GUID, greeting);

            // Assert
            Assert.IsNotNull(resultInvalidGreeting);
            Assert.IsInstanceOfType(resultInvalidGreeting, typeof(BadRequestErrorMessageResult));
            //*************************
        }

        [TestMethod]
        public void GetRating()
        {
            // Arrange
            CitiesController controller = new CitiesController();

            // Act
            IHttpActionResult result = controller.GetRating(_CITY_GUID, _USER_GUID);
            var contentResult = result as OkNegotiatedContentResult<getRatingForCity>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsNotNull(contentResult.Content.rating);
            Assert.IsInstanceOfType(contentResult.Content, typeof(getRatingForCity));
        }

        [TestMethod]
        public async Task PostRating()
        {
            // Arrange
            CitiesController controller = new CitiesController();
            JObject rating = new JObject();
            rating["rating"] = 5;

            // Act
            IHttpActionResult result = await controller.PostRating(_CITY_GUID, _USER_GUID, rating);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));

            //***Test Invalid Guid Request***
            // Act
            IHttpActionResult resultInvalidGuid = await controller.PostGreeting("invalid", _USER_GUID, rating);

            // Assert
            Assert.IsNotNull(resultInvalidGuid);
            Assert.IsInstanceOfType(resultInvalidGuid, typeof(BadRequestErrorMessageResult));
            //*************************

            //***Test Invalid Greeting Request***
            // Act
            rating["rating"] = 0;
            IHttpActionResult resultInvalidRating = await controller.PostGreeting(_CITY_GUID, _USER_GUID, rating);

            // Assert
            Assert.IsNotNull(resultInvalidRating);
            Assert.IsInstanceOfType(resultInvalidRating, typeof(BadRequestErrorMessageResult));
            //*************************
        }
    }
}
