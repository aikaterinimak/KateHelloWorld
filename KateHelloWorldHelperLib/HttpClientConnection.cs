using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace KateHelloWorldHelperLib
{
    public class HttpClientConnection
    {
        private HttpClient _client = null;
        private const int _TIMEOUT = 100;

        private static WebRequestHandler CreateDefaultWebRequestHandler()
        {
            WebRequestHandler aWebRequestHandler = new WebRequestHandler();

            if (aWebRequestHandler.SupportsRedirectConfiguration)
            {
                aWebRequestHandler.AllowAutoRedirect = true;
            }

            return aWebRequestHandler;
        }

        public TimeSpan Timeout
        {
            get { return _client.Timeout; }
            set { _client.Timeout = value; }
        }

        public Uri UriForBaseAddress
        {
            get { return _client.BaseAddress; }
            set { _client.BaseAddress = value; }
        }

        protected HttpRequestHeaders DefaultRequestHeaders
        {
            get { return _client.DefaultRequestHeaders; }
        }

        public HttpClientConnection(Uri aUriForBaseAddress, WebRequestHandler innerHandler, params DelegatingHandler[] handlers)
        {
            if (innerHandler == null)
            {
                innerHandler = CreateDefaultWebRequestHandler();
            }

            _client = HttpClientFactory.Create(innerHandler, handlers);
            Timeout = TimeSpan.FromSeconds(_TIMEOUT);
            UriForBaseAddress = aUriForBaseAddress;
        }

        protected async Task<string> sendRequestAndReadAsStringAsync(HttpRequestMessage aRequest, CancellationToken aCancellationToken = default(CancellationToken))
        {
            HttpResponseMessage response = await _client.SendAsync(aRequest, aCancellationToken).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        protected async Task<T> SendAndReadAsAsync<T>(HttpRequestMessage aRequest, CancellationToken aCancellationToken = default(CancellationToken))
        {
            HttpResponseMessage response = await _client.SendAsync(aRequest, aCancellationToken).ConfigureAwait(false);
            return await response.Content.ReadAsAsync<T>().ConfigureAwait(false);
        }
    }
}
