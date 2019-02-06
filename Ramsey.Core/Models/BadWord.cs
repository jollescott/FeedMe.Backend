using Ramsey.Shared.Enums;

namespace Ramsey.NET.Models
{
    public class BadWord
    {
        public int BadWordId { get; set; }
        public string Word { get; set; }
        public RamseyLocale Locale { get; set; }
    }
}
