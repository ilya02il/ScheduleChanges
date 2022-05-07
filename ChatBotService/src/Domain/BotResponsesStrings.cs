using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Domain
{
    public static class BotResponsesStrings
    {
        private static readonly string _path = @"C:\Users\ilya0\source\repos\ScheduleChanges\ChatBotService\src\bot_responses.json";
        public static Dictionary<string, string> GetResponsesStrings()
        {
            using var reader = new StreamReader(_path);
            
            var fileText = reader.ReadToEnd();
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileText);

            return result;
        }
    }
}
