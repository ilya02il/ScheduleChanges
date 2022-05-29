using System;

namespace IdentityAPI.Configurations
{
    public class JwtConfiguration
    {
        public string Secret { get; init; }
        public TimeSpan TokenLifetime { get; init; }
    }
}
