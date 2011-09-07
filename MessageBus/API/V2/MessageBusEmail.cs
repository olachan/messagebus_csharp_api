using System.Collections.Generic;

namespace MessageBus.API.V2 {
    public sealed class MessageBusEmail {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string PlaintextBody { get; set; }
        public string HtmlBody { get; set; }
    }
}