using System.Threading;
using System.Threading.Tasks;
using MimeKit;

namespace Touba.WebApis.API.Services.Contracts
{
    public interface IReceivedEmailHandlerService
    {
        Task MessageReceived(
            MimeMessage message,
            CancellationToken cancellationToken
        );
    }
}