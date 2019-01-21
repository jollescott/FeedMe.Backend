using System;
using System.Collections.Generic;
using System.Text;

namespace Ramsey.Shared.Dto.V2
{
    public class IngredientDtoV2
    {
        public int IngredientId { get; set; }
        public IngredientRole Role { get; set; }
        public List<RecipePartDtoV2> RecipeParts { get; set; }
        public string IngredientName { get; set; }
    }

    public enum IngredientRole
    {
        Include, Exclude
    }
}
