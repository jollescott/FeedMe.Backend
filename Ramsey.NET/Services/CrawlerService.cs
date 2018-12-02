using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Crawlers.Implementations;
using Ramsey.NET.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Services
{
    public class CrawlerService : ICrawlerService
    {
        private RamseyContext _context;
        private AHemmetsRecipeCrawler _crawler = new HemmetsRecipeCrawler();

        public CrawlerService(RamseyContext context)
        {
            _context = context;
        }

        public async Task UpdateIndexAsync()
        {
            var recipes = await _crawler.ScrapeRecipesAsync();

            foreach (var r in recipes)
            {
                List<Ingredient> ings = new List<Ingredient>();

                foreach(var i in r.Ingredients)
                {
                    Ingredient ingredient = _context.Ingredients.Find(i.ToLower());

                    if(ingredient == null)
                    {
                        ingredient = new Ingredient
                        {
                            IngredientID = i.ToLower()
                        };

                        _context.Ingredients.Add(ingredient);
                    }

                    ings.Add(ingredient);
                }

                await _context.SaveChangesAsync();

                Recipe recipe = _context.Recipes.Find(r.RecipeID);

                if(recipe == null)
                {
                    recipe = new Recipe
                    {
                        Image = r.Image,
                        Name = r.Name,
                        Owner = r.Owner,
                        Source = r.Source,
                        RecipeId = r.RecipeID
                    };

                    _context.Recipes.Add(recipe);
                    await _context.SaveChangesAsync();

                    var recipeParts = ings.Select(x => new RecipePart
                    {
                        IngredientId = x.IngredientID,
                        RecipeId = recipe.RecipeId
                    });

                    _context.RecipeParts.AddRange(recipeParts);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
