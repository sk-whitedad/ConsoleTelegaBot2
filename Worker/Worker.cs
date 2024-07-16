using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace ConsoleTelegaBot2.Worker
{
    public class Worker
    {
        // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
        public ITelegramBotClient _botClient { get; set; }

        // Это объект с настройками работы бота. Здесь мы будем указывать, какие типы Update мы будем получать, Timeout бота и так далее.
        public ReceiverOptions _receiverOptions { get; set; }

        public Worker(ITelegramBotClient client, ReceiverOptions receiverOptions)
        {
            _botClient = client;
            _receiverOptions = receiverOptions;

        }
        

    }
}
