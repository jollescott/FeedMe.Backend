using System;
using System.Collections.Generic;
using System.Text;

namespace Ramsey.Shared.Misc
{
    public class RamseyApi
    {
        public static string Base => "https://api.feedmeapp.se/";

        public class V1
        {
            public class Ingredient
            {
                public static string Suggest => Base + "ingredient/suggest";
            }

            public class Recipe
            {
                public static string Suggest => Base + "recipe/suggest";
                public static string Retreive => Base + "recipe/retrieve";
            }
        }

        public class V2
        {
            public class Ingredient
            {
                public static string Suggest => Base + "v2/ingredient/suggest";
            }

            public class Recipe
            {
                public static string Suggest => Base + "v2/recipe/suggest";
                public static string Retreive => Base + "v2/recipe/retrieve";
                public static string Text => Base + "v2/recipe/text";
            }

            public class Tags
            {
                public static string Suggest => Base + "v2/tags/suggest";
                public static string List => Base + "v2/tags/list";
            }
        }
    }
}
