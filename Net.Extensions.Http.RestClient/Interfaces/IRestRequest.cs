namespace Abstraction
{
    public interface IRestRequest
    {
        string Resource { get; }
        HttpMethod Method { get; }
        object? Body { get; }
        Dictionary<string, string>? Headers { get; }
        Dictionary<string, string>? QueryParameters { get; }
    }
}
