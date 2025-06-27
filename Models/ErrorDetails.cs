namespace Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? Details { get; set; }

        public ErrorDetails() { }

        public ErrorDetails(int statusCode, string? message, string? details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }
    }
}
