using KateHelloWorldHelperLib.Models.Data;
using KateHelloWorldHelperLib.Models.Responses;
using KateHelloWorldHelperLib.Models.Requests;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KateHelloWorldHelperLib
{
    public class KateHelloWorldServiceProvider : HttpClientConnection, IKateHelloWorldServiceProvider
    {
        public KateHelloWorldServiceProvider(Uri aUriForBaseAddress, WebRequestHandler innerHandler, params DelegatingHandler[] handlers)
            : base(aUriForBaseAddress, innerHandler, handlers)
        {
            DefaultRequestHeaders.Authorization = null;
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
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "cities/" + aCityId + "/users/" + aUserId + "/greetings");
            getUserGreetingsResponse response = await SendAndReadAsAsync<getUserGreetingsResponse>(request, aCancellationToken);
            return response?.greetings;
        }

        public async Task<string> postUserGreetingForCity(string aUserId, string aCityId, string aGreeting, CancellationToken aCancellationToken = default(CancellationToken))
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "cities/" + aCityId + "/users/" + aUserId + "/greetings");
            postUserGreetingRequest greetingRequest = new postUserGreetingRequest(aGreeting);

            request.Content = new StringContent(JsonConvert.SerializeObject(greetingRequest), System.Text.Encoding.UTF8, "application/json");
            return await sendRequestAndReadAsStringAsync(request, aCancellationToken);
        }

        public async Task<float> getUserRatingForCity(string aUserId, string aCityId, CancellationToken aCancellationToken = default(CancellationToken))
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "cities/" + aCityId + "/users/" + aUserId + "/ratings");
            getRatingForCity response = await SendAndReadAsAsync<getRatingForCity>(request, aCancellationToken);
            return response.rating;
        }
        
        public async Task<string> postUserRatingForCity(string aUserId, string aCityId, float aRating, CancellationToken aCancellationToken = default(CancellationToken))
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "cities/" + aCityId + "/users/" + aUserId + "/ratings");
            postUserRatingRequest ratingRequest = new postUserRatingRequest(aRating);

            request.Content = new StringContent(JsonConvert.SerializeObject(ratingRequest), System.Text.Encoding.UTF8, "application/json");
            return await sendRequestAndReadAsStringAsync(request, aCancellationToken);
        }
    }
}
