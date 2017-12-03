using Vault.DATA.Enums;

namespace Vault.DATA.DTOs.Auth
{
    public class FirstStepRegisterData
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SecondPassword { get; set; }
        public string TwoWayAuthTarget { get; set; } // Phone / Email
        public AuthModelType AuthModelType { get; set; }
    }
}
