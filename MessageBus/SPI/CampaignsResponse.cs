using System;
using System.Collections.Generic;

namespace MessageBus.SPI
{
    public class CampaignsResponse
    {
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public DateTime statusTime { get; set; }
        public int count { get; set; }
        public IList<CampaignsResponseResult> results { get; set; }
    }
}