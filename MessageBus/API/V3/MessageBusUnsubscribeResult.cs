using System;
using MessageBus.SPI;

namespace MessageBus.API.V3 {
    public class MessageBusUnsubscribeResult {
        public MessageBusUnsubscribeResult(UnsubscribeResponseResult result) {
            ToEmail = result.toEmail;
            Time = result.time;
        }

        public string ToEmail { get; private set; }
        public DateTime Time { get; private set; }
    }
}