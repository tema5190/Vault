using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Vault.API
{
    public class AuthOptions
    {
        public const string ISSUER = "VaultUser";
        public const string AUDIENCE = "http://vault.com";
        const string KEY = "qwertyuiop[]';lkjhgfdsazxcvbnm,./";
        public const int LIFETIME = 5;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
