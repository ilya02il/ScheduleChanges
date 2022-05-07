using System.Threading.Tasks;

namespace Domain.Events
{
    public delegate Task<bool> CheckingChatForExistingEventHandler(object sender, CheckingChatForExistingEventArgs e);

    public class CheckingChatForExistingEventArgs
    {
        public long ChatId { get; }

        public CheckingChatForExistingEventArgs(long chatId)
        {
            ChatId = chatId;
        }
    }
}
