using System;
using Telegram.Bot.Polling;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Extensions.DependencyInjection;
using ConsoleTelegaBot2.Services;
using ConsoleTelegaBot2.Worker;
using System.Reflection.Metadata;
using ConsoleTelegaBot2.Handler;

// Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
ITelegramBotClient _botClient;
        // Это объект с настройками работы бота. Здесь мы будем указывать, какие типы Update мы будем получать, Timeout бота и так далее.
        ReceiverOptions _receiverOptions;
        Worker _worker;
        Handler _handler;

            var services = ServiceProviderBuilder.GetServiceProvider(args);
            var option = services.GetRequiredService<IOptions<OptionBot>>();

            Console.WriteLine($"Token: {option.Value.TokenBot}");
 
            _botClient = new TelegramBotClient(option.Value.TokenBot); // Присваиваем нашей переменной значение, в параметре передаем Token, полученный от BotFather
            _receiverOptions = new ReceiverOptions // Также присваем значение настройкам бота
            {
                AllowedUpdates = new[] // Тут указываем типы получаемых Update`ов, о них подробнее расказано тут https://core.telegram.org/bots/api#update
                {
                UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
                UpdateType.CallbackQuery // Inline кнопки
            },
                // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
                // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
                ThrowPendingUpdates = true,
            };

            using var cts = new CancellationTokenSource();

            _worker = new Worker(_botClient, _receiverOptions);
            _handler = new Handler();


            // UpdateHander - обработчик приходящих Update`ов
            // ErrorHandler - обработчик ошибок, связанных с Bot API
            _botClient.StartReceiving(_handler.UpdateHandler, _handler.ErrorHandler, _receiverOptions, cts.Token); // Запускаем бота

            var me = await _botClient.GetMeAsync(); // Создаем переменную, в которую помещаем информацию о нашем боте.
            Console.WriteLine($"{me.FirstName} запущен!");

            await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно

        Console.ReadLine();
 