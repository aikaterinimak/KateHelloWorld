using System;
using System.Threading.Tasks;
using KateHelloWorldHelperLib.Models.Data;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KateHelloWorldHelperLib.Tests
{
    [TestClass]
    public class IKateHelloWorldServiceProviderTest
    {
        private static readonly string _USER_GUID = "98e57604-c91a-4bc4-a270-748eac19654b";
        private static readonly string _CITY_GUID = "30dd1c31-b5b1-4246-8f73-1876884ce2cd";
        private static readonly Uri _WS_URI= new Uri("https://katehelloworld-api.com/api/");

        private IKateHelloWorldServiceProvider _KateHelloServiceProvider;

        [TestInitialize]
        public void Setup()
        {
            _KateHelloServiceProvider = new KateHelloWorldServiceProvider(_WS_URI, null, null);
        }

        [TestMethod]
        public async Task getCityListAsync()
        {
            List<CityDto> cities = await _KateHelloServiceProvider.getCityListAsync() as List<CityDto>;

            // Assert
            Assert.IsNotNull(cities);
            Assert.IsInstanceOfType(cities, typeof(List<CityDto>));
            Assert.AreNotEqual(cities.Count, 0);
        }

        [TestMethod]
        public async Task getUserGreetingsForCity()
        {
            List<string> greetings = await _KateHelloServiceProvider.getUserGreetingsForCity(_USER_GUID, _CITY_GUID) as List<string>;

            // Assert
            Assert.IsNotNull(greetings);
            Assert.IsInstanceOfType(greetings, typeof(List<string>));
            Assert.AreNotEqual(greetings.Count, 0);

            //Test invalid CityGuid
            try
            {
                greetings = await _KateHelloServiceProvider.getUserGreetingsForCity("invalid", _CITY_GUID) as List<string>;
            }
            catch(Exception ex)
            {
                Assert.IsNotNull(ex.Message);
                Assert.AreNotEqual(String.IsNullOrWhiteSpace(ex.Message), true);
            }

            //Test invalid UserGuid
            try
            {
                greetings = await _KateHelloServiceProvider.getUserGreetingsForCity(_USER_GUID, "invalid") as List<string>;
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex.Message);
                Assert.AreNotEqual(String.IsNullOrWhiteSpace(ex.Message), true);
            }
        }

        [TestMethod]
        public async Task postUserGreetingsForCity()
        {
            string status = await _KateHelloServiceProvider.postUserGreetingForCity(_USER_GUID, _CITY_GUID, "Hello, test.");

            // Assert
            Assert.IsNotNull(status);

            //Test blank greeting
            try
            {
                status = await _KateHelloServiceProvider.postUserGreetingForCity(_USER_GUID, _CITY_GUID, "");
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex.Message);
                Assert.AreNotEqual(String.IsNullOrWhiteSpace(ex.Message), true);
            }

            //Test short greeting
            try
            {
                status = await _KateHelloServiceProvider.postUserGreetingForCity(_USER_GUID, _CITY_GUID, "hi");
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex.Message);
                Assert.AreNotEqual(String.IsNullOrWhiteSpace(ex.Message), true);
            }

            //Test invalid char greeting
            try
            {
                status = await _KateHelloServiceProvider.postUserGreetingForCity(_USER_GUID, _CITY_GUID, "hello%");
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex.Message);
                Assert.AreNotEqual(String.IsNullOrWhiteSpace(ex.Message), true);
            }

            //Test invalid UserGuid
            try
            {
                status = await _KateHelloServiceProvider.postUserGreetingForCity("invalid", _CITY_GUID, "greeting");
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex.Message);
                Assert.AreNotEqual(String.IsNullOrWhiteSpace(ex.Message), true);
            }

            //Test invalid CityGuid
            try
            {
                status = await _KateHelloServiceProvider.postUserGreetingForCity(_USER_GUID, "invalid", "greeting");
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex.Message);
                Assert.AreNotEqual(String.IsNullOrWhiteSpace(ex.Message), true);
            }
        }

        [TestMethod]
        public async Task getUserRatingsForCity()
        {
            float rating = await _KateHelloServiceProvider.getUserRatingForCity(_USER_GUID, _CITY_GUID);

            // Assert
            Assert.IsNotNull(rating);

            //Test invalid UserGuid
            try
            {
                rating = await _KateHelloServiceProvider.getUserRatingForCity("invalid", _CITY_GUID);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex.Message);
                Assert.AreNotEqual(String.IsNullOrWhiteSpace(ex.Message), true);
            }

            //Test invalid CityGuid
            try
            {
                rating = await _KateHelloServiceProvider.getUserRatingForCity(_USER_GUID, "invalid");
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex.Message);
                Assert.AreNotEqual(String.IsNullOrWhiteSpace(ex.Message), true);
            }
        }

        [TestMethod]
        public async Task postUserRatingsForCity()
        {
            string status = await _KateHelloServiceProvider.postUserRatingForCity(_USER_GUID, _CITY_GUID, 5);

            // Assert
            Assert.IsNotNull(status);

            //Test invalid rating
            try
            {
                status = await _KateHelloServiceProvider.postUserRatingForCity(_USER_GUID, _CITY_GUID, 10);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex.Message);
                Assert.AreNotEqual(String.IsNullOrWhiteSpace(ex.Message), true);
            }

            //Test invalid UserGuid
            try
            {
                status = await _KateHelloServiceProvider.postUserRatingForCity("invalid", _CITY_GUID, 5);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex.Message);
                Assert.AreNotEqual(String.IsNullOrWhiteSpace(ex.Message), true);
            }

            //Test invalid CityGuid
            try
            {
                status = await _KateHelloServiceProvider.postUserRatingForCity(_USER_GUID, "invalid", 5);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex.Message);
                Assert.AreNotEqual(String.IsNullOrWhiteSpace(ex.Message), true);
            }
        }
    }
}
