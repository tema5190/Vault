using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Vault.API.Infrastructure.JWT
{
    public static class JwtHelper
    {
        public static object CreateToken(ClaimsIdentity identity)
        {
            var now = DateTime.Now;

            var token = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                notBefore: now,

                claims: identity.Claims,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            var tokenDto = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return tokenDto;
        }
    }
}