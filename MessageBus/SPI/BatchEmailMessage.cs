using System.Collections.Generic;
using MessageBus.API.V2;

namespace MessageBus.SPI {
    /// <summary>
    /// Used internally to format messages in JSON for transmission on the wire.
    /// </summary>
    public sealed class BatchEmailMessage {
        public BatchEmailMessage() {
            // required no-arg constructor - used for testing
        }

        public BatchEmailMessage(MessageBusEmail email) {
            toEmail = email.ToEmail;
            subject = email.Subject;
            plaintextBody = email.PlaintextBody;
            htmlBody = email.HtmlBody;
            mergeFields = new Dictionary<string, string>(0);
        }

        public BatchEmailMessage(MessageBusTemplateEmail email) {
            mergeFields = email.MergeFields;
        }

        public string toEmail { get; set; }
        public string subject { get; set; }
        public string plaintextBody { get; set; }
        public string htmlBody { get; set; }
        public Dictionary<string, string> mergeFields { get; private set; }
    }
}