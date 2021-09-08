using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.TinkoffOpenApiModels
{
    public class CandlesResponse
    {
        public string trackingId { get; set; }
        public string status { get; set; }
        public Candle[] payload { get; set; }
    }
}
