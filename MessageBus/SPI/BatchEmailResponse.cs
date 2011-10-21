using System;

namespace MessageBus.SPI {
    /// <summary>
    /// used internally to translate JSON responses into consumable format
    /// </summary>
    public class BatchEmailResponse {
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public DateTime statusTime { get; set; }
        public int successCount { get; set; }
        public int failureCount { get; set; }
        public BatchEmailResult[] results { get; set; }

    }

    /// <summary>
    /// used internally to translate JSON responses for each message into consumable format
    /// </summary>
    public class BatchEmailResult {
        public string toEmail { get; set; }
        public string messageId { get; set; }
        public int messageStatus { get; set; }
    }
}