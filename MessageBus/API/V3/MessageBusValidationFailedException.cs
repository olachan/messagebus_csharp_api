using System;

namespace MessageBus.API.V3 {
    public class MessageBusValidationFailedException : Exception {
        public MessageBusValidationFailedException(string message)
            : base(message) {
        }
    }
}