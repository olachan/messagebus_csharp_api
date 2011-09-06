namespace MessageBus.SPI {
    /// <summary>
    /// used internally to translate JSON responses into consumable format
    /// </summary>
    public class BatchEmailResponse {
        public string statusMessage { get; set; }
        public int successCount { get; set; }
        public int failureCount { get; set; }
        public BatchEmailResult[] results { get; set; }

    }

    /// <summary>
    /// used internally to translate JSON responses for each message into consumable format
    /// </summary>
    public class BatchEmailResult {
        public string status { get; set; }
        public string statusMessage { get; set; }
        public string messageId { get; set; }
    }
}