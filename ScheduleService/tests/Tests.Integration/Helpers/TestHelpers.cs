using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Integration.Helpers
{
    internal static class TestHelpers
    {
        public static StringContent ToJsonBody<T>(T obj) =>
            new(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

        public async static Task<TOut> ReadAsAsync<TOut>(this HttpContent content) => 
            JsonConvert.DeserializeObject<TOut>(await content.ReadAsStringAsync());
    }
}
