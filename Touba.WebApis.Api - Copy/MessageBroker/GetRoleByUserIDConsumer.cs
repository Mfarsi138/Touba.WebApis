using Touba.WebApis.Helpers.MessageBroker.Models.Account;
using Touba.WebApis.API.Models.Account;
using Touba.WebApis.API.Services;
using MassTransit;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Touba.WebApis.API.MessageBroker
{
    public class GetRoleByUserIDConsumer: IConsumer<MB_GetRoleByUserRequest>
    {
        private readonly ILogger logger;
        private readonly IAccountService _accountService;

        public GetRoleByUserIDConsumer(ILogger logger,
            IAccountService accountService)
        {
            this.logger = logger;
            this._accountService = accountService;
        }

        public async Task Consume(ConsumeContext<MB_GetRoleByUserRequest> context)
        {
            try
            {
                logger.Debug("Getting Role by Userid . MessageId: {MessageId}, Message: {@Message}",
                    context.MessageId, context.Message);

                var data = context.Message;
                GetUserRolesRequest request = new GetUserRolesRequest();
                var res = await _accountService.GetRolesByUserIdAsync(data.userid);
                MB_GetRoleByUserResponse response = new MB_GetRoleByUserResponse();
                response.RoleName = res.ResponseList.FirstOrDefault();

                await context.RespondAsync(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in GetRoleByUserIDConsumer");
            }

        }
    }
}
