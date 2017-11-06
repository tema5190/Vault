using System.Security.Principal;

namespace Vault.DATA.DTOs.Auth
{
    public class UserIdentity : IIdentity
    {
        public UserIdentity(string name)
        {
            this.Name = name;
        }

        public string AuthenticationType
        {
            get { return "JWT"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get;
            private set;
        }
    }
}
