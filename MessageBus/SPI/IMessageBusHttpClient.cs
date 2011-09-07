using System.Net;
using System.Web.Script.Serialization;
using MessageBus.API;

namespace MessageBus.SPI {
    /// <summary>
    /// Internal Service Provider interface to handle the physical transmission of messages over the wire.
    /// </summary>
    public interface IMessageBusHttpClient {
        JavaScriptSerializer Serializer { get; set; }
        string Domain { set; }
        string Path { set; }
        IWebProxy Proxy { set; }
        ICredentials Credentials { set; }
        bool SslVerifyPeer { set; }
        BatchEmailResponse SendEmails(BatchEmailRequest batchEmailRequest);
    }
}