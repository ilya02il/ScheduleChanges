using ChatBot;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebAPI.Configurations;
using WebAPI.Keyboard;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route(ApiBaseRoute.BaseRoute + "/tg-bot")]
    public class TelegramBotController : ControllerBase
    {
        private readonly TelegramBotConfiguration _botConfig;
        private readonly TelegramBotService _tgBotService;
        private readonly GrpcChatBot.GrpcChatBotClient _chatBotClient;

        public TelegramBotController(TelegramBotService tgBotService,
            GrpcChatBot.GrpcChatBotClient chatBotClient,
            IConfiguration configuration)
        {
            _tgBotService = tgBotService;
            _chatBotClient = chatBotClient;
            _botConfig = configuration.GetSection("TelegramBotConfiguration")
                .Get<TelegramBotConfiguration>();
        }

        [HttpPost("{botToken}")]
        public async Task HandleMessage([FromRoute] string botToken,
            [FromBody] Update update,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(botToken) || botToken != _botConfig.Token)
                throw new ArgumentException("Token: {token} is not valid for a bot.", botToken);

            if (update is null || update.Message.Type is not MessageType.Text)
                return;

            var button = KeyboardButtonsProps.GetButtonFromTitle(update.Message.Text);
            var command = button is null ? update.Message.Text : KeyboardButtonsProps.GetButtonCommand(button ?? default);

            var request = new HandleMessageRequest()
            {
                ChatId = update.Message.Chat.Id,
                Username = update.Message.Chat.Username ?? string.Empty,
                Date = update.Message.Date.ToTimestamp(),
                Text = command
            };

            var response = await _chatBotClient.HandleMessageAsync(request, cancellationToken: cancellationToken);

            await _tgBotService.SendTextMessageAsync(response.ChatId,
                response.Messages,
                response.KeyboardButtons,
                cancellationToken);
        }
    }
}
