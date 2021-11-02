using Api.Dtos;

namespace Api.Contracts.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}
