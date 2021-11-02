using Microsoft.AspNetCore.Identity;

namespace Api.Data.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}