using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Ramsey.Core;
using Ramsey.NET.Auto;
using Ramsey.NET.Auto.Configs;
using Ramsey.NET.Auto.Implementations;
using Ramsey.NET.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ramsey.NET.Tests
{
    [TestFixture]
    public class AutoTests
    {
        private RamseyContext _ramseyContext;
        private IWordRemover _wordRemover;

        [OneTimeSetUp]
        public void LoadContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<RamseyContext>()
                .UseSqlite("Data Source=ramsey-test.db")
                .Options;

            _ramseyContext = new RamseyContext(dbContextOptions);

            _ramseyContext.Database.EnsureDeleted();
            _ramseyContext.Database.EnsureCreated();

            _wordRemover = new BasicWordRemover(_ramseyContext);
        }

        [OneTimeTearDown]
        public void UnloadContext()
        {
            _ramseyContext.Database.EnsureDeleted();
            _ramseyContext.Dispose();
        }

        [Test]
        public async System.Threading.Tasks.Task LoadIcaRecipeAsync()
        {
            var scraper = new RamseyAuto(new HemmetsConfig(), _ramseyContext, _wordRemover);
            var recipe = await scraper.ScrapeRecipeAsync("https://kokboken.ikv.uu.se/receptsida.php?recid=391&", true);

            Assert.IsTrue(Uri.IsWellFormedUriString(recipe.OwnerLogo, UriKind.Absolute));
            Assert.IsTrue(Uri.IsWellFormedUriString(recipe.Image, UriKind.Absolute));

            Assert.IsNotNull(recipe.Name);
            Assert.IsNotNull(recipe.Owner);

            Assert.IsNotNull(recipe.Directions);
            Assert.IsNotNull(recipe.Ingredients);
            Assert.IsNotNull(recipe.RecipeParts);

            Assert.IsNotEmpty(recipe.Directions);
            Assert.IsNotEmpty(recipe.Ingredients);
            Assert.IsNotEmpty(recipe.RecipeParts);
        }
    }
}
