using System.Threading;
using System.Threading.Tasks;

namespace Touba.WebApis.API.Services.Contracts
{
    /// <summary>
    /// The abstract background job to receive emails from email servers.
    /// It should be scheduled by any background manager the application is using.
    /// </summary>
    public interface IEmailListenerBackgroundService
    {
        /// <summary>
        /// Receives the messages from the email server, then dispatches the receive signal in the application if it's necessary.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Execute(
            CancellationToken cancellationToken
        );
    }
}