using System.Collections.Generic;
using System.Security.Principal;
using Vault.DATA.Enums;

namespace Vault.DATA.DTOs.Auth
{
    public class UserPrincipal : IPrincipal
    {
        public int UserId { get; set; }

        public List<UserRoles> Roles { get; set; }

        public IIdentity Identity => new UserIdentity(this.UserId.ToString());

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}
