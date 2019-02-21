using Ramsey.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ramsey.Core.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public RamseyLocale Locale { get; set; }

        public virtual ICollection<RecipeTag> RecipeTags { get; set; }
    }
}
