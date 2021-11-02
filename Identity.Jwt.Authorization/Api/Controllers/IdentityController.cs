using System.Linq;
using System.Threading.Tasks;
using Api.Contracts.Requests;
using Api.Contracts.Responses;
using Api.Data.Entities;
using Api.Dtos;
using Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtHelper _jwtHelper;

        public IdentityController(UserManager<User> userManager, JwtHelper jwtHelper)
        {
            _userManager = userManager;
            _jwtHelper = jwtHelper;
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

            return new AuthResponse
            {
                Token = _jwtHelper.GenerateToken(user.Id, Enviroment.Roles.USER),
                User = new UserDto
                {
                    Name = user.Name,
                    Username = user.UserName,
                    Email = user.Email
                }
            };
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

            var roles = string.Join(", ", await _userManager.GetRolesAsync(user));

            return new AuthResponse
            {
                Token = _jwtHelper.GenerateToken(user.Id, roles),
                User = new UserDto
                {
                    Name = user.Name,
                    Username = user.UserName,
                    Email = user.Email
                }
            };
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
    }
}