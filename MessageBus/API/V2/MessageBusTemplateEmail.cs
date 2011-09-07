using System.Collections.Generic;

namespace MessageBus.API.V2 {
    public sealed class MessageBusTemplateEmail {
        public MessageBusTemplateEmail() {
            MergeFields = new Dictionary<string, string>();
        }

        public Dictionary<string, string> MergeFields { get; private set; }
    }
}