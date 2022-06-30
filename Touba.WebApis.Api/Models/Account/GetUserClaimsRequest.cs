﻿using System.Security.Claims;

namespace Touba.WebApis.API.Models.Account
{
    public class GetUserClaimsRequest : PaginationModel
    {
        public string UserName { get; set; }
        public string ClaimName { get; set; }
        public string ClaimType { get; set; }
        public bool PagingMode { get; set; } = false;
    }
}
