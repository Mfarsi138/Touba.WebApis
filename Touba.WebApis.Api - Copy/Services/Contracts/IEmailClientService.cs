using System;
using System.Collections.Generic;
using System.Threading;
using MimeKit;

namespace Touba.WebApis.API.Services.Contracts
{
    public interface IEmailClientService : IDisposable
    {
        /// <summary>
        /// This method reads messages from a mail server in an async manner.
        /// </summary>
        /// <param name="readCount">The number of message to be read.</param>
        /// <param name="minMessageDateTime">If the pop3 server supports this option, it specified the minimum time of the messages received by the server.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="IAsyncEnumerable{T}"/> to async iterate over it.</returns>
        IAsyncEnumerable<MimeMessage> FetchEmailsFromServer(
            int readCount,
            DateTimeOffset? minMessageDateTime,
            CancellationToken cancellationToken
        );
    }
}