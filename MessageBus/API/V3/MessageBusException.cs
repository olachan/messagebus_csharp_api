using System;

namespace MessageBus.Impl
{
    public class MessageBusException : Exception {
        public MessageBusException(int statusCode, string statusMessage)
            : base(String.Format("Communication Failed with error code: {0} - {1}", statusCode, statusMessage)) {

            }
    }
}