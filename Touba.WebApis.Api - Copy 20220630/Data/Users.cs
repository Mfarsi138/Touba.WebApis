using System.Collections.Generic;

namespace Touba.WebApis.API.Data
{
    public class ClaimPermission
    {
        public readonly static string
            AccountCreate = "DAB.Account.Create",
            AccountDeactivate = "DAB.Account.Deactivate",
            AccountList = "DAB.Account.List",
            AccountModify = "DAB.Account.Modify",
            DeactivateUser = "DAB.Account.DeactivateUser",

            CustomerModify = "DAB.Customer.Modify",
            CustomerDelete = "DAB.Customer.Delete",
            CustomerList = "DAB.Customer.List",

            DMSUpload = "DAB.DMS.Upload",
            DMSList = "DAB.DMS.List",

            ZakaRequestCreate = "DAB.Zaka.Request.Create",
            ZakaRequestView = "DAB.Zaka.Request.View",
            ZakaRequestApproveReject = "DAB.Zaka.Request.ApproveReject",
            ZakaRequestModifyState = "DAB.Zaka.Request.ModifyState",

            SeasonalRequestView = "DAB.Seasonal.Request.View",
            SeasonalRequestApproveReject = "DAB.Seasonal.Request.ApproveReject";

    }

    public class UserRole
    {
        public string Name { get; set; }

        public List<string> Permissions { get; set; }

        public readonly static UserRole Admin = new()
        {
            Name = Roles.Administrator,
            Permissions = new List<string>
            {
                ClaimPermission.AccountCreate,
                ClaimPermission.AccountList,
                ClaimPermission.AccountModify,
                ClaimPermission.AccountDeactivate,

                ClaimPermission.CustomerDelete,
                ClaimPermission.CustomerList,
                ClaimPermission.CustomerModify,

                ClaimPermission.DeactivateUser,
                ClaimPermission.DMSList,
                ClaimPermission.DMSUpload,

                ClaimPermission.ZakaRequestApproveReject,
                ClaimPermission.ZakaRequestCreate,
                ClaimPermission.ZakaRequestModifyState,
                ClaimPermission.ZakaRequestView,
            }
        };

        public readonly static UserRole Customer = new()
        {
            Name = Roles.Customer,
            Permissions = new List<string>
            {
                ClaimPermission.AccountCreate,
                ClaimPermission.AccountModify,
                ClaimPermission.ZakaRequestCreate,
                ClaimPermission.ZakaRequestView
            }
        };

        public readonly static UserRole Assessor = new()
        {
            Name = Roles.Assessor,
            Permissions = new List<string>
            {
                ClaimPermission.ZakaRequestView,
                ClaimPermission.ZakaRequestApproveReject
            }
        };

        public readonly static UserRole DepartmentManager = new()
        {
            Name = Roles.DepartmentManager,
            Permissions = new List<string>
            {
                ClaimPermission.ZakaRequestView,
                ClaimPermission.ZakaRequestApproveReject,
                ClaimPermission.ZakaRequestModifyState
            }
        };

        public readonly static UserRole SectorManager = new()
        {
            Name = Roles.SectorManager,
            Permissions = new List<string>
            {
                ClaimPermission.ZakaRequestView,
                ClaimPermission.ZakaRequestApproveReject,
                ClaimPermission.ZakaRequestModifyState
            }
        };

        public readonly static UserRole CEO = new()
        {
            Name = Roles.CEO,
            Permissions = new List<string>
            {
                ClaimPermission.ZakaRequestView,
                ClaimPermission.ZakaRequestApproveReject,
                ClaimPermission.ZakaRequestModifyState
            }
        };

        public readonly static UserRole BoardMember = new()
        {
            Name = Roles.BoardMember,
            Permissions = new List<string>
            {
                ClaimPermission.ZakaRequestView,
                ClaimPermission.ZakaRequestApproveReject
            }
        };

        public readonly static UserRole BoardSupervisor = new()
        {
            Name = Roles.BoardSupervisor,
            Permissions = new List<string>
            {
                ClaimPermission.ZakaRequestView,
                ClaimPermission.ZakaRequestApproveReject
            }
        };

        public readonly static UserRole SeasonalDepManager = new()
        {
            Name = Roles.SeasonalDepManager,
            Permissions = new List<string>
            {
                ClaimPermission.SeasonalRequestView,
                ClaimPermission.SeasonalRequestApproveReject
            }
        };

        public readonly static UserRole SeasonalPurchaseManager = new()
        {
            Name = Roles.PurchaseManager,
            Permissions = new List<string>
            {
                ClaimPermission.SeasonalRequestView,
                ClaimPermission.SeasonalRequestApproveReject
            }
        };
    }
    public class PredefinedUsers
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public UserRole Role { get; set; }

        public static List<PredefinedUsers> GetAll()
        {
            return new List<PredefinedUsers>
            {
                new PredefinedUsers
                {
                    Username = "administrator",
                    Password = "Abc$123456",
                    FirstName="Seyed Amin",
                    LastName="Esmaeily",
                    IsActive=true,
                    MiddleName=string.Empty,
                    Role = UserRole.Admin
                },
                new PredefinedUsers
                {
                    Username = "customer1",
                    Password = "Abc$123456",
                    FirstName="Zaka",
                    LastName="Customer 1",
                    IsActive=true,
                    MiddleName=string.Empty,
                    Role = UserRole.Customer
                }
            };
        }
    }
}
