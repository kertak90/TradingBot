using System;

namespace Models.TinkoffOpenApiModels
{
    public class Candle
    {
        public string figi { get; set; }
        public string interval  { get; set; }
        public decimal o { get; set; }
        public decimal c { get; set; }
        public decimal h { get; set; }
        public decimal l { get; set; }
        public decimal v { get; set; }
        public DateTime time { get; set; }
    }
}
