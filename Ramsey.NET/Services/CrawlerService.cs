using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Crawlers.Implementations;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Models;
using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Ramsey.NET.Crawlers.Implementations.Hemmets;
using Ramsey.NET.Crawlers.Implementations.Ica;

namespace Ramsey.NET.Services
{
    public class CrawlerService : ICrawlerService
    {
        private readonly IRamseyContext _context;
        private readonly AHemmetsRecipeCrawler _hCrawler = new HemmetsRecipeCrawler();
        private readonly AIcaRecipeCrawler _iCrawler = new IcaRecipeCrawler();

        public CrawlerService(IRamseyContext context)
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

        public async Task AddRecipesAsync(IList<RecipeMetaDtoV2> recipeDtos)
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
                        RecipeId = r.RecipeID,
                        OwnerLogo = r.OwnerLogo
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

        public Task<RecipeDtoV2> ScrapeRecipeAsync(string url, RecipeProvider provider)
        {
            return _hCrawler.ScrapeRecipeAsync(url, true);
        }
    }
}
