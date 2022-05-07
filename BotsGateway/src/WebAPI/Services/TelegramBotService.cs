using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WebAPI.Keyboard;
using static ChatBot.HandleMessageResponse.Types;

namespace WebAPI.Services
{
    public class TelegramBotService
    {
        private readonly ITelegramBotClient _botClient;

        public TelegramBotService(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task SendTextMessageAsync(long chatId,
            IEnumerable<string> messages,
            IEnumerable<KeyboardButtons> keyboardButtons,
            CancellationToken cancellationToken)
        {
            using var enumenator = messages.GetEnumerator();

            var last = !enumenator.MoveNext();
            string currentMessage;

            while (!last)
            {
                Message result;

                currentMessage = enumenator.Current;

                last = !enumenator.MoveNext();

                if (last)
                {
                    var buttons = keyboardButtons.Select(kb =>
                    {
                        return new KeyboardButton(KeyboardButtonsProps.GetButtonTitle(kb));
                    });

                    IReplyMarkup markup = buttons.Any() ? new ReplyKeyboardMarkup(buttons) { ResizeKeyboard = true } : new ReplyKeyboardRemove();

                    result = await _botClient.SendTextMessageAsync(chatId,
                        currentMessage,
                        cancellationToken: cancellationToken,
                        replyMarkup: markup,
                        parseMode: ParseMode.Markdown
                        );
                }

                else
                {
                    result = await _botClient.SendTextMessageAsync(chatId,
                    currentMessage,
                    cancellationToken: cancellationToken,
                    parseMode: ParseMode.Markdown
                    );
                }

                if (result is null)
                    throw new Exception("Message has not been sended.");
            }
        }
    }
}
