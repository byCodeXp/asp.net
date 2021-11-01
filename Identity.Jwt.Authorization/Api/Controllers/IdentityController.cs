using System.Linq;
using System.Threading.Tasks;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.Requests;
using Models.Responses;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IdentityService _identityService;
        
        public IdentityController(IdentityService identityService)
        {
            _identityService = identityService;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            return await _identityService.RegisterAsync(request);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            return await _identityService.LoginAsync(request);
        }

        [HttpGet("user")]
        [Authorize(Roles = Enviroment.Roles.USER + ", " + Enviroment.Roles.ADMIN)]
        public async Task<ActionResult<UserVM>> User()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(m => m.Type == "id")?.Value;
            return await _identityService.GetUserAsync(userId);
        }
    }
}