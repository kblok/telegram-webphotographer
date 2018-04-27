using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace WebPhotographerBot
{
    class Program
    {
        static void Main(string[] args)
        {
            string telegramApiKey = Environment.GetEnvironmentVariable("WEBPHOTOGRAPHER_APIKEY");
            var botClient = new TelegramBotClient(telegramApiKey);

            botClient.OnMessage += BotClient_OnMessage;
            Console.WriteLine("Telegram Bot Started\npress any key to exit");
            Console.ReadLine();

        }

        private static void BotClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {

        }
    }
}
