using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiWithRefreshToken.Data.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string JwtId { get; set; }
        public DateTime ExpireAt { get; set; }
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
