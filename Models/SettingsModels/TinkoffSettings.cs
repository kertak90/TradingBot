namespace Models.SettingsModels
{
    public class TinkoffSettings
    {
        public static string Section = "TinkoffSettings";
        public TokenFilePath TokenPath { get; set; }
        public string TinkoffOpenApiBaseAdress { get; set; }
    }
}
