using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vault.API
{
    public class AuthOptions
    {
        public const string ISSUER = "VaultUser";
        public const string AUDIENCE = "http://vault.com";
        const string KEY = "kulikPetux";
        public const int LIFETIME = 5;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
