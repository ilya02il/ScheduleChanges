using Domain.Entities;
using Domain.ValueObjects;
using System.Collections.Generic;

namespace Domain.Dtos
{
    public class ChatBotResponseDto
    {
        public long ChatId { get; init; }
        public string Username { get; init; }
        public List<string> Messages { get; init; }
        public KeyboardButton[] KeyboardButtons { get; init; }

        public ChatBotResponseDto(ChatBotEntity chatBot, List<string> messages, KeyboardButton[] keyboardButtons)
        {
            ChatId = chatBot.PlatformSpecificChatId;
            Username = chatBot.UserInfo.Username;
            Messages = messages;
            KeyboardButtons = keyboardButtons;
        }
    }
}
