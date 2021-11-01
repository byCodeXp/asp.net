using System.Linq;
using System.Threading.Tasks;
using Api.Exceptions;
using Api.Helpers;
using AutoMapper;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Models.Entities;
using Models.Requests;
using Models.Responses;

namespace Api.Services
{
    public class IdentityService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtHelper _jwtHelper;
        private readonly IMapper _mapper;

        public IdentityService(UserManager<User> userManager, IMapper mapper, JwtHelper jwtHelper, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtHelper = jwtHelper;
            _mapper = mapper;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            // Create user
            var user = _mapper.Map<User>(request);
            
            var createResult = await _userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                throw new BadRequestRestException("User was not created successfully", createResult.Errors);
            }

            // Add role to user
            var roleResult = await _userManager.AddToRoleAsync(user, Enviroment.Roles.USER);

            if (!roleResult.Succeeded)
            {
                throw new BadRequestRestException("User was not created successfully", roleResult.Errors);
            }

            return new AuthResponse
            {
                Token = _jwtHelper.GenerateToken(user.Id, Enviroment.Roles.USER),
                User = _mapper.Map<UserVM>(user)
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            // Find user
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                throw new BadRequestRestException("Invalid credentials");
            }

            // Check password
            var checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!checkPassword)
            {
                throw new BadRequestRestException("Invalid credentials");
            }

            // Get user role
            var role = string.Join(", ", await _userManager.GetRolesAsync(user));
            
            return new AuthResponse
            {
                Token = _jwtHelper.GenerateToken(user.Id, role),
                User = _mapper.Map<UserVM>(user)
            };
        }

        public async Task<UserVM> GetUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new NotFoundRestException("User was not found");
            }
            
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user == null)
            {
                throw new NotFoundRestException("User was not found");
            }

            return _mapper.Map<UserVM>(user);
        }
    }
}