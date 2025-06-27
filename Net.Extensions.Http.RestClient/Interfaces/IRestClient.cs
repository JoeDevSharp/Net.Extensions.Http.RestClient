namespace Net.Extensions.Http.RestClient.Interface
{
    public interface IRestClient
    {
        Task<RestResponse<T>> SendAsync<T>(RestRequest request);
    }
}
