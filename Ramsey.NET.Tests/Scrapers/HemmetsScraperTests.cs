using NUnit.Framework;
using Ramsey.NET.Crawlers.Implementations;
using Ramsey.NET.Crawlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ramsey.NET.Crawlers.Implementations.Hemmets;

namespace Ramsey.NET.Tests.Scrapers
{
    [TestFixture]
    public class HemmetsScraperTests
    {
        readonly AHemmetsRecipeCrawler _crawler = new HemmetsRecipeCrawler();

        [SetUp]
        public void Setup()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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
        public async Task ScrapeAllPagesAsync()
        {
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var result = await _crawler.ScrapeRecipesAsync();
            Console.WriteLine("All recipes scraped");

            Assert.AreEqual(764, result.Count);
        }
    }
}
