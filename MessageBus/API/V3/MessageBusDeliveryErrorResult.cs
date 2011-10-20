using System;
using MessageBus.SPI;

namespace MessageBus.API.V3 {
    public class MessageBusDeliveryErrorResult {
        public MessageBusDeliveryErrorResult(DeliveryErrorsResponseResult result) {
            ToEmail = result.toEmail;
            MessageId = result.messageId;
            Time = result.time;
            DSNCode = result.DSNCode;
        }

        public string ToEmail { get; private set; }
        public string MessageId { get; private set; }
        public DateTime Time { get; private set; }
        public string DSNCode { get; private set; }
    }
}