namespace Models
{
    namespace Net.Extensions.Http.RestClient.Models
    {
        public class ApiResponse<T>
        {
            public bool Success { get; set; }
            public T? Result { get; set; }
            public string? Error { get; set; }

            public ApiResponse() { }

            public ApiResponse(bool success, T? result, string? error)
            {
                Success = success;
                Result = result;
                Error = error;
            }
        }
    }

}
