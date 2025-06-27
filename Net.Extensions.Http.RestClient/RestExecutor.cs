namespace Net.Extensions.Http.RestClient
{
    internal class RestExecutor
    {
        private readonly HttpClient _httpClient;

        public RestExecutor(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<HttpResponseMessage> ExecuteAsync(RestRequest request, CancellationToken cancellationToken = default)
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
                    httpRequest.Headers.Remove(header.Key);
                    httpRequest.Headers.Add(header.Key, header.Value);
                }
            }

            return await _httpClient.SendAsync(httpRequest, cancellationToken);
        }

        private Uri BuildUri(RestRequest request)
        {
            var baseUri = _httpClient.BaseAddress ?? throw new InvalidOperationException("BaseAddress must be set.");

            var uriBuilder = new UriBuilder(new Uri(baseUri, request.Resource));

            if (request.QueryParameters != null && request.QueryParameters.Count > 0)
            {
                var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
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
