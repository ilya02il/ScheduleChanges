using ScheduleChanges.Core.Domain.BaseClasses;

namespace Domain.Enums
{
    public class ChatBotCommands : BaseEnumenation<string>
    {
        public static ChatBotCommands Start => new(0, "/start");
        public static ChatBotCommands EditEducOrg => new(1, "/editeducorg");
        public static ChatBotCommands EditGroupNumber => new(2, "/editgroupnum");
        public static ChatBotCommands Help => new(3, "/help");
        public static ChatBotCommands Exit => new(4, "/exit");

        public ChatBotCommands(int id, string name) : base(id, name) { }
    }
}