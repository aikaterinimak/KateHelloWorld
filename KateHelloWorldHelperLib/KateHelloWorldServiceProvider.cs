using KateHelloWorldHelperLib.Models.Data;
using KateHelloWorldHelperLib.Models.Responses;
using KateHelloWorldHelperLib.Models.Requests;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using KateHelloWorldHelperLib.Helpers.AppStrings;

namespace KateHelloWorldHelperLib
{
    public class KateHelloWorldServiceProvider : HttpClientConnection, IKateHelloWorldServiceProvider
    {
        public KateHelloWorldServiceProvider(Uri aUriForBaseAddress, WebRequestHandler innerHandler, params DelegatingHandler[] handlers)
            : base(aUriForBaseAddress, innerHandler, handlers)
        {
            DefaultRequestHeaders.Authorization = null;
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            UriForBaseAddress = aUriForBaseAddress;
        }
                
        public async Task<IList<CityDto>> getCityListAsync(CancellationToken aCancelationToken = default(CancellationToken))
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "cities/");
            getCitiesResponse cityList = await SendAndReadAsAsync<getCitiesResponse>(request, aCancelationToken);
            return cityList?.cities;
        }

        public async Task<IList<string>> getUserGreetingsForCity(string aUserId, string aCityId, CancellationToken aCancellationToken = default(CancellationToken))
        {
            Guid guidOutput;
            if (!Guid.TryParse(aUserId, out guidOutput))
            {
                throw new ArgumentException(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidUserId"), aUserId));
            }

            if (!Guid.TryParse(aCityId, out guidOutput))
            {
                throw new ArgumentException(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidCityId"), aCityId));
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "cities/" + aCityId + "/users/" + aUserId + "/greetings");
            getUserGreetingsResponse response = await SendAndReadAsAsync<getUserGreetingsResponse>(request, aCancellationToken);
            return response?.greetings;
        }

        public async Task<string> postUserGreetingForCity(string aUserId, string aCityId, string aGreeting, CancellationToken aCancellationToken = default(CancellationToken))
        {
            Guid guidOutput;
            if (!Guid.TryParse(aUserId, out guidOutput))
            {
                throw new ArgumentException(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidUserId"), aUserId));
            }

            if (!Guid.TryParse(aCityId, out guidOutput))
            {
                throw new ArgumentException(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidCityId"), aCityId));
            }

            if (String.IsNullOrWhiteSpace(aGreeting))
            {
                throw new ArgumentException(ErrorMessageResources.errorMngr.GetString("ErrorBlankGreeting"));
            }

            if (aGreeting.Length < 5 || aGreeting.Length > 100)
            {
                throw new ArgumentException(ErrorMessageResources.errorMngr.GetString("ErrorGreetingLength"));
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "cities/" + aCityId + "/users/" + aUserId + "/greetings");
            postUserGreetingRequest greetingRequest = new postUserGreetingRequest(aGreeting);

            request.Content = new StringContent(JsonConvert.SerializeObject(greetingRequest), System.Text.Encoding.UTF8, "application/json");
            return await sendRequestAndReadAsStringAsync(request, aCancellationToken);
        }

        public async Task<float> getUserRatingForCity(string aUserId, string aCityId, CancellationToken aCancellationToken = default(CancellationToken))
        {
            Guid guidOutput;
            if (!Guid.TryParse(aUserId, out guidOutput))
            {
                throw new ArgumentException(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidUserId"), aUserId));
            }

            if (!Guid.TryParse(aCityId, out guidOutput))
            {
                throw new ArgumentException(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidCityId"), aCityId));
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "cities/" + aCityId + "/users/" + aUserId + "/ratings");
            getRatingForCity response = await SendAndReadAsAsync<getRatingForCity>(request, aCancellationToken);
            return response.rating;
        }
        
        public async Task<string> postUserRatingForCity(string aUserId, string aCityId, float aRating, CancellationToken aCancellationToken = default(CancellationToken))
        {
            Guid guidOutput;
            if (!Guid.TryParse(aUserId, out guidOutput))
            {
                throw new ArgumentException(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidUserId"), aUserId));
            }

            if (!Guid.TryParse(aCityId, out guidOutput))
            {
                throw new ArgumentException(String.Format(ErrorMessageResources.errorMngr.GetString("ErrorInvalidCityId"), aCityId));
            }

            if (aRating < 1 || aRating > 5)
            {
                throw new ArgumentException(ErrorMessageResources.errorMngr.GetString("ErrorRatingRange"));
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "cities/" + aCityId + "/users/" + aUserId + "/ratings");
            postUserRatingRequest ratingRequest = new postUserRatingRequest(aRating);

            request.Content = new StringContent(JsonConvert.SerializeObject(ratingRequest), System.Text.Encoding.UTF8, "application/json");
            return await sendRequestAndReadAsStringAsync(request, aCancellationToken);
        }

        public async Task<Guid> getUserId(string aDisplayName, CancellationToken aCancellationToken = default(CancellationToken))
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "users/" + aDisplayName);
            getUserGuidResponse response = await SendAndReadAsAsync<getUserGuidResponse>(request, aCancellationToken);
            return response.UserId;
        }
    }
}
