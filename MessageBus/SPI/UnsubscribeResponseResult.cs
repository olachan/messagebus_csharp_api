using System;

namespace MessageBus.SPI
{
    public class UnsubscribeResponseResult
    {
        public string toEmail { get; set; }
        public DateTime time { get; set; }
    }
}