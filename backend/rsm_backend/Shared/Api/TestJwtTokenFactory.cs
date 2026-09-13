using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Tls.Crypto;
using Shared.Api;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Api
{
    public static class TestJwtTokenFactory
    {


        private const string Issuer =
            "IntegrationTests";

        private const string Audience =
            "IntegrationTests";

        public static string CreateGuestToken(Claim[] claims)
        {

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(EcommerceApiFactory.TestJwtKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "rsm-backend",
                Audience = "rsm-guest-orders",
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = credentials
            };

            return new JsonWebTokenHandler().CreateToken(descriptor);

        }
    }
}
