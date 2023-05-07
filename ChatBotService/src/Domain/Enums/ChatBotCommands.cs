using Domain.Common;
using System.Collections.Generic;

namespace Domain.Enums
{
    public class ChatBotCommands : Enumenation
    {
        public static ChatBotCommands Start => new(0, "/start");
        public static ChatBotCommands EditEducOrg => new(1, "/editeducorg");
        public static ChatBotCommands EditGroupNumber => new(2, "/editgroupnum");
        public static ChatBotCommands Help => new(3, "/help");
        public static ChatBotCommands Exit => new(4, "/exit");

        public ChatBotCommands(int id, string name) : base(id, name) { }

        public static IEnumerable<ChatBotCommands> GetEqualityComponents()
        {
            yield return Start;
            yield return EditEducOrg;
            yield return EditGroupNumber;
            yield return Help;
            yield return Exit;
        }

        //public static implicit operator string(ChatBotCommands command)
        //{
        //    return command.ToString();
        //}
    }
}