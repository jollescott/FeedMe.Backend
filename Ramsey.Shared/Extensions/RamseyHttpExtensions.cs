using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.Shared.Extensions
{
    public static class RamseyHttpExtensions
    {
        public static async Task<string> ReadAsSwedishStringAsync(this HttpContent httpContent)
        {
            var byteArray = await httpContent.ReadAsByteArrayAsync();
            var result = Encoding.GetEncoding("ISO-8859-1").GetString(byteArray, 0, byteArray.Length);
            return result;
        }
    }
}
