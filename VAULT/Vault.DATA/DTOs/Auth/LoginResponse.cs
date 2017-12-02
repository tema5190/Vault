using System;
using System.Collections.Generic;
using System.Text;

namespace Vault.DATA.DTOs.Auth
{
    public class LoginResponse
    {
        public bool IsError { get; set; }
        public object Token { get; set; }
        public bool IsRegistrationNotFinished { get; set; }
    }
}
