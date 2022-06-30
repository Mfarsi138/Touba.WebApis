using Touba.WebApis.IdentityServer.API.Models.Message;
using Touba.WebApis.IdentityServer.DataLayer.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Touba.WebApis.API.Services
{
    public interface IMessageService
    {
        Task AddAsync(Message message);
        Task<MessageReponseModel> SendAsync(MessageSendModel messageSendModel);
    }
}
