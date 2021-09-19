namespace Models.SettingsModels
{
    public class TinkoffSettings
    {
        public static string Section = "TinkoffSettings";
        public TinkoffBrokerTokenFilePath TokenPath { get; set; }
        public string TinkoffOpenApiBaseAdress { get; set; }
    }
}
