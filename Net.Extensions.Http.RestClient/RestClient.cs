using Net.Extensions.Http.RestClient.Interface;
using System.Web;

namespace Net.Extensions.Http.RestClient
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient _httpClient;

        public RestClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            if (_httpClient.BaseAddress == null)
                throw new ArgumentException("HttpClient.BaseAddress must be set.");
        }

        public async Task<RestResponse<T>> SendAsync<T>(RestRequest request)
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
    }

}
