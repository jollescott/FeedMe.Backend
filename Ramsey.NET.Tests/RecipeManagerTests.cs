using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Ramsey.NET.Crawlers.Implementations;
using Ramsey.NET.Crawlers.Interfaces;
using Ramsey.NET.Implementations;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.NET.Tests
{
    [TestFixture]
    public class RecipeManagerTests 
    {
        IRecipeCrawler hCrawler = new HemmetsRecipeCrawler();
        IRecipeManager _recipeManager = new SqlRecipeManager();
        private RamseyContext _context;

        [SetUp]
        public void SetUp()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<RamseyContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ramsey_unit;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            _context = new RamseyContext(options);
            _context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Database?.EnsureDeleted();
        }

        [Test]
        public async Task TestFullRefresh()
        {
            var recipes = await hCrawler.ScrapeRecipesAsync(10);
            await _recipeManager.UpdateRecipeDatabaseAsync(_context, recipes);
        }
    }
}
