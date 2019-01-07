using System;
using System.Collections.Generic;
using System.Text;

namespace Ramsey.Shared.Dto.V2
{
    public class RecipeMetaDtoV2
    {
        public string RecipeID { get; set; }
        public string Name { get; set; }

        public string Source { get; set; }
        public RecipeProvider Owner { get; set; }
        public string OwnerLogo { get; set; }
        public string Image { get; set; }

        public IEnumerable<string> Ingredients { get; set; }
        public IEnumerable<RecipePartDtoV2> RecipeParts { get; set; }
        
        public double Coverage { get; set; }

        public static implicit operator RecipeMetaDtoV2(RecipeDtoV2 recipe)
        {
            RecipeMetaDtoV2 recipeMetaDto = new RecipeMetaDtoV2();
            recipeMetaDto.Name = recipe.Name;
            recipeMetaDto.Image = recipe.Image;
            recipeMetaDto.Owner = recipe.Owner;
            recipeMetaDto.OwnerLogo = recipe.OwnerLogo;
            recipeMetaDto.RecipeID = recipe.RecipeID;
            recipeMetaDto.Source = recipe.Source;
            recipeMetaDto.Ingredients = recipe.Ingredients;
            recipeMetaDto.RecipeParts = recipe.RecipeParts;

            return recipeMetaDto;
        }
    }
}
