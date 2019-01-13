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
            }

            public class Favorite
            {
                public static string Add => Base + "v2/favorite/add";
                public static string Delete => Base + "v2/favorite/delete";
                public static string List => Base + "v2/favorite/list";
            }

            public class Rating
            {
                public static string Update => Base + "v2/rating/update";
            }

            public class User
            {
                public static string Sync => Base + "v2/user/sync";
            }
        }
    }
}
