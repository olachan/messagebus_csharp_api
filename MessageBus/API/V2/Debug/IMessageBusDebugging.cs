using System.Net;

namespace MessageBus.API.V2.Debug {
    public interface IMessageBusDebugging {
        bool SkipValidation { set; }
        string Domain { set; }
        string Path { set; }
        bool SslVerifyPeer { set; }
        IWebProxy Proxy { set; }
        ICredentials Credentials { set; }
    }
}