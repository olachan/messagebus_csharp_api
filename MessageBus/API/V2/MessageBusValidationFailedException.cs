using System;

namespace MessageBusTest.Impl {
    public class MessageBusValidationFailedException : Exception {
        public MessageBusValidationFailedException(string message)
            : base(message) {
        }
    }
}