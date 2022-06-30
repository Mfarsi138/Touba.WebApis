using System;
using System.Collections.Generic;
using System.Text;

namespace Touba.WebApis.Helpers.MessageBroker.Models.Account
{
    public class MB_GetRoleByUserRequest 
    {
        public string userid { get; set; }
        
    }
    public class MB_GetRoleByUserResponse
    {
        public string RoleName { get; set; }

    }
}
