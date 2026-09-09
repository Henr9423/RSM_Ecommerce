using Microsoft.Extensions.Configuration;
using rsm_backend.Application.Services.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.JsonWebTokens;

namespace rsm_backend.Infrastructure
{
    public class GuestOrderTokenService : IGuestOrderTokenService
    {
        private readonly string _secret;

        public GuestOrderTokenService(IConfiguration configuration)
        {
            _secret = configuration["GuestJwt:Secret"]
                        ?? throw new InvalidOperationException(
                            "Guest JWT secret is not configured."
                        );
        }
        public string GenerateToken(string orderNumber)
        {
            var claims = new[]
            {
                new Claim("orderNumber",orderNumber),
                new Claim("purpose", "guest-order-access"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "rsm-backend",
                Audience = "rsm-guest-orders",
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials=credentials
            };

            return new JsonWebTokenHandler().CreateToken(descriptor);
        }
    }
}
