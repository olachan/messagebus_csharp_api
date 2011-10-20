using System;

namespace MessageBus.SPI
{
    public class MailingListEntryDeleteResponse {
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public DateTime statusTime { get; set; }
    }
}