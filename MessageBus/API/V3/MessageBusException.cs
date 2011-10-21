using System;
using System.Net;

namespace MessageBus.API.V3 {
    public class MessageBusException : Exception {
        private readonly int _statusCode;
        private readonly string _statusMessage;

        public MessageBusException(int statusCode, string statusMessage)
            : base(String.Format("Communication Failed with error code: {0} - {1}", statusCode, statusMessage)) {
            _statusCode = statusCode;
            _statusMessage = statusMessage;
        }

        public MessageBusException(WebException e)
            : base(String.Format("Communication with Server Failed with error code: {0}", e.Status)) {
            _statusCode = -1;
            _statusMessage = e.Status.ToString();
        }

        public int StatusCode {
            get { return _statusCode; }
        }

        public string StatusMessage {
            get { return _statusMessage; }
        }

        public bool IsRetryable() {
            return _statusCode > 500 && _statusCode < 600;
        }
    }
}