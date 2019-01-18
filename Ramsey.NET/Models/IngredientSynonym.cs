using Ramsey.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Models
{
    public class IngredientSynonym
    {
        public int IngredientSynonymId { get; set; }
        
        public string Correct { get; set; }
        public string Wrong { get; set; }

        public RamseyLocale Locale { get; set; }
    }
}
