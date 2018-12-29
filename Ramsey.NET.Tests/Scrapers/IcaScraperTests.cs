using NUnit.Framework;
using Ramsey.NET.Crawlers.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ramsey.NET.Crawlers.Implementations.Ica;

namespace Ramsey.NET.Tests.Scrapers
{
    [TestFixture]
    public class IcaScraperTests
    {
        private readonly AIcaRecipeCrawler _cralwer = new IcaRecipeCrawler();

        [SetUp]
        public void Setup()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [Test]
        public async Task LoadLinksAsync()
        {
            var links = await _cralwer.ScrapeLinksAsync(10);
            Assert.IsNotEmpty(links);
        }

        [Test]
        public async Task ScrapeRecipeAsync()
        {
            var recipe = await _cralwer.ScrapeRecipeAsync("https://www.ica.se/recept/najadlax-med-hovmastarkram-och-gurksallad-724469/");
            Assert.IsNotNull(recipe);
        }
    }
}
