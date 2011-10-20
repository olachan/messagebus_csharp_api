using System;

namespace MessageBus.SPI
{
    public class DeliveryErrorsResponseResult {
        public string toEmail { get; set; }
        public string messageId { get; set; }
        public DateTime time { get; set; }
        public string DSNCode { get; set; }
    }
}