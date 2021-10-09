using System.Threading.Tasks;

namespace TelegramBot.Abstractions
{
    public interface ITelegramBotConnector
    {
        Task SendMessage(string message);
    }
}