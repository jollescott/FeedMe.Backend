using Ramsey.Shared.Enums;
using System.Collections.Generic;

namespace Ramsey.Shared.Dto
{
    public class RecipeMetaDto
    {
        public string RecipeId { get; set; }
        public string Name { get; set; }

        public string Source { get; set; }
        public RecipeProvider Owner { get; set; }
        public string OwnerLogo { get; set; }
        public string Image { get; set; }

        public List<string> Ingredients { get; set; }

        public static implicit operator RecipeMetaDto(RecipeDto recipe)
        {
            RecipeMetaDto recipeMetaDto = new RecipeMetaDto();
            recipeMetaDto.Name = recipe.Name;
            recipeMetaDto.Image = recipe.Image;
            recipeMetaDto.Owner = recipe.Owner;
            recipeMetaDto.OwnerLogo = recipe.OwnerLogo;
            recipeMetaDto.RecipeId = recipe.RecipeId;
            recipeMetaDto.Source = recipe.Source;
            recipeMetaDto.Ingredients = recipe.Ingredients;

            return recipeMetaDto;
        }
    }
}
