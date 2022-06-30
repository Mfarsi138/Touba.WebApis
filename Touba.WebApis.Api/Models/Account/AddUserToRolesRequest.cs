using System.Collections.Generic;

namespace Touba.WebApis.API.Models.Account
{
    public class AddUserToRolesRequest
    {
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
