using Touba.WebApis.API.Data;
using Touba.WebApis.API.Helpers.WebApiHelper;
using Touba.WebApis.API.Models;
using Touba.WebApis.API.Models.Account;
using Touba.WebApis.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Touba.WebApis.API.Controllers
{
    [ApiController]
    [Route("ToubaWebApis/api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [EnableCors("_allowedOrigins")]
    //[Authorize(Roles = Roles.Administrator)]
    public class AdministratorController : Controller
    {
        private readonly string ERPGetApiUrl = "IdentityServer/api/Administrator";

        public IConfiguration configuration { get; }
        private static AppSettings _appSettings;
        private readonly IAccountService accountService;
        private readonly ILogger logger;

        public AdministratorController(IConfiguration configuration, IAccountService accountService,
            ILogger logger)
        {
            this.configuration = configuration;
            this.accountService = accountService;
            this.logger = logger;
        }

        /// <summary>
        /// SetEmail
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/SetEmail
        ///     {        
        ///       "email": "user1@gmail.com",
        ///       "userName": "user1"
        ///     }
        /// </remarks>
        [HttpPost("SetEmail")]
        public async Task<IActionResult> SetEmail([FromBody] SetEmailRequest request)
        {
            try
            {
                var changeEmailTokenResponse = await accountService.SetEmailAsync(request);
                if (!changeEmailTokenResponse.Succeeded)
                    return StatusCode((int)changeEmailTokenResponse.HttpStatusCode, changeEmailTokenResponse.Message);

                return Ok(AccountOptions.CheckYoutEmailForChangeEmail);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in SetEmail");
                return StatusCode(500);
            }
        }

       
        [HttpPost("ActiveDeactiveUser")]
        public async Task<IActionResult> ActiveDeactiveUser([FromBody] DeactiveUserRequest request)
        {
            try
            {
                var response = await accountService.ActiveDeactiveUserAsync(request);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in DeactivateUser");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// AddClaim
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/AddClaim
        ///     {        
        ///       "type": "string",
        ///       "value": "string"
        ///     }
        /// </remarks>
        [HttpPost("AddClaim")]
        public async Task<IActionResult> AddClaim([FromBody] NewClaimRequest claim)
        {
            try
            {
                var response = await accountService.AddClaimAsync(claim, claim.UserName);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in AddClaim");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// RemoveClaim
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/RemoveClaim
        ///     {
        ///     //username: administrator
        ///       "type": "x",
        ///       "value": "DAB.Account.List"
        ///     }
        /// </remarks>
        [HttpPost("RemoveClaim")]
        public async Task<IActionResult> RemoveClaim([FromBody] NewClaimRequest claim)
        {
            try
            {
                var response = await accountService.RemoveClaimAsync(claim, claim.UserName);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in RemoveClaim");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// AddClaims
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/AddClaims
        ///     { 
        ///         "type": "string",
        ///         "value": "string"
        ///     }
        /// </remarks>
        [HttpPost("AddClaims")]
        public async Task<IActionResult> AddClaims([FromBody] IEnumerable<NewClaimRequest> claims)
        {
            try
            {
                var response = await accountService.AddClaimsAsync(claims, User.Identity.Name);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in AddClaims");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// RemoveClaims
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/RemoveClaims
        ///     {        
        ///         "type": "string",
        ///         "value": "string"
        ///     }
        /// </remarks>
        [HttpPost("RemoveClaims")]
        public async Task<IActionResult> RemoveClaims([FromBody] IEnumerable<NewClaimRequest> claims)
        {
            try
            {
                var response = await accountService.RemoveClaimsAsync(claims, User.Identity.Name);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in RemoveClaims");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// AddUserToRole
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/AddUserToRole
        ///     {     
        ///       "Counter Staff"
        ///     }
        /// </remarks>
        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole([FromBody] string role)
        {
            try
            {
                var response = await accountService.AddUserToRoleAsync(role, User.Identity.Name);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in AddUserToRole");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// RemoveUserFromRole
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/RemoveUserFromRole
        ///     { 
        ///       "Counter Staff"
        ///     }
        /// </remarks>
        [HttpPost("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole([FromBody] string role)
        {
            try
            {
                var response = await accountService.RemoveUserFromRoleAsync(role, User.Identity.Name);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in RemoveUserFromRole");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// AddUserToRoles
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/AddUserToRoles
        ///     {        
        ///          "roles": ["Zaka Manager","Board Member"]
        ///     }
        /// </remarks>
        [HttpPost("AddUserToRoles")]
        public async Task<IActionResult> AddUserToRoles([FromBody] AddUserToRolesRequest request)
        {
            try
            {
                var response = await accountService.AddUserToRolesAsync(request, request.UserName);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in AddUserToRoles");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// RemoveUserFromRoles
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/RemoveUserFromRoles
        ///     {        
        ///          "roles": ["Zaka Manager","Board Member"]
        ///     }
        /// </remarks>
        [HttpPost("RemoveUserFromRoles")]
        public async Task<IActionResult> RemoveUserFromRoles([FromBody] AddUserToRolesRequest request)
        {
            try
            {
                var response = await accountService.RemoveUserFromRolesAsync(request, request.UserName);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in RemoveUserFromRoles");
                return StatusCode(500);
            }
        }

        #region Queries

        /// <summary>
        /// GetUserClaims
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/GetUserClaims
        ///     {        
        ///       "pageIndex": 0,
        ///       "pageSize": 0,
        ///       "userName": "string",
        ///       "claimName": "",
        ///       "claimType": "",
        ///       "pagingMode": false
        ///     }
        /// </remarks>
        [HttpPost("GetUserClaims")]
        public async Task<IActionResult> GetUserClaims(GetUserClaimsRequest request)
        {
            try
            {
                var response = await accountService.GetUserClaimsAsync(request);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "An error occurred in GetUserClaims");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GetUserRoles
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/GetUserRoles"
        ///     {        
        ///       "pageIndex": 0,
        ///       "pageSize": 0,
        ///       "userName": "user1",
        ///       "roleName": "",
        ///       "pagingMode": false
        ///     }
        /// </remarks>
        [HttpPost("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(GetUserRolesRequest request)
        {
            try
            {
                var response = await accountService.GetUserRolesAsync(request);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "An error occurred in GetUserRoles");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GetRoleUsers
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/GetRoleUsers
        ///     {        
        ///       "pageIndex": 0,
        ///       "pageSize": 0,
        ///       "roleName": "admin",
        ///       "userName": "",
        ///       "pagingMode": false
        ///     }
        /// </remarks>
        [HttpPost("GetRoleUsers")]
        public async Task<IActionResult> GetRoleUsers(GetRoleUsersRequest request)
        {
            try
            {
                var response = await accountService.GetRoleUsersAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in GetRoleUsers");
                return StatusCode(500);
            }
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                return Ok(await accountService.GetAllUsersAsync());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in GetAllUsers");
                return StatusCode(500);
            }
        }

        [HttpPost("GetPagingUsers")]
        public async Task<IActionResult> GetPagingUsers(GetUserRequest request) => Ok(await accountService.GetAllUsersAsync(request));

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                return Ok(await accountService.GetAllRolesAsync());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in GetAllRoles");
                return StatusCode(500);
            }
        }

        [HttpGet("ERPGetAllRoles")]
        [AllowAnonymous]
        public async Task<IActionResult> ERPGetAllRoles()
        {
            _appSettings = configuration.Get<AppSettings>();
            WebApiResponse getList = new WebApiResponse();
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Dictionary<string, string> paramsDictionary = new Dictionary<string, string>();

                getList = WebApiCallHelper.CallApiWithGetParameters((_appSettings.APIGatewayBaseAddress + "/" + ERPGetApiUrl), "GetAllRoles", paramsDictionary, null);
                responseModel.HttpStatusCode = System.Net.HttpStatusCode.OK;
                if (getList.ResponseCode == "OK")
                {
                    responseModel.Succeeded = true;
                    responseModel.Message = getList.ResponseContent;
                }
                else
                {
                    responseModel.Succeeded = false;
                    responseModel.HttpStatusCode = System.Net.HttpStatusCode.OK;
                    responseModel.Message = getList.ResponseContent;
                    return Ok(responseModel);
                }
                responseModel.Message = getList.ResponseContent;
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                responseModel.Succeeded = false;
                responseModel.HttpStatusCode = System.Net.HttpStatusCode.OK;
                responseModel.Message = ex.ToString();
                return Ok(responseModel);
            }
        }

        [HttpGet("GetAllUserClaims")]
        public async Task<IActionResult> GetAllUserClaims()
        {
            try
            {
                return Ok(await accountService.GetAllUserClaimsAsync());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in GetAllUserClaims");
                return StatusCode(500);
            }
        }

        [HttpGet("GetAllRoleClaims")]
        public async Task<IActionResult> GetAllRoleClaims()
        {
            try
            {
                return Ok(await accountService.GetAllRoleClaimsAsync());
            }
            catch(Exception ex)
            {
                logger.Error(ex, "An error occurred in GetAllRoleClaims");
                return StatusCode(500);
            }
        }

        [Route("GetPagingRoles")]
        [HttpPost]
        public async Task<IActionResult> GetPagingRoles(GetRoleRequest request) => Ok(await accountService.GetAllRolesAsync(request));


        [HttpPost("AddNewRole")]
        public async Task<IActionResult> AddNewRole(string roleName)
        {
            return Ok(await accountService.AddRole(roleName));
        }

        [HttpPost("EditRole")]
        public async Task<IActionResult> AddNewRole(EditRoleNameRequest req)
        {
            return Ok(await accountService.EditRole(req));
        }

        [HttpPost("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            return Ok(await accountService.DeleteRole(id));
        }

        [HttpPost("CreateUserByAdmin")]
        public async Task<ActionResult> CreateUser(CreateUserByAdminRequest request)
        {
            var res = await accountService.CreateUserAsync(request);
            if (res.Succeeded)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        #endregion
    }
}
