using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ApiWithRefreshToken
{
    public class Seed
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public Seed(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task InvokeAsync()
        {
            if (!await _roleManager.RoleExistsAsync(Enviroment.Roles.ADMIN))
            {
                await _roleManager.CreateAsync(new IdentityRole(Enviroment.Roles.ADMIN));
            }
            
            if (!await _roleManager.RoleExistsAsync(Enviroment.Roles.USER))
            {
                await _roleManager.CreateAsync(new IdentityRole(Enviroment.Roles.USER));
            }
        }
    }
}