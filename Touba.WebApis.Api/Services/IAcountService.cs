using Touba.WebApis.Helpers.MessageBroker.Models.Account;
using Touba.WebApis.API.Models;
using Touba.WebApis.API.Models.Account;
using Touba.WebApis.IdentityServer.DataLayer.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Touba.WebApis.API.Services
{
    public interface IAccountService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<ResponseModel> RegisterAsync(RegisterUserRequest request);
        Task<ResponseModel> CreateUserAsync(CreateUserByAdminRequest request);
        Task<ResponseModel> GenerateEmailConfirmationTokenAsync(EmailConfirmationRequest request);
        Task<ResponseModel> SetEmailAsync(SetEmailRequest request);
        Task<ResponseModel> GenerateChangeEmailTokenAsync(ChangeEmailRequest request, string userName);
        Task<ResponseModel> ConfirmEmailAsync(string token, string email);
        Task<UserProfileResponse> GetUserProfile(string userId);

        /// <summary>
        /// call from consumer to update profile information (this function not send  change to other microservices)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<ResponseModel> SyncUserProfileAsync(UpdateUserProfileRequest request, string username);

        /// <summary>
        /// call from controller. after update send new chnage to other microservice to sync information
        /// </summary>
        /// <param name="request"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<ResponseModel> UpdateUserProfileAsync(UpdateUserProfileRequest request, string username);
        Task<ResponseModel> ActiveDeactiveUserAsync(DeactiveUserRequest request);
        Task<ResponseModel> ChangePasswordAsync(ChangePasswordRequest request, string username);
        Task<ResponseModel> GenerateForgetPasswordTokenAsync(ForgetPasswordRequest request);
        Task<ResponseModel> ResetPasswordAsync(ResetPasswordModel request);
        Task<ResponseModel> ChangePhoneNumberAsync(ChangePhoneNumberRequest request);
        Task<ResponseModel> ConfirmPhoneNumberAsync(ConfirmPhoneNumberRequest request);
        Task<ResponseModel> AddRole(string roleName);
        Task<ResponseModel> EditRole(EditRoleNameRequest editRole);
        Task<ResponseModel> AddClaimAsync(NewClaimRequest claim, string username);
        Task<ResponseModel> RemoveClaimAsync(NewClaimRequest claim, string username);
        Task<ResponseModel> AddClaimsAsync(IEnumerable<NewClaimRequest> claims, string username);
        Task<ResponseModel> RemoveClaimsAsync(IEnumerable<NewClaimRequest> claims, string username);
        Task<ResponseModel> AddUserToRoleAsync(string role, string username);
        Task<ResponseModel> RemoveUserFromRoleAsync(string role, string username);
        Task<ResponseModel> AddUserToRolesAsync(AddUserToRolesRequest request, string username);
        Task<ResponseModel> RemoveUserFromRolesAsync(AddUserToRolesRequest request, string username);
        Task<ResponseModel<Claim>> GetUserClaimsAsync(GetUserClaimsRequest request);
        Task<ResponseModel<string>> GetUserRolesAsync(GetUserRolesRequest request);
        Task<List<UserResponse>> GetRoleUsersAsync(GetRoleUsersRequest request);
        Task<ResponseModel<GetAllRolesResponse>> GetAllRolesAsync();
        Task<ResponseModel<GetAllRolesResponse>> GetAllRolesAsync(Models.Account.GetRoleRequest request);
        Task<ResponseModel<GetClaimResponse>> GetClaimAsync(Models.Account.GetClaimRequest request);
        Task<ResponseModel<GetAllUsersResponse>> GetAllUsersAsync();
        Task<ResponseModel<GetAllUsersResponse>> GetAllUsersAsync(GetUserRequest request);
        Task<ResponseModel<GetAllRoleClaimsResponse>> GetAllRoleClaimsAsync();
        Task<ResponseModel<GetAllUserClaimsResponse>> GetAllUserClaimsAsync();
        Task<ResponseModel> DeleteRole(string id);

        Task<ResponseModel<string>> GetRolesByUserIdAsync(string userid);
        Task<MB_GetUserInfoByIdResponse> GetUserInfoAsync(string UserId);

    }
}
