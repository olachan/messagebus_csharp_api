using System;
using System.Collections.Generic;
using System.Net;

namespace MessageBus.API.V2 {
    /// <summary>
    /// Defines the methods required for sending email via the MessageBus ReST API.
    /// </summary>
    public interface IMessageBusClient : IDisposable {

        /// <summary>
        /// The API key for your account
        /// </summary>
        string ApiKey { get; }

        /// <summary>
        /// Read-only.  The API version.
        /// </summary>
        string ApiVersion { get; }

        /// <summary>
        /// Optional.  The template key to use for the merge.
        /// Note:  If this is specified that plaintextBody and htmlBody are not used.
        /// </summary>
        string TemplateKey { get; set; }

        /// <summary>
        /// The from email address. e.g. bobsmith@example.com
        /// </summary>
        string FromEmail { get; set; }

        /// <summary>
        /// The from name. e.g. Bob Smith
        /// </summary>
        string FromName { get; set; }

        /// <summary>
        /// The tags to associate with this email for statistical analysis.
        /// </summary>
        string[] Tags { get; set; }

        /// <summary>
        /// Custom headers to include with the email.  To add a customer header call CustomHeaders.Add("{headerName}", "{headerValue}")
        /// </summary>
        Dictionary<string, string> CustomHeaders { get; }

        /// <summary>
        /// The the proxy to use for Http connections.
        /// </summary>
        IWebProxy Proxy { set; }

        /// <summary>
        /// The Number of emails to buffer before sending over the wire.
        /// </summary>
        int EmailBufferSize { get; set; }

        /// <summary>
        /// Flush any email waiting to be sent.  Returns true if the operation caused email to be transferred to the server.
        /// </summary>
        /// <returns>true if the operation results in a transmission</returns>
        bool Flush();

        /// <summary>
        /// Send the specified email.  Note that this is a buffered operation.  Email is queued locally until the specified buffer size
        /// is reached and then transferred to the server in a single batch.
        /// </summary>
        /// <param name="email">The email to be sent</param>
        /// <returns>true if this operation resulted in a transmission</returns>
        bool Send(MessageBusEmail email);

        /// <summary>
        /// This event is triggered whenever messages are transmitted to the server.
        /// </summary>
        event MessageTransmissionHandler Transmitted;

        /// <summary>
        /// Flushes and e-mail waiting to be sent.  Any future calls to send will result in an error.
        /// </summary>
        /// <returns>returns true if this operation results in a transmission to the server</returns>
        bool Close();
    }
}