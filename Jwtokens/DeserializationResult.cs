using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Jwtokens
{
    public class DeserializationResult
    {
        public ClaimsPrincipal ClaimsPrincipal { get; }

        public SecurityToken Token { get; }

        public DeserializationResult(ClaimsPrincipal claimsPrincipal, SecurityToken token)
        {
            ClaimsPrincipal = claimsPrincipal;
            Token = token;
        }
    }
}