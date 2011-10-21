using System;

namespace MessageBus.SPI {
    public class ErrorResponse {
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public DateTime statusTime { get; set; }
    }
}