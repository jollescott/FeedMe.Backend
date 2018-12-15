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

namespace Ramsey.NET.Implementations
{
    public class CrawlerService : ICrawlerService
    {
        private RamseyContext _context;
        private IRecipeManager _recipeManager;
        private AHemmetsRecipeCrawler _hCrawler = new HemmetsRecipeCrawler();
        private AIcaRecipeCrawler _iCrawler = new IcaRecipeCrawler();

        public CrawlerService(RamseyContext context, IRecipeManager recipeManager)
        {
            _context = context;
            _recipeManager = recipeManager;
        }

        public async Task UpdateIndexAsync()
        {
            var hRecipes = await _hCrawler.ScrapeRecipesAsync();
            await _recipeManager.UpdateRecipeDatabaseAsync(_context,hRecipes);
            hRecipes.Clear();

            var iRecipes = await _iCrawler.ScrapeRecipesAsync();
            await _recipeManager.UpdateRecipeDatabaseAsync(_context, iRecipes);
            System.Diagnostics.Debug.WriteLine("All done!");
        }

        public Task<RecipeDto> ScrapeRecipeAsync(string url, RecipeProvider provider)
        {
            return _hCrawler.ScrapeRecipeAsync(url, true);
        }
    }
}
