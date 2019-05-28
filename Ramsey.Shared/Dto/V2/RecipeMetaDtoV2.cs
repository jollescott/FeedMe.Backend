using Ramsey.Shared.Enums;
using System.Collections.Generic;

namespace Ramsey.Shared.Dto.V2
{
    public class RecipeMetaDtoV2
    {
        public string RecipeId { get; set; }
        public string Name { get; set; }

        public string Source { get; set; }
        public RecipeProvider Owner { get; set; }
        public string OwnerLogo { get; set; }
        public string Image { get; set; }

        public IEnumerable<string> Ingredients { get; set; }
        public IEnumerable<RecipePartDtoV2> RecipeParts { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        
        public double Coverage { get; set; }

        public static implicit operator RecipeMetaDtoV2(RecipeDtoV2 recipe)
        {
            RecipeMetaDtoV2 recipeMetaDto = new RecipeMetaDtoV2
            {
                Name = recipe.Name,
                Image = recipe.Image,
                Owner = recipe.Owner,
                OwnerLogo = recipe.OwnerLogo,
                RecipeId = recipe.RecipeId,
                Source = recipe.Source,
                Ingredients = recipe.Ingredients,
                RecipeParts = recipe.RecipeParts,
                Tags = recipe.Tags
            };

            return recipeMetaDto;
        }
    }
}
