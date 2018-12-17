using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ramsey.Shared.Dto
{
    public class RecipeDtoV2
    {
        public string RecipeID { get; set; }
        public string Name { get; set; }

        public List<string> Ingredients { get; set; }
        public List<RecipePartDtoV2> RecipeParts { get; set; }

        public List<string> Directions { get; set; }

        [DefaultValue("")]
        public string Desc { get; set; }

        public string Source { get; set; }
        public RecipeProvider Owner { get; set; }
        public string OwnerLogo { get; set; }
        public string Image { get; set; }
    }
}
