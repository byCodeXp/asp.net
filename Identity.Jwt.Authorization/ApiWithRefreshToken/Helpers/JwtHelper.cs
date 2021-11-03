using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Configurations;
using Microsoft.IdentityModel.Tokens;

namespace ApiWithRefreshToken.Helpers
{
    public class JwtHelper
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtConfiguration _jwtConfiguration;

        public JwtHelper(JwtConfiguration jwtConfiguration, TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
            _jwtConfiguration = jwtConfiguration;

            var secret = Encoding.ASCII.GetBytes(_jwtConfiguration.Secret);
            var symmetricSecurityKey = new SymmetricSecurityKey(secret);
            const string alg = SecurityAlgorithms.HmacSha512Signature;

            _signingCredentials = new SigningCredentials(symmetricSecurityKey, alg);
        }

        public (string, string) GenerateToken(string userId, string roles)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("id", userId),
                    new Claim("role", roles),
                }),
                Expires = DateTime.UtcNow.Add(_jwtConfiguration.LifeTime.AccessToken),
                SigningCredentials = _signingCredentials
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return (token.Id, jwtTokenHandler.WriteToken(token));
        }

        public ClaimsPrincipal Verify(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var options = _tokenValidationParameters;
            options.ValidateLifetime = false;

            var principal = tokenHandler.ValidateToken(token, options, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwt)
            {
                return null;
            }

            if (!jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
    }
}