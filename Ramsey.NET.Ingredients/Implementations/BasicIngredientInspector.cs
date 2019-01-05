using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ramsey.NET.Ingredients.Interfaces;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto.V2;

namespace Ramsey.NET.Ingredients.Implementations
{
    public class BasicIngredientInspector : IIngredientInspector<RecipePartDtoV2>
    {
        public void InspectIngredient(IList<RecipePartDtoV2> ingredients)
        {
            CheckMultiple(ingredients);
            CheckParentheses(ingredients);
            CheckBinding(ingredients);
        }

        private void CheckBinding(IList<RecipePartDtoV2> ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                var pos = ingredient.IngredientID.IndexOf(" - ");
                if (pos > -1)
                    ingredient.IngredientID =
                        ingredient.IngredientID.Remove(pos, ingredient.IngredientID.Length - pos);
            }
        }

        private void CheckParentheses(IList<RecipePartDtoV2> ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                Regex r = new Regex("\\(.*\\)");
                ingredient.IngredientID = r.Replace(ingredient.IngredientID, "");
            }
        }

        /// <summary>
        /// Check if ingredient contains "Och"
        /// </summary>
        /// <param name="ingredient"></param>
        private void CheckMultiple(IList<RecipePartDtoV2> ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                if (ingredient.RecipeID.Contains("och") || ingredient.RecipeID.Contains("eller"))
                {
                    var ids = ingredient.RecipeID
                        .Replace("och", string.Empty)
                        .Replace("eller", string.Empty)
                        .Split(' ');

                    ingredient.RecipeID = ids.First();
                    ingredients.Add(new RecipePartDtoV2
                    {
                        Quantity = ingredient.Quantity,
                        Unit = ingredient.Unit,
                        IngredientID = ids.Last(),
                        RecipeID = ingredient.RecipeID
                    });
                }
            }
        }
      
    }
}