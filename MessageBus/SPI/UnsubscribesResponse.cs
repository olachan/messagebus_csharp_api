using System;
using System.Collections.Generic;

namespace MessageBus.SPI
{
    public class UnsubscribesResponse
    {
        public UnsubscribesResponse() {
            results = new List<UnsubscribeResponseResult>();
        }

        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public DateTime statusTime { get; set; }
        public List<UnsubscribeResponseResult> results { get; private set; }
    }
}