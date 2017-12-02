using System;
using System.Collections.Generic;
using System.Text;

namespace Vault.DATA.DTOs.Registration
{
    public class FirstStepResultDto
    {
        public bool IsEmailExist { get; set; } = false;
        public bool UserNameNotFound { get; set; } = false;
        public bool IsCompleted
        {
            get
            {
                return !IsEmailExist && !UserNameNotFound;
            }
        }
    }
}
