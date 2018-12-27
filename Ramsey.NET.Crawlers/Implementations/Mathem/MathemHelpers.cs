using System.Collections.Generic;
using System.Linq;
using Ramsey.Shared.Dto;

namespace Ramsey.NET.Crawlers.Implementations.Mathem
{
    public class MathemRecipeDetails
    {
        public string Heading { get; set; }
        public string Id { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        
        public IEnumerable<string> Instructions { get; set; }
        public IEnumerable<MathemIngredientGroup> Ingredients { get; set; }
    }

    public class MathemIngredientGroup
    {
        public string GroupName { get; set; }
        public IEnumerable<MathemIngredient> Ingredients { get; set; }
    }
    
    public class MathemIngredient
    {
        public float Amount { get; set; }
        public string Unit { get; set; }
        public string Name { get; set; }
    }

    public static class MathemExtensions
    {
        public static RecipeDtoV2 ToRecipeDtoV2(this MathemRecipeDetails recipeDetails, bool includeAll = true)
        {
            var recipe = new RecipeDtoV2
            {
                Name = recipeDetails.Heading,
                Image = recipeDetails.ImageUrl,
                RecipeID = "MH" + recipeDetails.Id,
                Owner = RecipeProvider.Mathem,
                OwnerLogo = "https://static.mathem.se/images/logos/logo.svg",
                Source = "https://www.mathem.se/recept/" + recipeDetails.Url,
                RecipeParts = recipeDetails.Ingredients.SelectMany(x => x.Ingredients).Select(x =>
                    new RecipePartDtoV2
                    {
                        Quantity = x.Amount, RecipeID = recipeDetails.Id, IngredientID = x.Name, Unit = x.Unit
                    }),
            };

            recipe.Ingredients = recipe.RecipeParts.Select(x => x.IngredientID).Distinct();

            if (includeAll)
                recipe.Directions = recipeDetails.Instructions;
            
            return recipe;
        }
    }
}