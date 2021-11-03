using System;

namespace Api.Configurations
{
    public class JwtConfiguration
    {
        public class Lifetime
        {
            public TimeSpan AccessToken { get; set; }
            public TimeSpan RefreshToken { get; set; }
        }

        public string Secret { get; set; }
        public Lifetime LifeTime { get; set; }
    }
}