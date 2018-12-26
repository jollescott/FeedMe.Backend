using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NUnit.Framework;
using Ramsey.NET.Crawlers.Implementations.ReceptSe;

namespace Ramsey.NET.Tests.Scrapers
{
    [TestFixture]
    public class ReceptSeScraperTests
    {
        private readonly AReceptSeCrawler _cralwer = new ReceptSeCrawler();

        [SetUp]
        public void Setup()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [Test]
        public async Task GetCount()
        {
            var count = await _cralwer.GetRecipeCountAsync();
            Assert.Greater(count, 0);
            Assert.GreaterOrEqual(179, count);
        }

        [Test]
        public async Task GetPageLinks()
        {
            var links = await _cralwer.GetPageLinksAsync(1);
            
            Assert.IsNotNull(links);
            Assert.IsNotEmpty(links);
            Assert.AreEqual(18, links.Count());
        }

        [Test]
        public void GetRecipeTitle()
        {
            var document = GetTestDocument();

            var title = _cralwer.GetRecipeTitle(document);
            
            Assert.IsNotNull(title);
            Assert.AreEqual("Söta påskchokladkoppar", title);
        }

        [Test]
        public void GetSiteLogo()
        {
            var document = GetTestDocument();

            var logo = _cralwer.GetReceptSeLogo(document);
            
            Assert.IsNotNull(logo);
            Assert.AreEqual("http://recept.se/sites/default/files/recept-se-citrus-2014.gif",logo);
        }

        [Test]
        public void GetRecipeIngredients()
        {
            var document = GetTestDocument();
            var ingredients = _cralwer.GetRecipeParts(document);
            
            Assert.IsNotNull(ingredients);
            Assert.IsNotEmpty(ingredients);
            Assert.AreEqual(6, ingredients.Count());
        }

        [Test]
        public void GetRecipeDirections()
        {
            var document = GetTestDocument();
            var directions = _cralwer.GetRecipeDirections(document);
            
            Assert.IsNotNull(directions);
            Assert.IsNotEmpty(directions);
            Assert.AreEqual(4, directions.Count());
        }

        [Test]
        public void GetRecipeLogo()
        {
            var document = GetTestDocument();
            var logo = _cralwer.GetRecipeLogo(document);
            
            Assert.IsNotNull(logo);
            Assert.AreEqual("http://recept.se/sites/default/files/styles/recipe-large/public/sota-paskchokladkoppar_1_0.jpg?itok=0-eyh6he",logo);
        }

        [Test]
        public async Task ScrapeRecipeAsync()
        {
            var recipeDto = await _cralwer.ScrapeRecipeAsync("http://recept.se/content/sota-paskchokladkoppar", true);
            
            Assert.IsNotNull(recipeDto);
            Assert.IsNotNull(recipeDto.Name);
            Assert.IsNotNull(recipeDto.Image);
            Assert.IsNotNull(recipeDto.Source);
            Assert.IsNotNull(recipeDto.OwnerLogo);
            Assert.IsNotNull(recipeDto.RecipeID);
            
            Assert.IsNotEmpty(recipeDto.Directions);
            Assert.IsNotEmpty(recipeDto.Ingredients);
            Assert.IsNotEmpty(recipeDto.RecipeParts);
        }

        [Test]
        public async Task ScrapeAllRecipes()
        {
            var recipes = await _cralwer.ScrapeRecipesAsync();
            
            Assert.IsNotNull(recipes);
            Assert.IsNotEmpty(recipes);
            Assert.AreEqual(3216, recipes.Count());
        }

        private HtmlDocument GetTestDocument()
        {
            var web = new HtmlWeb();
            var document = web.Load("http://recept.se/content/sota-paskchokladkoppar");

            return document;
        }
    }
}