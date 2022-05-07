using System;

namespace Application.Common.Helpers
{
    public class CacheKeyHelper
    {
        public static string CreateHashedCacheKeyFromProps<Tin>(Tin obj, params Func<Tin, object>[] props)
        {
            var result = string.Empty;

            foreach (var prop in props)
            {
                result += prop(obj).ToString();
            }

            return GetDEKHash(result).ToString();
        }

        private static int GetDEKHash(string str)
        {
            int hash = str.Length;

            for (int i = 0; i < str.Length; i++)
            {
                hash = ((hash << 5) ^ (hash >> 27)) ^ str[i];
            }

            return hash;
        }
    }
}
