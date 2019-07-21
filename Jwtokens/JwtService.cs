using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Jwtokens
{
    public class JwtService
    {
        private JwtokensOptions Options { get; }

        private JwtokensSettings Settings { get; }

        private JwtSecurityTokenHandler TokenHandler { get; }

        public JwtService(JwtokensOptions options, IOptions<JwtokensSettings> jwtSettings)
        {
            Options = options;
            TokenHandler = new JwtSecurityTokenHandler();
            Settings = jwtSettings.Value;
        }

        public string CreateToken(params Claim[] claims)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = Options.Expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(GetKey()),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            return TokenHandler.WriteToken(TokenHandler.CreateToken(tokenDescriptor));
        }

        public DeserializationResult DeserializeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var claimsPrincipal = handler.ValidateToken(token, GetTokenValidationParameters(), out var securityToken);
            return new DeserializationResult(claimsPrincipal, securityToken);
        }

        internal void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(authenticationOptions =>
                {
                    authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.RequireHttpsMetadata = Options.RequireHttps;
                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.TokenValidationParameters = GetTokenValidationParameters();
                });
        }

        private byte[] GetKey()
        {
            return Encoding.ASCII.GetBytes(Settings.Secret);
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(GetKey()),
                ValidateIssuer = Options.ValidateIssuer,
                ValidateAudience = Options.ValidateAudience
            };
            return validations;
        }
    }
}