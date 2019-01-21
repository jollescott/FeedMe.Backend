using Ramsey.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Models
{
    public class BadWord
    {
        public int BadWordId { get; set; }
        public string Word { get; set; }
        public RamseyLocale Locale { get; set; }
    }
}
