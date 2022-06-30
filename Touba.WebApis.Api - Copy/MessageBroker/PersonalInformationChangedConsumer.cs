using Touba.WebApis.Helpers.MessageBroker.Models;
using Touba.WebApis.API.Models.Account;
using Touba.WebApis.API.Services;
using MassTransit;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Touba.WebApis.API.MessageBroker
{
    public class PersonalInformationChangedConsumer : IConsumer<PersonalInformationChanged>
    {
        private readonly ILogger logger;
        private readonly IAccountService _accountService;

        public PersonalInformationChangedConsumer(ILogger logger,
            IAccountService accountService)
        {
            this.logger = logger;
            this._accountService = accountService;
        }

        public async Task Consume(ConsumeContext<PersonalInformationChanged> context)
        {
            try
            {
                var message = context.Message;

                logger.Debug($"Receive PersonalInformationChanged {message.FirstName} {message.LastName}");

                //perosnal info not related to user
                if (string.IsNullOrWhiteSpace(message.UserId))
                {
                    return;
                }

                var user = await _accountService.GetUserInfoAsync(context.Message.UserId);

                await _accountService.SyncUserProfileAsync(new UpdateUserProfileRequest
                {
                    Gender = message.Gender , 
                    FirstName = message.FirstName,
                    LastName = message.LastName,
                    MiddleName = message.MiddleName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    ProfileImageUrl = user.ProfileImageUrl,

                }, user.UserName );
               
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in GetRoleByUserIDConsumer");
            }

        }
    }
}
