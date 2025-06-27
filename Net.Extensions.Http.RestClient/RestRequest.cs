namespace Net.Extensions.Http.RestClient
{
    public class RestRequest
    {
        public string Resource { get; private set; }
        public HttpMethod Method { get; private set; }
        public object? Body { get; set; }
        public Dictionary<string, string>? Headers { get; set; }
        public Dictionary<string, string>? QueryParameters { get; set; }

        public RestRequest(string resource, HttpMethod method)
        {
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
            Method = method ?? throw new ArgumentNullException(nameof(method));
        }

        public RestRequest(string resource)
        {
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }

        public RestRequest AddBody(object body)
        {
            Body = body;
            return this;
        }

        public RestRequest AddHeader(string key, string value)
        {
            if (Headers == null)
                Headers = new Dictionary<string, string>();

            Headers[key] = value;
            return this;
        }

        public RestRequest AddQueryParameter(string key, string value)
        {
            if (QueryParameters == null)
                QueryParameters = new Dictionary<string, string>();

            QueryParameters[key] = value;
            return this;
        }
    }
}
