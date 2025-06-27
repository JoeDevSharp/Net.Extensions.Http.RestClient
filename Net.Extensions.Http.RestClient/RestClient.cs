using Net.Extensions.Http.RestClient.Interface;
using Net.Extensions.Http.RestClient.Policies;
using System.Web;

namespace Net.Extensions.Http.RestClient
{
    public class RestClient : IRestClient
    {
        private HttpClient _httpClient;
        private RetryPolicy? _retryPolicy;
        private TimeoutPolicy? _timeoutPolicy;

        public RestClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            if (_httpClient.BaseAddress == null)
                throw new ArgumentException("HttpClient.BaseAddress must be set.");
        }

        public RestClient()
        {
            _httpClient = new HttpClient();
        }

        public Uri BaseUrl => _httpClient.BaseAddress!;
        public async Task<RestResponse<T>> SendAsync<T>(RestRequest request)
        {
            Func<Task<RestResponse<T>>> action = () => SendRequestInternal<T>(request);

            if (_timeoutPolicy != null)
            {
                action = () => _timeoutPolicy.ExecuteAsync(action);
            }

            if (_retryPolicy != null)
            {
                return await _retryPolicy.ExecuteAsync(action);
            }

            return await action();
        }
        private async Task<RestResponse<T>> SendRequestInternal<T>(RestRequest request)
        {
            try
            {
                var httpRequest = new HttpRequestMessage(request.Method, BuildUri(request));

                if (request.Body != null)
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(request.Body);
                    httpRequest.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                }

                if (request.Headers != null)
                {
                    foreach (var header in request.Headers)
                    {
                        if (httpRequest.Headers.Contains(header.Key))
                            httpRequest.Headers.Remove(header.Key);
                        httpRequest.Headers.Add(header.Key, header.Value);
                    }
                }

                var response = await _httpClient.SendAsync(httpRequest);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var data = System.Text.Json.JsonSerializer.Deserialize<T>(content);
                    return new RestResponse<T>(true, (int)response.StatusCode, data, null);
                }
                else
                {
                    return new RestResponse<T>(false, (int)response.StatusCode, default, content);
                }
            }
            catch (Exception ex)
            {
                return new RestResponse<T>(false, 0, default, ex.Message);
            }
        }
        public void SetAuthHandler(HttpMessageHandler authHandler)
        {
            if (authHandler == null)
                throw new ArgumentNullException(nameof(authHandler));

            var baseAddress = _httpClient.BaseAddress;

            // Crea un nuevo HttpClient con el handler personalizado
            _httpClient.Dispose();
            _httpClient = new HttpClient(authHandler)
            {
                BaseAddress = baseAddress
            };
        }
        public void SetBaseAddress(Uri baseAddress)
        {
            if (baseAddress == null)
                throw new ArgumentNullException(nameof(baseAddress));

            _httpClient.BaseAddress = baseAddress;
        }
        public void SetRetryPolicy(RetryPolicy retryPolicy)
        {
            _retryPolicy = retryPolicy ?? throw new ArgumentNullException(nameof(retryPolicy));
        }
        public void SetTimeout(TimeSpan timeout)
        {
            if (timeout <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeout), "El timeout debe ser mayor que cero.");

            _httpClient.Timeout = timeout;
        }
        private Uri BuildUri(RestRequest request)
        {
            var baseUri = _httpClient.BaseAddress ?? throw new InvalidOperationException("BaseAddress no está configurada.");

            var uriBuilder = new UriBuilder(new Uri(baseUri, request.Resource));

            if (request.QueryParameters != null && request.QueryParameters.Count > 0)
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                foreach (var param in request.QueryParameters)
                {
                    query[param.Key] = param.Value;
                }
                uriBuilder.Query = query.ToString() ?? string.Empty;
            }

            return uriBuilder.Uri;
        }
        public void SetTimeoutPolicy(TimeoutPolicy timeoutPolicy)
        {
            _timeoutPolicy = timeoutPolicy ?? throw new ArgumentNullException(nameof(timeoutPolicy));
        }
    }
}
