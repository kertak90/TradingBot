using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.TinkoffOpenApiModels
{
    public class TinkoffSettings
    {
        public static string Section = "TinkoffSettings";
        public string TinkoffBrokerTokenFilePath { get; set; }
        public string TinkoffOpenApiBaseAdress { get; set; }
    }
}
