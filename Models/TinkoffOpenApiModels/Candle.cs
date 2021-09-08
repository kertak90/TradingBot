namespace Models.TinkoffOpenApiModels
{
    public class Candle
    {
        public string figi { get; set; }
        public string interval  { get; set; }
        public double o { get; set; }
        public double c { get; set; }
        public double h { get; set; }
        public double l { get; set; }
        public double v { get; set; }
        public string time { get; set; }
    }
}
