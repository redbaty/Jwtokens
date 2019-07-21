using System;

namespace Jwtokens
{
    public class JwtokensOptions
    {
        public DateTime? Expiration { get; set; }

        public bool RequireHttps { get; set; } = false;

        public bool ValidateAudience { get; set; } = false;

        public bool ValidateIssuer { get; set; } = false;
    }
}