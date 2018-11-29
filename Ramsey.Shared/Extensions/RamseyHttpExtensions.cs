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
            var bytes = await httpContent.ReadAsByteArrayAsync();
            return Encoding.GetEncoding(1252).GetString(bytes);
        }
    }
}
