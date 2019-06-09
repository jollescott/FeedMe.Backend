using Ramsey.Shared.Enums;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ramsey.Shared.Dto
{
    public class RecipeDto
    {
        public string RecipeId { get; set; }
        public string Name { get; set; }

        public List<string> Ingredients { get; set; }
        public List<string> Directions { get; set; }

        [DefaultValue("")]
        public string Desc { get; set; }

        public string Source { get; set; }
        public RecipeProvider Owner { get; set; }
        public string OwnerLogo { get; set; }
        public string Image { get; set; }
    }
}
