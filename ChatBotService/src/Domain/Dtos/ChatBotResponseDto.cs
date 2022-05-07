using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Dtos
{
    public class ChatBotResponseDto
    {
        public long ChatId { get; init; }
        public string Username { get; init; }
        public string[] Messages { get; init; }
        public KeyboardButton[] KeyboardButtons { get; init; }

        public ChatBotResponseDto(ChatBotEntity chatBot, string[] messages, KeyboardButton[] keyboardButtons)
        {
            ChatId = chatBot.PlatformSpecificChatId;
            Username = chatBot.UserInfo.Username;
            Messages = messages;
            KeyboardButtons = keyboardButtons;
        }
    }
}
