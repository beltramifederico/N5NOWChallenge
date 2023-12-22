using System.Text.Json.Serialization;

namespace N5NOW.UserPermissions.Application.Exceptions
{
    public class ExceptionResponse
    {
        public ExceptionResponse(int statusCode, string message, IEnumerable<string>? errors)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
        }

        public ExceptionResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("errors")]
        public IEnumerable<string>? Errors { get; set; }
    }
}
