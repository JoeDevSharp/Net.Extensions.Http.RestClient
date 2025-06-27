namespace Net.Extensions.Http.RestClient
{
    public class RestResponse<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }

        public RestResponse() { }

        public RestResponse(bool isSuccess, int statusCode, T? data, string? errorMessage)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Data = data;
            ErrorMessage = errorMessage;
        }
    }
}
