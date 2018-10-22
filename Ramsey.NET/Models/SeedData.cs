using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GusteauSharp.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new GusteauContext(
                serviceProvider.GetRequiredService<DbContextOptions<GusteauContext>>()))
            {
                if (context.Recipes.Any())
                    return;

                var tomato = new Ingredient
                {
                    Name = "Tomato"
                };

                var water = new Ingredient
                {
                    Name = "Water"
                };

                var uranium = new Ingredient
                {
                    Name = "Uranium"
                };

                var v2 = new Ingredient
                {
                    Name = "German V2 rocket"
                };

                var recipe = new Recipe();
                recipe.Bitter = 0;
                recipe.Course = "Dessert";
                recipe.Cuisine = "American";
                recipe.Meaty = 0;
                recipe.TotalTimeInSeconds = 5;
                recipe.Sweet = 0.5f;
                recipe.Sour = 0.3f;
                recipe.Salty = 0.4f;
                recipe.Rating = 5;
                recipe.Piquant = 0;
                recipe.NumberOfServings = 5;
                recipe.Name = "Felix Tomato Ketchup Soup";
                recipe.MediumImage = "http://www.scandicfoodschina.com/media/catalog/product/cache/1/image/600x600/9df78eab33525d08d6e5fb8d27136e95/9/7/9758-f.jpg";
                recipe.SmallImage = "http://cumulus.mediakoket.se/procthumbs/06066.jpg";

                context.Recipes.Add(recipe);
                context.Ingredients.AddRange(new Ingredient[] { uranium, tomato, water, v2 });

                context.SaveChanges();

                var recipe_parts = new List<RecipePart>
                {
                    new RecipePart
                    {
                        IngredientID = tomato.IngredientID,
                        Quantity = 100,
                        RecipeID = recipe.RecipeID
                    },
                    new RecipePart
                    {
                        IngredientID = water.IngredientID,
                        Quantity = 100,
                        RecipeID = recipe.RecipeID
                    },
                    new RecipePart
                    {
                        IngredientID = uranium.IngredientID,
                        Quantity = 15,
                        RecipeID = recipe.RecipeID
                    }
                };

                context.RecipeParts.AddRange(recipe_parts);
                context.SaveChanges();
            }
        }
    }
}
