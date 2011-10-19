using System.Collections.Generic;
using MessageBus.API.V3;

namespace MessageBus.SPI {
    /// <summary>
    /// Used internally to format messages in JSON for transmission on the wire.
    /// </summary>
    public sealed class BatchTemplateMessage {
        public BatchTemplateMessage() {
            // required no-arg constructor - used for testing
        }

        public BatchTemplateMessage(MessageBusTemplateEmail email) {
            mergeFields = email.MergeFields;
            toEmail = email.ToEmail;
            toName = email.ToName;
            customHeaders = email.CustomHeaders;
            templateKey = email.TemplateKey;
        }

        public string toEmail { get; set; }
        public string templateKey { get; set; }
        public string toName { get; set; }
        public Dictionary<string, string> mergeFields { get; private set; }
        public Dictionary<string, string> customHeaders { get; private set; }
    }
}