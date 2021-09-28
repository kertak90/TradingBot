using Models.SettingsModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.Models
{
    public class TelegramBotSettings
    {
        public static string Section = "TelegramBotSettings";
        public TokenFilePath TokenPath { get; set; }
        public string ChatId { get; set; }
    }
}
