using System;
using System.Collections.Generic;

namespace MessageBus.SPI {
    public class MailingListsResponse {
        public MailingListsResponse() {
            results = new List<MailingListItem>();
        }

        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public DateTime statusTime { get; set; }
        public List<MailingListItem> results { get; set; }
    }
}