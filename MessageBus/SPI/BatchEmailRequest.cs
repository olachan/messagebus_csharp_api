using System.Collections.Generic;
using MessageBus.API;
using MessageBus.API.V2;

namespace MessageBus.SPI {
   /// <summary>
   /// Used internally to format messages in the correct JSON format for transmission to the server.  Note:  property names are in camelCase (not the standard PascalCase)
   /// </summary>
    public sealed class BatchEmailRequest {

        private readonly Dictionary<string, string> _customHeaders;
        private readonly List<BatchEmailMessage> _messages = new List<BatchEmailMessage>();

        public BatchEmailRequest() {
            _customHeaders = new Dictionary<string, string>();
        }

        public BatchEmailRequest(IMessageBusClient client) {
            _customHeaders = client.CustomHeaders;
            apiKey = client.ApiKey;
            apiVersion = client.ApiVersion;
            templateKey = client.TemplateKey;
            fromEmail = client.FromEmail;
            fromName = client.FromName;
            tags = client.Tags;
        }

        public string apiKey { get; set; }
        public string apiVersion { get; set; }
        public string templateKey { get; set; }
        public string fromEmail { get; set; }
        public string fromName { get; set; }
        public string[] tags { get; set; }
        public Dictionary<string, string> customHeaders { get { return _customHeaders; } }
        public int messageCount { get { return _messages.Count; } }
        public List<BatchEmailMessage> messages { get { return _messages; } }
    }
}