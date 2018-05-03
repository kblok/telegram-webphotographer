using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace WebPhotographerBot
{
    class Program
    {
        public static string azureFunction = Environment.GetEnvironmentVariable("WEBPHOTOGRAPHER_AZUREFUNCTION");
        static void Main(string[] args)
        {
            string telegramApiKey = Environment.GetEnvironmentVariable("WEBPHOTOGRAPHER_APIKEY");
            var botClient = new TelegramBotClient(telegramApiKey);

            botClient.OnMessage += BotClient_OnMessage;

            botClient.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine("Telegram Bot Started\npress any key to exit");
            Console.ReadLine();
            botClient.StopReceiving();
        }

        private static async void BotClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var bot = (TelegramBotClient)sender;
            
            if (!string.IsNullOrEmpty(e.Message.Text))
            {
                foreach (Match m in linkParser.Matches(e.Message.Text))
                {
                    await bot.SendTextMessageAsync(e.Message.Chat.Id, "Prepping a screenshot for you my friend");

                    var url = (m.Value.StartsWith("http") ? string.Empty : "https://") + m.Value;

                    try
                    {
                        await bot.SendPhotoAsync(
                            e.Message.Chat.Id,
                            new Telegram.Bot.Types.FileToSend(azureFunction + url),
                            m.Value);
                    }
                    catch
                    {
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, "Unable to get a screenshot for you");
                    }
                }
            }
        }
    }
}
