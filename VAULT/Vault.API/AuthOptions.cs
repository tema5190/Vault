using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Vault.API
{
    public class AuthOptions
    {
        public const string ISSUER = "VaultUser";
        public const string AUDIENCE = "http://vaultbank.blackstar.com";
        const string KEY = "pleaseiwanttowinwhat'swrongwithyouguys:(lookatcat=^_^=";
        public const int LIFETIME = 15;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
