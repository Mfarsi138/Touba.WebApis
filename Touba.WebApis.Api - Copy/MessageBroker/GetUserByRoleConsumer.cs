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
    public class GetUserByRoleConsumer : IConsumer<MB_GetUserByRoleRequest>
    {
        private readonly IAccountService accountService;
        private readonly ILogger logger;

        public GetUserByRoleConsumer(IAccountService accountService,
            ILogger logger)
        {
            this.accountService = accountService;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<MB_GetUserByRoleRequest> context)
        {
            try
            {
                logger.Debug("A new request received in the GetUserByRoleConsumer. The request is, {@Message}", 
                    context.Message);

                var param = new Models.Account.GetRoleUsersRequest
                {
                    RoleName = context.Message.RoleName,
                    PagingMode = false
                };
                var result = await accountService.GetRoleUsersAsync(param);
                logger.Debug("Fetched users for role {RoleName}", context.Message.RoleName);
                await context.RespondAsync(new MB_GetUserByRoleResponse
                {
                    Rows = result
                });
            }
            catch(Exception ex)
            {
                logger.Error(ex, "An error occurred when consuming the GetUserByRoleConsumer.");
                throw;
            }
        }
    }
}
