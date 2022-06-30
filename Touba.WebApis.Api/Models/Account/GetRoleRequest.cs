namespace Touba.WebApis.API.Models.Account
{
    public class GetRoleRequest:PaginationModel
    {
        public string RoleName { get; set; }
        public bool PagingMode { get; set; }
    }
}
