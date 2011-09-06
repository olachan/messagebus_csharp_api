using System.Web.Script.Serialization;

namespace MessageBus.SPI {
    /// <summary>
    /// Internal Service Provider interface to handle the physical transmission of messages over the wire.
    /// </summary>
    public interface IMessageBusHttpClient {
        JavaScriptSerializer Serializer { get; set; }
        string Domain { set; }
        string Path { set; }
        BatchEmailResponse SendEmails(BatchEmailRequest batchEmailRequest);
    }
}