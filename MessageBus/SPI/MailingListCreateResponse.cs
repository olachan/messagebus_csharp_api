using System;

namespace MessageBus.SPI
{
    public class MailingListCreateResponse
    {
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public DateTime statusTime { get; set; }
        public string key { get; set; }
    }
}