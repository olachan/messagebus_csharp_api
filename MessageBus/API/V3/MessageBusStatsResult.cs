using System;
using MessageBus.SPI;

namespace MessageBus.API.V3 {
    public class MessageBusStatsResult {
        public MessageBusStatsResult(StatsResponseResult result) {
            Date = result.date;
            Sent = result.sent;
            Errors = result.errors;
            Opens = result.opens;
            UniqueOpens = result.uniqueOpens;
            Clicks = result.clicks;
        }

        public DateTime Date { get; private set; }
        public int Sent { get; private set; }
        public int Errors { get; private set; }
        public int Opens { get; private set; }
        public int UniqueOpens { get; private set; }
        public int Clicks { get; private set; }
    }
}