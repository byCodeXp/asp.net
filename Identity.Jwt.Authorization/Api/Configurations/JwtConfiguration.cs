﻿namespace Api.Configurations
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }
        public int Lifetime { get; set; }
    }
}