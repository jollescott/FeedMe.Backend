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

        public static string ParseIcaIngredient(this string x)
        {
            string ingredient = x;

            ingredient = ingredient.ParseHemmetsIngredient();

            Match ellerMatch = Regex.Match(ingredient, @"\w+(?=\s+eller)");

            if(ellerMatch.Success)
            {
                ingredient = ellerMatch.Value;
            }

            Match tillMatch = Regex.Match(ingredient, @"\w+(?=\s+till)");

            if (tillMatch.Success)
            {
                ingredient = tillMatch.Value;
            }

            Match unitMatch = Regex.Match(ingredient, @"[\d-]");

            if(unitMatch.Success)
            {
                ingredient = ingredient.Replace(unitMatch.Value, string.Empty).Trim();
            }

            var parts = ingredient.Split(' ');

            if(parts.Count() > 1)
            {
                ingredient = parts.Last();
            }

            System.Diagnostics.Debug.WriteLine("[BEFORE] " + x + " [AFTER] " + ingredient);

            return ingredient.Trim();
        }
    }
}
