using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Crawlers.Implementations;
using Ramsey.NET.Models;
using Ramsey.Shared.Dto;
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
        private AHemmetsRecipeCrawler _hCrawler = new HemmetsRecipeCrawler();
        private AIcaRecipeCrawler _iCrawler = new IcaRecipeCrawler();

        public CrawlerService(RamseyContext context)
        {
            _context = context;
        }

        public async Task UpdateIndexAsync()
        {
            var recipes = await _hCrawler.ScrapeRecipesAsync();
            await AddRecipesAsync(recipes);
            recipes.Clear();

            //recipes = await _iCrawler.ScrapeRecipesAsync();
            //await AddRecipesAsync(recipes);
            System.Diagnostics.Debug.WriteLine("All done!");
        }

        public async Task AddRecipesAsync(List<RecipeMetaDto> recipeDtos)
        {
            foreach (var r in recipeDtos)
            {
                List<Ingredient> ings = new List<Ingredient>();

                foreach (var i in r.Ingredients)
                {
                    Ingredient ingredient = _context.Ingredients.Find(i.ToLower());

                    if (ingredient == null)
                    {
                        ingredient = new Ingredient
                        {
                            IngredientID = i.ToLower()
                        };

                        _context.Ingredients.Add(ingredient);
                    }

                    ings.Add(ingredient);
                }

                RecipeMeta recipe = _context.Recipes.Find(r.RecipeID);

                if (recipe == null)
                {
                    recipe = new RecipeMeta
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
                }
            }

            await _context.SaveChangesAsync();
        }

        public Task<RecipeDto> ScrapeRecipeAsync(string url, RecipeProvider provider)
        {
            return _hCrawler.ScrapeRecipeAsync(url, true);
        }
    }
}
