using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ramsey.Shared.Extensions
{
    public static class IngredientStringExt
    {
        public static string ParseHemmetsIngredient(this string x)
        {
            string ingredient = x;

            Match match1 = Regex.Match(ingredient, @"\(([^\)]*)\)");

            if (match1.Success)
            {
                ingredient = ingredient.Replace(match1.Value, string.Empty);
            }

            Match match2 = Regex.Match(ingredient, @"(\d+)%");

            if(match2.Success)
            {
                ingredient = ingredient.Replace(match2.Value, string.Empty);
            }

            ingredient = ingredient.Split(',').First();

            System.Diagnostics.Debug.WriteLine("[BEFORE] " + x + " [AFTER] " + ingredient);

            return ingredient.Trim();
        }
    }
}
