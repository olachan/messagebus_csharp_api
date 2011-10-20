using System;
using System.Collections.Generic;

namespace MessageBus.SPI
{
    public class StatsResponse
    {
        public StatsResponse()
        {
            results = new List<StatsResponseResult>();
        }

        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public DateTime statusTime { get; set; }
        public List<StatsResponseResult> results { get; private set; }
    }
}