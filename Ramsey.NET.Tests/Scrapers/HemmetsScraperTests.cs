using NUnit.Framework;
using Ramsey.NET.Crawlers.Implementations;
using Ramsey.NET.Crawlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ramsey.NET.Crawlers.Implementations.Hemmets;
using Ramsey.NET.Implementations;
using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Extensions;
using Ramsey.NET.Interfaces;

namespace Ramsey.NET.Tests.Scrapers
{
    [TestFixture]
    public class HemmetsScraperTests
    {
        readonly AHemmetsRecipeCrawler _crawler = new HemmetsRecipeCrawler();
        private SqlRecipeManager _recipeManager;
        private IRamseyContext _context;

        [SetUp]
        public void SetUp()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var options = new DbContextOptionsBuilder<RamseyContext>()
                .ConnectRamseyTestServer(null, true)
                .Options;

            _context = new RamseyContext(options);
            _context.Database.EnsureCreated();

            _recipeManager = new SqlRecipeManager(_context);
        }

        [Test]
        public async Task GetCountAsync()
        {
            var count = await _crawler.GetRecipeCountAsync();
            Assert.NotZero(count);
        }

        [Test]
        public async Task ScrapeRecipePageAsync()
        {
            var url = "https://kokboken.ikv.uu.se/receptsida.php?recid=385";
            var result = await _crawler.ScrapeRecipeAsync(url);

            Assert.NotNull(result);
        }

        [Test]
        public async Task ScrapePageAsync()
        {
            var result = await _crawler.ScrapePageAsync(0);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public async Task ScrapeTest()
        {
            await _crawler.ScrapeRecipesAsync(_recipeManager, amount: 10);

            await _context.Recipes.Include(x => x.RecipeParts).ForEachAsync(x =>
            {
                Assert.NotNull(x.RecipeId);
                Assert.NotNull(x.Name);
                Assert.NotNull(x.Image);
                Assert.NotNull(x.OwnerLogo);
                Assert.NotNull(x.Source);

                foreach(var part in x.RecipeParts)
                {
                    Assert.NotNull(part.IngredientId);
                    Assert.NotNull(part.RecipeId);

                    Assert.IsNotEmpty(part.RecipeId);
                    Assert.IsNotEmpty(part.IngredientId);

                    Assert.NotNull(part.Recipe);
                    Assert.NotNull(part.Ingredient);

                    Assert.NotNull(part.Unit);
                    Assert.IsNotEmpty(part.Unit);
                }
            });
        }
    }
}
