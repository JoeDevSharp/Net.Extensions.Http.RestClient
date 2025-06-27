namespace Net.Extensions.Http.RestClient.Extensions
{
    public static class RestRequestBuilderExtensions
    {
        public static RestRequest WithResource(this RestRequest request, string resource)
        {
            // Como Resource es readonly, creamos un nuevo RestRequest con mismo método
            return new RestRequest(resource, request.Method)
            {
                Body = request.Body,
                Headers = request.Headers,
                QueryParameters = request.QueryParameters
            };
        }

        public static RestRequest WithMethod(this RestRequest request, HttpMethod method)
        {
            return new RestRequest(request.Resource, method)
            {
                Body = request.Body,
                Headers = request.Headers,
                QueryParameters = request.QueryParameters
            };
        }

        public static RestRequest AddHeader(this RestRequest request, string key, string value)
        {
            request.AddHeader(key, value);
            return request;
        }

        public static RestRequest AddQueryParam(this RestRequest request, string key, string value)
        {
            request.AddQueryParameter(key, value);
            return request;
        }

        public static RestRequest WithBody(this RestRequest request, object body)
        {
            request.AddBody(body);
            return request;
        }
    }
}
