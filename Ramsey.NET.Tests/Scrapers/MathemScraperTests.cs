using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Ramsey.NET.Crawlers.Implementations.Mathem;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Extensions;

namespace Ramsey.NET.Tests.Scrapers
{
    [TestFixture]
    public class MathemScraperTests
    {
        private readonly AMathemCrawler _cralwer = new MathemCrawler();

        [SetUp]
        public void Setup()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        
        [Test]
        public async Task ConvertFromApi()
        {
            var httpClient = new HttpClient();
            
            var response = await httpClient.GetAsync("https://api.mathem.io/ecom-recipe/noauth/recipes/detail?url=mathems-klassiska-julbord-for-8-personer&portions=0");
            var json = await response.Content.ReadAsSwedishStringAsync();

            var mathemDetails = _cralwer.GetDetailsFromJson(json);
            
            Assert.IsNotNull(mathemDetails);
            Assert.IsNotNull(mathemDetails.Id);
            Assert.IsNotNull(mathemDetails.Url);
            Assert.IsNotNull(mathemDetails.Heading);
            Assert.IsNotNull(mathemDetails.ImageUrl);
            
            Assert.IsNotEmpty(mathemDetails.Ingredients);
            Assert.IsNotEmpty(mathemDetails.Instructions);
        }

        [Test]
        public async Task ScrapeRecipe()
        {
            var recipe =
                await _cralwer.ScrapeRecipeAsync("https://www.mathem.se/recept/mathems-klassiska-julbord-for-8-personer", true);
            
            Assert.IsNotNull(recipe);
            Assert.IsNotNull(recipe.Name);
            Assert.IsNotNull(recipe.Image);
            Assert.IsNotNull(recipe.Source);
            Assert.IsNotEmpty(recipe.Directions);
            Assert.IsNotEmpty(recipe.Ingredients);
            Assert.IsNotEmpty(recipe.RecipeParts);
            Assert.IsNotNull(recipe.RecipeID);
        }

        [Test]
        public async Task QueryTest()
        {
            var recipes = new List<RecipeMetaDtoV2>();
            var sqlManagerMock = new Mock<IRecipeManager>();

            sqlManagerMock.Setup(x => x.UpdateRecipeMetaAsync(It.IsAny<RecipeMetaDtoV2>()))
                .Returns(Task.FromResult(true)).Callback<RecipeMetaDtoV2>(recipes.Add);

            await _cralwer.ScrapeRecipesAsync(sqlManagerMock.Object, 10);
            
            Assert.IsNotEmpty(recipes);
        }
    }
}