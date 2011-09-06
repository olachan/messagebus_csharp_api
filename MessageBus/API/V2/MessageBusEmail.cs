using System.Collections.Generic;

namespace MessageBus.API.V2 {
    public sealed class MessageBusEmail {
        public MessageBusEmail() {
            MergeFields = new Dictionary<string, string>();
        }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string PlaintextBody { get; set; }
        public string HtmlBody { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string[] Tags { get; set; }
        public Dictionary<string, string> MergeFields { get; private set; }
    }
}