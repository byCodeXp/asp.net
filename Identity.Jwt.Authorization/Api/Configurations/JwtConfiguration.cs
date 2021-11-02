using System;

namespace Api.Configurations
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }
        public TimeSpan LifeTime { get; set; }
    }
}