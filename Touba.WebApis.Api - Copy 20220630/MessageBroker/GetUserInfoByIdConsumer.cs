using Touba.WebApis.Helpers.MessageBroker.Models.Account;
using Touba.WebApis.API.Services;
using MassTransit;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Touba.WebApis.API.MessageBroker
{
    public class GetUserInfoByIdConsumer : IConsumer<MB_GetUserInfoByIdRequest>
    {
        private readonly IAccountService accountService;
        private readonly ILogger logger;

        public GetUserInfoByIdConsumer(IAccountService accountService,
            ILogger logger)
        {
            this.accountService = accountService;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<MB_GetUserInfoByIdRequest> context)
        {
            try
            {
                logger.Debug("A new request received in the GetUserInfoByIdConsumer. The request is, {@Message}", 
                    context.Message);

                var result = await accountService.GetUserInfoAsync(context.Message.UserId);
                logger.Debug("Fetched users for role {UserId}", context.Message.UserId);
                await context.RespondAsync(result);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "An error occurred when consuming the GetUserInfoByIdConsumer.");
                throw;
            }
        }
    }
}
