using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;

namespace Domain
{
    public static class BotResponsesStrings
    {
        private const string FileName = "bot_responses.json";
        private static readonly ConcurrentDictionary<string, string> _responses;

        static BotResponsesStrings()
        {
            var dirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            using var reader = new StreamReader(Path.Combine(dirPath, FileName));

            var fileText = reader.ReadToEnd();
            _responses = JsonConvert.DeserializeObject<ConcurrentDictionary<string, string>>(fileText);
        }

        public static ConcurrentDictionary<string, string> ResponsesStrings => _responses;
    }
}
