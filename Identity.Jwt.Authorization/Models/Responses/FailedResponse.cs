using System.Text.Json;

namespace Models.Responses
{
    public class FailedResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Errors { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}