using Microsoft.AspNetCore.Identity;

namespace ApiWithRefreshToken.Data.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}