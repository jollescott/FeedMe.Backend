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
                    Ingredient ingredient;

                    if (_context.Ingredients.Any(x => x.Name.Equals(i)))
                    {
                        ingredient = _context.Ingredients.Where(x => x.Name.Equals(i)).FirstOrDefault();
                    }
                    else
                    {
                        ingredient = new Ingredient
                        {
                            Name = i
                        };

                        _context.Ingredients.Add(ingredient);
                        await _context.SaveChangesAsync();
                    }

                    ings.Add(ingredient);
                }

                Recipe recipe;

                if(_context.Recipes.Any(x => x.Name.Equals(r.Name)))
                {
                    recipe = _context.Recipes.Where(x => x.Name.Equals(r.Name)).FirstOrDefault();
                }
                else
                {
                    recipe = new Recipe
                    {
                        Image = r.Image,
                        Name = r.Name,
                        NativeID = r.NativeID,
                        Owner = r.Owner,
                        Source = r.Source
                    };

                    _context.Recipes.Add(recipe);
                    await _context.SaveChangesAsync();
                }

                var recipeParts = ings.Select(x => new RecipePart
                {
                    IngredientID = x.IngredientID,
                    RecipeID = recipe.RecipeID
                });

                _context.RecipeParts.AddRange(recipeParts);
                await _context.SaveChangesAsync();
            }
        }
    }
}
