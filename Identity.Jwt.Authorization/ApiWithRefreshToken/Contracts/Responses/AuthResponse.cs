using ApiWithRefreshToken.Dtos;
using System;

namespace ApiWithRefreshToken.Contracts.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
        public Guid RefreshToken { get; internal set; }
    }
}
