using System;
using System.Collections.Generic;

namespace MessageBus.API.V3 {
    public sealed class MessageBusTemplateEmail {

        public MessageBusTemplateEmail() {
            MergeFields = new Dictionary<string, string>();
            CustomHeaders = new Dictionary<string, string>();
        }

        public Dictionary<string, string> MergeFields { get; private set; }

        public string TemplateKey { get; set; }

        public Dictionary<string, string> CustomHeaders { get; private set; }

        public string ToEmail { get; set; }
        public string ToName { get; set; }
    }
}