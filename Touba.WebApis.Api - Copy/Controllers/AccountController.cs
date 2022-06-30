using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Touba.WebApis.API.Models;
using Touba.WebApis.API.Services;
using Touba.WebApis.API.Models.Account;
using Touba.WebApis.API.Data;
using Microsoft.AspNetCore.Cors;
using System.Linq;
using System;
using MassTransit;
using Touba.WebApis.Helpers.MessageBroker.Models.Notification;
using System.Web;
using System.Security.Claims;
using Touba.WebApis.API.Helpers.WebApiHelper;
using Newtonsoft.Json;
using RestSharp;
using Touba.WebApis.Helpers.MessageBroker.Models.Account;

namespace Touba.WebApis.API.Controllers
{
    [EnableCors("_allowedOrigins")]
    [ApiController]
    [Route("ToubaWebApis/api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AccountController : ControllerBase
    {
        string SendEmailUrl = "";


        //private readonly string CheckIfUserIsDraftApiUrl = "customer/api/v1/Customer";
        //private readonly string SaveAsDraftCustomerUrl = "customer/api/v1/Customer/SaveAsDraft";
        //private readonly string SaveCustomerUrl = "customer/api/Customer/v1/Save";
        //private readonly string GetByUserIdUrl = "customer/api/v1/Customer";
        //private readonly string SendEmailUrl = "Notification/api/Email/SendEmail";

        private readonly ILogger logger;
        private readonly IRequestClient<MB_EmailSend> emailSendClient;

        public IConfiguration configuration { get; }
        private static AppSettings _appSettings;
        private readonly IAccountService accountService;

        public AccountController(IConfiguration configuration,
                              ILogger logger,
                              IRequestClient<MB_EmailSend> emailSendClient,
                              IAccountService accountService
                              )
        {
            this.configuration = configuration;
            this.logger = logger;
            this.emailSendClient = emailSendClient;
            this.accountService = accountService;

        }

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Login
        ///     {        
        ///       "username": "user1",
        ///       "password": "Abc$123456",
        ///       "ClientId": "05vt4ByxkkWeQUCOzO5IIQ",
        ///       "RememberLogin" : false
        ///     }
        /// </remarks>
        /// <param name="LoginRequest" example="false">login request</param>
        [HttpPost("Login")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken] //Prevent XSRF attack
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            _appSettings = configuration.Get<AppSettings>();
            var response = await accountService.LoginAsync(loginRequest);
           /* if (!response.Succeeded && response.PhoneNumberNotConfirmed)
            {
                response.HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(response);
            }
            else*/ if (!response.Succeeded)
            {
                response.HttpStatusCode = System.Net.HttpStatusCode.OK;
                return Ok(response);
                //return BadRequest(response);
            }

           

            return Ok(response);
        }

      

        /// <summary>
        /// Register
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Register
        ///     {       "username": "string",
        ///             "email": "string",
        ///             "phoneNumber": "string",
        ///             "dateOfBirth": "2021-02-26T13:36:15.132Z",
        ///             "password": "string",
        ///             "firstName": "string",
        ///             "lastName": "string",
        ///             "middleName": "string",
        ///             "roleName": "string",
        ///             "profileImageUrl": "string",
        ///             "isTemporaryCustomer": true,
        ///             "isActive": true
        ///     }
        /// </remarks>
        [HttpPost("Register")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {

            var response = await accountService.RegisterAsync(request);         
            return Ok(response);//StatusCode((int)response.HttpStatusCode, response.Message);
       
    
        }

      

        [HttpPost("ChangePhoneNumber")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePhoneNumber([FromBody] ChangePhoneNumberRequest request)
        {
            var response = await accountService.ChangePhoneNumberAsync(request);
            if (!response.Succeeded)
            {
                response.HttpStatusCode = System.Net.HttpStatusCode.OK;
                return Ok(response);
            }
            response.HttpStatusCode = System.Net.HttpStatusCode.OK;
            return Ok(response);
        }

       

        [HttpPost("ConfirmPhoneNumber")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmPhoneNumber(ConfirmPhoneNumberRequest request)
        {
            var response = await accountService.ConfirmPhoneNumberAsync(request);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);

            return Ok(response);
        }

     

        /// <summary>
        /// ChangeEmail
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/ChangeEmail
        ///     {                
        ///       "email": "string",
        ///       "userName": "string"
        ///     }
        /// </remarks>
        [HttpPost("ChangeEmail")]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailRequest request)
        {
            /*var changeEmailTokenResponse = await accountService.GenerateChangeEmailTokenAsync(request, User.Identity.Name);
            if (!changeEmailTokenResponse.Succeeded)
                return StatusCode((int)changeEmailTokenResponse.HttpStatusCode, changeEmailTokenResponse.Message);

            var token = changeEmailTokenResponse.Message;
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = request.Email }, Request.Scheme);

            var emailProvider = new MessageProtocolProvider(configuration, logger).Instance(MessageProtocol.Email);

            var result = await emailProvider.SendMessage(AccountOptions.ConfirmationPasswordSubject, string.Format(AccountOptions.ConfirmationEmailBody, link), request.Email);
            if (!result)
            {
                logger.Error(string.Format(AccountOptions.EmailProviderServiceDown, DateTime.Now, User.Identity.Name));
                return StatusCode(500, AccountOptions.PublicErrorMessage);
            }*/

            return Ok(AccountOptions.CheckYoutEmailForChangeEmail);
        }

        /// <summary>
        /// ConfirmEmail
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/ConfirmEmail
        ///     {        
        ///       "token": "string",
        ///       "email": "string"
        ///     }
        /// </remarks>
        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var response = await accountService.ConfirmEmailAsync(token, email);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);
            return Ok(response);
        }

       

        /// <summary>
        /// Retrieves the user profile
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/Account/Profile
        ///     {        
        ///       "userId": "1be1ca92-74a9-4ca7-9bb1-9130a007eb86"
        ///     }
        /// </remarks>
        /// <returns>The user profile</returns>
        [HttpGet("Profile")]
        [Authorize(Policy = "NotCustomer")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            var response = await accountService.GetUserProfile(userId);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);
            return Ok(response);
        }

      


