using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Configurations;
using Microsoft.IdentityModel.Tokens;

namespace Api.Helpers
{
    public class JwtHelper
    {
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly SigningCredentials _signingCredentials;
        
        public JwtHelper(JwtConfiguration jwtConfiguration)
        {
            _jwtConfiguration = jwtConfiguration;
            
            var secret = Encoding.ASCII.GetBytes(_jwtConfiguration.Secret);
            var symmetricSecurityKey = new SymmetricSecurityKey(secret);
            
            _signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
        }
        
        public string GenerateToken(string userId, string role)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", userId),
                    new Claim("role", role)
                }),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromSeconds(_jwtConfiguration.Lifetime)),
                SigningCredentials = _signingCredentials
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}