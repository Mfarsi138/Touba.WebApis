using Touba.WebApis.IdentityServer.DataLayer.Models;
using Touba.WebApis.IdentityServer.DataLayer;
using Touba.WebApis.IdentityServer.API.Models.Message;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Touba.WebApis.API.Services;
using Touba.WebApis.API.Models;
using Touba.WebApis.API.Models.Enums;
using Touba.WebApis.API.Helpers.WebApiHelper;
using Touba.WebApis.Helpers.MessageBroker.Models.Notification;
using Newtonsoft.Json;

namespace Touba.WebApis.API.Services.Implementation
{
    public class MessageService : IMessageService
    {
        private readonly Serilog.ILogger _logger;
        private readonly AppSettings _appSettings;
        private readonly DataContext _context;
        private readonly string SendSmsUrl = "Notification/api/Message/SendMessage";


        public MessageService(Serilog.ILogger logger, IOptions<AppSettings> options, DataContext context)
        {
            _logger = logger;
            _appSettings = options.Value;
            _context = context;
        }

        public async Task AddAsync(Message message)
        {
            _context.Message.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<MessageReponseModel> SendAsync1(MessageSendModel messageSendModel)
        {
            Message message = new Message()
            {
                Id = Guid.NewGuid().ToString(),
                Sender = _appSettings.MessageConfiguration.sender,
                Receiver = messageSendModel.Receiver,
                Body = messageSendModel.Body,
                Command = MessageCmd.sendSMS.GetEnumDescription(),
                Date = DateTime.Now
            };

            try
            {
                HttpClient httpClient = new HttpClient();
                /*var response = await httpClient.GetAsync(_appSettings.MessageConfiguration.url +
                    "username=" + _appSettings.MessageConfiguration.username +
                    "&password=" + _appSettings.MessageConfiguration.password +
                    "&cmd=" + MessageCmd.sendSMS.GetEnumDescription() +
                    "&to=" + messageSendModel.Receiver +
                    "&sender=" + _appSettings.MessageConfiguration.sender +
                    "&message=" + messageSendModel.Body +
                    "&uniCode=" + messageSendModel.UniCode);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("send message operation failed");

                if ((int)response.StatusCode != 200)
                    throw new Exception("send message operation failed");

                message.IsSend = true;
                await AddAsync(message);

                string responseContent = await response.Content.ReadAsStringAsync();

                _logger.Debug("message send successfully");
                return new MessageReponseModel()
                {
                    Response = responseContent,
                    IsSend = true
                };*/

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get,
                    _appSettings.MessageConfiguration.url +
                    "username=" + _appSettings.MessageConfiguration.username +
                    "&password=" + _appSettings.MessageConfiguration.password +
                    "&cmd=" + MessageCmd.sendSMS.GetEnumDescription() +
                    "&to=" + messageSendModel.Receiver +
                    "&sender=" + _appSettings.MessageConfiguration.sender +
                    "&message=" + messageSendModel.Body +
                    "&uniCode=" + messageSendModel.UniCode);
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = await httpClient.SendAsync(requestMessage))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new Exception("send message operation failed");

                    if ((int)response.StatusCode != 200)
                        throw new Exception("send message operation failed");

                    message.IsSend = true;
                    await AddAsync(message);

                    var responseContent = await response.Content.ReadAsStringAsync();
                    //var model = JsonConvert.DeserializeObject(content);

                    return new MessageReponseModel()
                    {
                        Response = responseContent,
                        IsSend = true
                    };
                }
            }
            catch (Exception ex)
            {
                message.IsSend = false;
                await AddAsync(message);

                _logger.Error(ex, "an error occurred in MessageService");
                return new MessageReponseModel()
                {
                    IsSend = false
                };
            }
        }

        public async Task<MessageReponseModel> SendAsync(MessageSendModel messageSendModel)
        {
            MB_SmsSend message = new MB_SmsSend()
            {
              
                Receiver = messageSendModel.Receiver,
                Body = messageSendModel.Body,
                UniCode="UTF8"
            };
            string mBSmsSendSerializeObject = JsonConvert.SerializeObject(message);

            try
            {
                var sendEmailResultResult = WebApiCallHelper.CallApiWithPostJson(_appSettings.APIGatewayBaseAddress + "/" + SendSmsUrl, mBSmsSendSerializeObject, null);

                    return new MessageReponseModel()
                    {
                        IsSend = true
                    };
                
            }
            catch (Exception ex)
            {
             

                _logger.Error(ex, "an error occurred in MessageService");
                return new MessageReponseModel()
                {
                    IsSend = false
                };
            }
        }

    }
}