        [HttpGet("MyProfile")]
        [Authorize()]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;
            var response = await accountService.GetUserProfile(userId);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);
            return Ok(response);
        }

       

        /// <summary>
        /// Updates the user's profile
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PATCH api/Account/Profile
        ///     {        
        ///       "firstName": "Davood",
        ///       "middleName": "string",
        ///       "lastName": "Pournabi",
        ///       "email": "Davood@gmail.com",
        ///       "gender": "Male",
        ///       "phoneNumber": "555444333",
        ///       "profileImageUrl": "NULL"
        ///     }
        /// </remarks>
        [HttpPost("Profile")]
        [Authorize()]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileRequest request)
        {

            var response = await accountService.UpdateUserProfileAsync(request, User.Identity.Name);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);
            return Ok(response);
        }

      


        /// <summary>
        /// Retrieves the user roles
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/Account/Profile
        ///     {
        ///       "userId": "1be1ca92-74a9-4ca7-9bb1-9130a007eb86"
        ///     }
        /// </remarks>
        /// <returns>The user roles list</returns>
        [HttpGet("Roles")]
        [Authorize]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var response = await accountService.GetRolesByUserIdAsync(userId);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);
            return Ok(response);
        }

       

        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/ChangePassword
        ///     { 
        ///     //user1
        ///         "currentPassword": "Abc$123456",
        ///         "newPassword": "string"
        ///     }
        /// </remarks>
        [HttpPost("ChangePassword")]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var response = await accountService.ChangePasswordAsync(request, User.Identity.Name);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);
            return Ok(response);
        }

      

        /// <summary>
        /// ForgetPassword
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/ForgetPassword
        ///     {
        ///       "email": "string",
        ///       "username": "string"
        ///     }
        /// </remarks>
        [HttpPost("ForgetPassword")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            try
            {
                _appSettings = configuration.Get<AppSettings>();
                string resetPasswordUrl = _appSettings.ResetPasswordUrl;
                var response = await accountService.GenerateForgetPasswordTokenAsync(request);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);

                var token = HttpUtility.UrlEncode(response.Message);

                var link = $@"
                    <a href='{resetPasswordUrl}?token={token}&email={request.Email}'>
                        Click here to reset password
                    </a>";
                MB_EmailSend mB_EmailSend = new MB_EmailSend
                {
                    Body = link,
                    EmailReceiver = request.Email,
                    Subject = "Reset Password Touba",
                };
                string mBEmailSendSerializeObject = JsonConvert.SerializeObject(mB_EmailSend);
                var sendEmailResultResult = WebApiCallHelper.CallApiWithPostJson(_appSettings.APIGatewayBaseAddress + "/" + SendEmailUrl, mBEmailSendSerializeObject, null);

                if (sendEmailResultResult.ResponseCode == "OK")

                {
                    return Ok();
                }
                else
                {

                    return BadRequest(sendEmailResultResult.ResponseContent);
                }


               
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }



           

            ///var res = emailService.SendAsync(mB_EmailSend);
            //var res = await emailSendClient.GetResponse<MB_EmailSend>(new MB_EmailSend
            //{
            //    Body = link,
            //    EmailReceiver = request.Email,
            //    Subject = "Reset Password Touba", 
            //});

          //  return Ok(new { IsSend = res });
        }

    
        /// <summary>
        /// ResetPassword
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/ResetPassword
        ///     {        
        ///       "password": "string",
        ///       "confirmPassword": "string",
        ///       "email": "string",
        ///       "token": "string"
        ///     }
        /// </remarks>
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel request)
        {
            var response = await accountService.ResetPasswordAsync(request);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);
            return Ok(response);
        }

      
     

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);

            return Ok(AccountOptions.LoggedOutSuccessfully);
        }

       


      
    }

   
}
