using System.Collections.Generic;
using MessageBus.API.V3;

namespace MessageBus.SPI {
   /// <summary>
   /// Used internally to format messages in the correct JSON format for transmission to the server.  Note:  property names are in camelCase (not the standard PascalCase)
   /// </summary>
    public sealed class BatchTemplateSendRequest {

        private readonly List<BatchTemplateMessage> _messages = new List<BatchTemplateMessage>();

        public BatchTemplateSendRequest() {
        }

        public List<BatchTemplateMessage> messages { get { return _messages; } }
    }
}