using Application.ChatEntities.Commands.HandleMessageCommand;
using ChatBot;
using Grpc.Core;
using MediatR;
using System.Linq;
using System.Threading.Tasks;
using static ChatBot.HandleMessageResponse.Types;

namespace GrpcAPI.Services
{
    public class ChatBotService : GrpcChatBot.GrpcChatBotBase
    {
        private readonly ISender _sender;

        public ChatBotService(ISender sender)
        {
            _sender = sender;
        }

        public async override Task<HandleMessageResponse> HandleMessage(HandleMessageRequest request, ServerCallContext context)
        {
            var command = new HandleMessageCommand()
            {
                ChatId = request.ChatId,
                PlatformHash = request.PlatformHash,
                Username = request.Username,
                Date = request.Date.ToDateTimeOffset(),
                Text = request.Text
            };

            var result = await _sender.Send(command, context.CancellationToken);

            var keyboardButtons = result.KeyboardButtons.Select(kb => (KeyboardButtons)kb);

            var response = new HandleMessageResponse()
            {
                ChatId = result.ChatId,
                Username = result.Username
            };

            response.Messages.AddRange(result.Messages);
            response.KeyboardButtons.AddRange(keyboardButtons);

            return response;
        }
    }
}
