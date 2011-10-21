using System;
using System.Collections.Generic;

namespace MessageBus.SPI {
    public class DeliveryErrorsResponse {
        public DeliveryErrorsResponse() {
            results = new List<DeliveryErrorsResponseResult>();
        }

        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public DateTime statusTime { get; set; }
        public List<DeliveryErrorsResponseResult> results { get; set; }
    }
}