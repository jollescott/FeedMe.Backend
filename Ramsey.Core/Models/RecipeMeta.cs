using Ramsey.Core.Models;
using Ramsey.Shared.Enums;
using System.Collections.Generic;

namespace Ramsey.NET.Models
{
    public class RecipeMeta
    {
        public string RecipeId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<RecipePart> RecipeParts { get; set; }
        public virtual ICollection<RecipeTag> RecipeTags { get; set; }

        public string Source { get; set; }
        public RecipeProvider Owner { get; set; }
        public RamseyLocale Locale { get; set; }
        public string OwnerLogo { get; set; }
        public string Image { get; set; }
        public double Rating { get; set; }
    }
}
