﻿namespace Touba.WebApis.API.Models.Account
{
    public class GetRoleUsersRequest : PaginationModel
    {
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public bool PagingMode { get; set; } = false;
    }
}
