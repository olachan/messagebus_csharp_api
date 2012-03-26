using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageBus.SPI
{
    public class CampaignStatusResponse
    {
        public Boolean completed { get; set; }
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public DateTime statusTime { get; set; }
    }
}
