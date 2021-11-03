using System;

namespace ApiWithRefreshToken.Contracts.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public Guid RefreshToken { get; set; }
    }
}
