using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Domain
{
    public static class BotResponsesStrings
    {
        private static readonly string _path = "\\bot_responses.json";
        public static Dictionary<string, string> GetResponsesStrings()
        {
            var dirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            using var reader = new StreamReader(dirPath + _path);

            var fileText = reader.ReadToEnd();
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileText);

            return result;
        }
    }
}
