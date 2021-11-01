using Models.Entities;

namespace Models.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public UserVM User { get; set; }
    }
}