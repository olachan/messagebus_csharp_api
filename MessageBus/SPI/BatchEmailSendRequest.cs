using System.Collections.Generic;
using MessageBus.API.V3;

namespace MessageBus.SPI {
   /// <summary>
   /// Used internally to format messages in the correct JSON format for transmission to the server.  Note:  property names are in camelCase (not the standard PascalCase)
   /// </summary>
    public sealed class BatchEmailSendRequest {

        private readonly List<BatchEmailMessage> _messages = new List<BatchEmailMessage>();

        public BatchEmailSendRequest() {
        }

        public List<BatchEmailMessage> messages { get { return _messages; } }
    }
}