using NUnit.Framework;
using Ramsey.NET.Crawlers.Implementations;
using Ramsey.NET.Crawlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.NET.Tests
{
    [TestFixture]
    public class HemmetsScraperTests
    {
        IHemmetsRecipeCrawler crawler = new HemmetsRecipeCrawler();

        [SetUp]
        public void Setup()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [Test]
        public async Task GetCountAsync()
        {
            var count = await crawler.GetRecipeCountAsync();
            Assert.NotZero(count);
        }

        [Test]
        public async Task ScrapeRecipePageAsync()
        {
            var url = "https://kokboken.ikv.uu.se/receptsida.php?recid=385";
            var result = await crawler.ScrapeRecipeAsync(url);

            Assert.NotNull(result);
        }

        [Test]
        public async Task ScapePageAsync()
        {
            var result = await crawler.ScrapePageAsync(0);
            Assert.IsNotEmpty(result);
        }
    }
}
