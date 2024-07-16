using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleTelegaBot2.Handler
{
    public class Handler
    {
        public async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            var message = update.Message;
                            var user = message.From;
                            Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");
                            var chat = message.Chat;

                            switch (message.Type)
                            {
                                case MessageType.Text:
                                    {
                                        if (message.Text == "/start")
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, "Добро пожаловать в PhotoDream-Bot\n");
                                            await GetReplyKeyboard(botClient, chat);
                                            return;
                                        }

                                        if (message.Text == "ОТМЕНИТЬ")
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, "Вы отменили заказ.\n");
                                            await GetReplyKeyboard(botClient, chat);
                                            return;
                                        }

                                        if (message.Text == "Меню")
                                        {
                                            await GetInlineKeyboard(botClient, chat);
                                            return;
                                        }

                                        if (message.Text == "Настройки")
                                        {
                                            await GetInlineKeyboard(botClient, chat);
                                            return;
                                        }
                                        return;
                                    }
                                case MessageType.Photo:
                                    {
                                        await botClient.SendTextMessageAsync(chat.Id, "Хорошее фото, но лучше отправить как ФАЙЛ. \n");
                                        return;
                                    }

                                case MessageType.Document:
                                    {
                                        await botClient.SendTextMessageAsync(chat.Id, "Фотография принята! \n");
                                        return;
                                    }
                            }
                            return;
                        }

                    case UpdateType.CallbackQuery:
                        {
                            // Переменная, которая будет содержать в себе всю информацию о кнопке, которую нажали
                            var callbackQuery = update.CallbackQuery;
                            // Аналогично и с Message мы можем получить информацию о чате, о пользователе и т.д.
                            var user = callbackQuery.From;
                            // Выводим на экран нажатие кнопки
                            Console.WriteLine($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");
                            // Вот тут нужно уже быть немножко внимательным и не путаться!
                            // Мы пишем не callbackQuery.Chat , а callbackQuery.Message.Chat , так как
                            // кнопка привязана к сообщению, то мы берем информацию от сообщения.
                            var chat = callbackQuery.Message.Chat;
                            // Добавляем блок switch для проверки кнопок
                            switch (callbackQuery.Data)
                            {
                                // Data - это придуманный нами id кнопки, мы его указывали в параметре
                                // callbackData при создании кнопок. У меня это button1, button2 и button3
                                case "buttonNewOrder":
                                    {
                                        // В этом типе клавиатуры обязательно нужно использовать следующий метод
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Нажата кнопка заказ");
                                        // Для того, чтобы отправить телеграмму запрос, что мы нажали на кнопку
                                        await botClient.SendTextMessageAsync(chat.Id, "Выбрана опция печати фото.");
                                        await InlineKeyboardNewOrder(botClient, chat);
                                        return;
                                    }

                                case "buttonPrice":
                                    {
                                        // А здесь мы добавляем наш сообственный текст, который заменит слово "загрузка", когда мы нажмем на кнопку
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Нажата кнопка цены");

                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            $"Вы нажали на {callbackQuery.Data}");
                                        return;
                                    }

                                case "buttonDelivery":
                                    {
                                        // А тут мы добавили еще showAlert, чтобы отобразить пользователю полноценное окно
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Нажата кнопка доставки");
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            $"Вы нажали на {callbackQuery.Data}");
                                        return;
                                    }

                                case "buttonPay":
                                    {
                                        // А тут мы добавили еще showAlert, чтобы отобразить пользователю полноценное окно
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Нажата кнопка оплаты");

                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            $"Вы нажали на {callbackQuery.Data}");
                                        return;
                                    }

                                case "photo10x15":
                                    {
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Выбран размер 10х15");
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Выберите фотографии и отправьте как \"ФАЙЛ\", чтобы избежать потери качества.");
                                        await CreateOrder10x15(botClient, chat);
                                        return;
                                    }

                                case "photo15x21":
                                    {
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Выбран размер 15х21");
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Выберите фотографии и отправьте как \"ФАЙЛ\", чтобы избежать потери качества.");
                                        await CreateOrder15x21(botClient, chat);
                                        return;
                                    }

                                case "photo21x30":
                                    {
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Выбран размер 21х30");
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Выберите фотографии и отправьте как \"ФАЙЛ\", чтобы избежать потери качества.");
                                        await CreateOrder21x30(botClient, chat);
                                        return;
                                    }
                            }

                            return;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task GetReplyKeyboard(ITelegramBotClient botClient, Chat chat)
        {
            var replyKeyboard = new ReplyKeyboardMarkup(
            new List<KeyboardButton[]>()
            {
            new KeyboardButton[]{ new ("Меню"), new("Настройки")}})
            {
                ResizeKeyboard = true,
            };

            await botClient.SendTextMessageAsync(chat.Id, "Для начала работы нажмите \"Меню\"\n", replyMarkup: replyKeyboard); // опять передаем клавиатуру в параметр replyMarkup
        }

        private async Task GetInlineKeyboard(ITelegramBotClient botClient, Chat chat)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>() // здесь создаем лист (массив), который содрежит в себе массив из класса кнопок
                {
                    new InlineKeyboardButton[] // тут создаем массив кнопок
                    {
                       InlineKeyboardButton.WithUrl("Посетить наш сайт", "https://fotka44.ru/"),
                    },
                    new InlineKeyboardButton[] // тут создаем массив кнопок
                    {
                       InlineKeyboardButton.WithCallbackData("Сделать заказ", "buttonNewOrder"),
                    },
                    new InlineKeyboardButton[]
                    {
                       InlineKeyboardButton.WithCallbackData("Цены", "buttonPrice"),
                       InlineKeyboardButton.WithCallbackData("Доставка", "buttonDelivery"),
                       InlineKeyboardButton.WithCallbackData("Оплата", "buttonPay"),
                    }
                });

            await botClient.SendTextMessageAsync(
                chat.Id,
                "Выберите опцию:",
                replyMarkup: inlineKeyboard); // Все клавиатуры передаются в параметр replyMarkup
        }

        private async Task InlineKeyboardNewOrder(ITelegramBotClient botClient, Chat chat)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>() // здесь создаем лист (массив), который содрежит в себе массив из класса кнопок
                {
                    new InlineKeyboardButton[]
                    {
                       InlineKeyboardButton.WithCallbackData("10x15", "photo10x15"),
                       InlineKeyboardButton.WithCallbackData("15x21", "photo15x21"),
                       InlineKeyboardButton.WithCallbackData("21x30", "photo21x30"),
                    }
                });

            await botClient.SendTextMessageAsync(
                chat.Id,
                "Выберите размер фото:",
                replyMarkup: inlineKeyboard); // Все клавиатуры передаются в параметр replyMarkup
        }

        private async Task CreateOrder10x15(ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendTextMessageAsync(
                chat.Id,
                "Печатаем размер 10х15"); // Все клавиатуры передаются в параметр replyMarkup

            var replyKeyboard = new ReplyKeyboardMarkup(
            new List<KeyboardButton[]>()
            {
                new KeyboardButton[]{ new ("ОТМЕНИТЬ"), new("ПОДТВЕРДИТЬ")}})
            {
                ResizeKeyboard = true,
            };
            await botClient.SendTextMessageAsync(chat.Id, "Для подтверждения заказа нажмите \"ПОДТВЕРДИТЬ\"\nДля отмены заказа - \"ОТМЕНИТЬ\"", replyMarkup: replyKeyboard); // опять передаем клавиатуру в параметр replyMarkup

        }

        private async Task CreateOrder15x21(ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendTextMessageAsync(
                chat.Id,
                "Печатаем размер 15x21"); // Все клавиатуры передаются в параметр replyMarkup
        }

        private async Task CreateOrder21x30(ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendTextMessageAsync(
                chat.Id,
                "Печатаем размер 21x30"); // Все клавиатуры передаются в параметр replyMarkup
        }

        public Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

    }
}
