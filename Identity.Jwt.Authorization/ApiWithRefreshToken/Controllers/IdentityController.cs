using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Api.Configurations;
using ApiWithRefreshToken.Contracts.Requests;
using ApiWithRefreshToken.Contracts.Responses;
using ApiWithRefreshToken.Data;
using ApiWithRefreshToken.Data.Entities;
using ApiWithRefreshToken.Dtos;
using ApiWithRefreshToken.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithRefreshToken.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private readonly JwtHelper _jwtHelper;

        public IdentityController(UserManager<User> userManager, JwtHelper jwtHelper, DataContext context = null, JwtConfiguration jwtConfiguration = null)
        {
            _jwtConfiguration = jwtConfiguration;
            _userManager = userManager;
            _jwtHelper = jwtHelper;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            // Create user
            var user = new User
            {
                Name = request.Name,
                UserName = request.Username,
                Email = request.Email
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                return BadRequest("User was not created successfully");
            }

            // Add role to user
            var roleResult = await _userManager.AddToRoleAsync(user, Enviroment.Roles.USER);

            if (!roleResult.Succeeded)
            {
                return BadRequest("User was not created successfully");
            }

            return await GetAuthResponseAsync(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            // Find user
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                return BadRequest("Invalid credentials");
            }

            // Check password
            var checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!checkPassword)
            {
                return BadRequest("Invalid credentials");
            }

            return await GetAuthResponseAsync(user);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var principal = _jwtHelper.Verify(request.Token);

            if (principal == null)
            {
                return BadRequest("Invalid access token");
            }

            var storedRefreshToken = _context.RefreshTokens.FirstOrDefault(m => m.Id == request.RefreshToken);

            if (storedRefreshToken == null)
            {
                return BadRequest("Refresh token doesn't exist");
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpireAt)
            {
                return BadRequest("Refresh token expired");
            }

            var jti = principal.Claims.First(m => m.Type == JwtRegisteredClaimNames.Jti).Value;

            if (storedRefreshToken.JwtId != jti)
            {
                return BadRequest("Refresh token doesn't match this Jwt");
            }

            var user = await _userManager.FindByIdAsync(principal.Claims.First(m => m.Type == "id").Value);

            if (user == null)
            {
                return BadRequest("User was not found");
            }

            _context.Remove(storedRefreshToken);
            _context.SaveChanges();
            
            return await GetAuthResponseAsync(user);
        }

        [HttpGet("user")]
        [Authorize(Roles = Enviroment.Roles.USER + ", " + Enviroment.Roles.ADMIN)]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            var userId = User.Claims.FirstOrDefault(m => m.Type == "id")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("Use was not found");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("Use was not found");
            }

            return new UserDto
            {
                Name = user.Name,
                Username = user.UserName,
                Email = user.Email
            };
        }

        private async Task<AuthResponse> GetAuthResponseAsync(User user)
        {
            string roles = string.Join(", ", await _userManager.GetRolesAsync(user));

            var (id, token) = _jwtHelper.GenerateToken(user.Id, roles);

            var refreshToken = new RefreshToken
            {
                JwtId = id,
                UserId = user.Id,
                ExpireAt = DateTime.UtcNow.Add(_jwtConfiguration.LifeTime.RefreshToken)
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Token = token,
                RefreshToken = refreshToken.Id,
                User = new UserDto
                {
                    Name = user.Name,
                    Username = user.UserName,
                    Email = user.Email
                }
            };
        }
    }
}