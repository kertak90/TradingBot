using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBot.Abstractions;
using TelegramBot.Models;

namespace TelegramBot
{
    public class TelegramBotConnector : ITelegramBotConnector
    {
        private readonly TelegramBotSettings _telegramBotSettings;
        private TelegramBotClient _telegramBotClient;
        public TelegramBotConnector(TelegramBotSettings telegramBotSettings)
        {
            _telegramBotSettings = telegramBotSettings ?? throw new ArgumentNullException(nameof(telegramBotSettings));
            Initialize();
        }
        private void Initialize()
        {
            var token = ReadToken();
            _telegramBotClient = new TelegramBotClient(token);            
            Console.WriteLine();
        }
        private string ReadToken()
        {
            var token = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? File.ReadAllText(_telegramBotSettings.TokenPath.Windows)
                : File.ReadAllText(_telegramBotSettings.TokenPath.Linux);
            return token;
        }
        public async Task SendMessage(string message)
        {
            _telegramBotClient.StartReceiving();
            await _telegramBotClient.SendTextMessageAsync(_telegramBotSettings.ChatId, message);
            _telegramBotClient.StopReceiving();
        }
    }
}
