using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotExperiments
{

    class Program
    {
        public static string[] sqarekam = new string[7] { "https://castlots.org/img/kamasutra/72.jpg",
        "https://castlots.org/img/kamasutra/11.jpg",
        "https://castlots.org/img/kamasutra/54.jpg",
        "https://castlots.org/img/kamasutra/90.jpg",
        "https://castlots.org/img/kamasutra/16.jpg",
        "https://castlots.org/img/kamasutra/96.jpg",
        "https://castlots.org/img/kamasutra/66.jpg"};

        public static string[] listpozesy = new string[7] { "https://castlots.org/img/kamasutra/72.jpg",
        "https://castlots.org/img/kamasutra/11.jpg",
        "https://castlots.org/img/kamasutra/54.jpg",
        "https://castlots.org/img/kamasutra/90.jpg",
        "https://castlots.org/img/kamasutra/16.jpg",
        "https://castlots.org/img/kamasutra/96.jpg",
        "https://castlots.org/img/kamasutra/66.jpg"};

        public static string[] listpozmedium = new string[7] { "https://castlots.org/img/kamasutra/72.jpg",
        "https://castlots.org/img/kamasutra/11.jpg",
        "https://castlots.org/img/kamasutra/54.jpg",
        "https://castlots.org/img/kamasutra/90.jpg",
        "https://castlots.org/img/kamasutra/16.jpg",
        "https://castlots.org/img/kamasutra/96.jpg",
        "https://castlots.org/img/kamasutra/66.jpg"};

        static ITelegramBotClient bot = new TelegramBotClient("_________token______");

        public static ReplyKeyboardMarkup replyKeyboardMarkuprandompoze = new(new[]
        {new KeyboardButton[] { "/easy", "/medium","back" },}){ ResizeKeyboard = true};
        public static ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]{ new KeyboardButton[] { "/flipsqares", "/randompoze" }, new KeyboardButton[] { "/keyadd", "/keydel", "/info" },}){ ResizeKeyboard = true};

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;

                switch (message.Text.ToLower())
                {
                    case "/start":
                        await botClient.SendTextMessageAsync(message.Chat, "Доброе время суток, пользователь!" +
                            "\n Наберите команду /info для отображенения всех команд бота");
                        break;

                    case "/keyadd":

                            Message sentMessage = await botClient.SendTextMessageAsync(
                            message.Chat,
                            text: "Выберите команду",
                            replyMarkup: replyKeyboardMarkup,
                            cancellationToken: cancellationToken);
                        break;

                    case "/keydel":
                        await botClient.SendTextMessageAsync(
                        message.Chat,
                        text: "Клавиатура удалена",
                        replyMarkup: new ReplyKeyboardRemove(),
                        cancellationToken: cancellationToken);
                        break;

                    case "/flipsqares":
                        string photo = Sqare();
                        await botClient.SendPhotoAsync(message.Chat, photo);
                        break;

                    case "/randompoze":
                        await botClient.SendTextMessageAsync(message.Chat, text: "Выберите команду", replyMarkup: replyKeyboardMarkuprandompoze, cancellationToken: cancellationToken);          
                        break;

                    case "/easy":
                        await botClient.SendPhotoAsync(message.Chat, Easy());
                        break;

                    case "/info":
                        await botClient.SendTextMessageAsync(message.Chat, "Полный список команд:" +
                            "\n /flipsqares - Подбрасывает кубик Камасутры и показывает результат" +
                            "\n /randompoz - Отображает любую позы с Камасутры" +
                            "\n /start - Начало работы с ботом" +
                            "\n /info - Список всех команд бота" +
                            "\n /keyadd - Добавляет удобные кнопки для работы с ботом " +
                            "\n /keydel - Удаляет кнопки для рабоы с ботом (после этой команды команды отправляются в ручную" +
                            "");
                        break;
                    case "back": goto case "/keyadd";


                    default:
                        await botClient.SendTextMessageAsync(message.Chat, "Команда не распознана, введите команду /info, чтобы узнать подробнее");
                        break;
                }

         


            }
        }


        public static string Sqare()
        {
            Random random = new Random();
            int i  = random.Next(0, 6);
            return Convert.ToString(sqarekam[i]);
            
        }
        public static string Easy()
        {
            Random random = new Random();
            int i = random.Next(0, 6);
            return Convert.ToString(listpozesy[i]);

        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );



            Console.ReadLine();
        }
    }
}
