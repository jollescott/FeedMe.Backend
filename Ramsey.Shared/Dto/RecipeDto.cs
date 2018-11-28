using System.Collections.Generic;
using System.ComponentModel;

namespace Ramsey.Shared.Dto
{
    public class RecipeDto
    {
        public int RecipeID { get; set; }
        public string Name { get; set; }

        public List<string> Ingredients { get; set; }
        public List<string> Directions { get; set; }

        [DefaultValue("")]
        public string Desc { get; set; }

        public string Source { get; set; }
        public string Image { get; set; }
    }

    public class RecipeCategoryDto
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public int RecipeID { get; set; }
    }

    public class RecipeDirectionDto
    {
        public int DirectionID { get; set; }
        public string Instruction { get; set; }

        public int RecipeID { get; set; }
    }
}
