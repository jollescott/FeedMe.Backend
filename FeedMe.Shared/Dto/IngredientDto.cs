using System.Collections.Generic;

namespace Ramsey.Shared.Dto
{
    public class IngredientDto
    {
        public string IngredientId { get; set; }
        public List<RecipePartDto> RecipeParts { get; set; }
    }
}
