using System;

namespace MessageBus.SPI
{
    public class StatsResponseResult
    {
        public DateTime date { get; set; }
        public int sent { get; set; }
        public int errors { get; set; }
        public int opens { get; set; }
        public int uniqueOpens { get; set; }
        public int clicks { get; set; }
    }
}