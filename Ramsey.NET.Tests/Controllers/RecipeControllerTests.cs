using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Ramsey.NET.Controllers.V2;
using Ramsey.NET.Implementations;
using Ramsey.Shared.Dto;

namespace Ramsey.NET.Tests.Controllers
{
    [TestFixture]
    public class RecipeControllerTests : BaseControllerTests
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }
        
        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }
        
        [Test]
        public void SuggestRecipes()
        {
            var controller = new RecipeControllerV2(_context, null);

            var parts = _context.RecipeParts.Where(x => x.IngredientId == "salt").Select(y => new RecipePartDtoV2
            {
                RecipeID = y.RecipeId,
                IngredientID = y.IngredientId,
                Quantity = y.Quantity,
                Unit = y.Unit
            }).ToList();
            
            var recipes = controller.Suggest(new List<IngredientDtoV2>
            {
                new IngredientDtoV2
                {
                    IngredientId = "salt",
                    RecipeParts = parts
                }
            });

            var jsonResult = (JsonResult) recipes;
            var dtos = jsonResult.Value as IEnumerable<RecipeMetaDtoV2>;

            Assert.IsNotNull(dtos);
            Assert.IsNotEmpty(dtos);

            foreach (var dto in dtos)
            {
                Assert.LessOrEqual(dto.Coverage, 1);
            }
        }

        [Test]
        public async Task RetrieveAsync()
        {
            var controller = new RecipeControllerV2(_context, new CrawlerService(_context, new SqlRecipeManager(_context)));
            var result = await controller.RetrieveAsync("1");
            
            var jsonResult = (JsonResult) result;
            var recipe = jsonResult.Value as RecipeDtoV2;
            
            Assert.IsNotNull(recipe);
            Assert.IsNotEmpty(recipe.Ingredients);
            Assert.IsNotEmpty(recipe.RecipeParts);
            Assert.IsNotNull(recipe.Directions);
        }

        [Test]
        public void TestExclusions()
        {
            var controller = new RecipeControllerV2(_context, null);

            var sParts = _context.RecipeParts.Where(x => x.IngredientId == "salt").Select(y => new RecipePartDtoV2
            {
                RecipeID = y.RecipeId,
                IngredientID = y.IngredientId,
                Quantity = y.Quantity,
                Unit = y.Unit
            }).ToList();
            
            var tParts = _context.RecipeParts.Where(x => x.IngredientId == "tomat").Select(y => new RecipePartDtoV2
            {
                RecipeID = y.RecipeId,
                IngredientID = y.IngredientId,
                Quantity = y.Quantity,
                Unit = y.Unit
            }).ToList();
            
            var recipes = controller.Suggest(new List<IngredientDtoV2>
            {
                new IngredientDtoV2
                {
                    IngredientId = "salt",
                    RecipeParts = sParts,
                    Role = IngredientRole.Exclude
                },
                new IngredientDtoV2
                {
                    IngredientId = "tomat",
                    RecipeParts = tParts,
                    Role = IngredientRole.Include
                }
            });

            var jsonResult = (JsonResult) recipes;
            var dtos = jsonResult.Value as IEnumerable<RecipeMetaDtoV2>;

            Assert.IsNotNull(dtos);
            Assert.IsNotEmpty(dtos);

            foreach (var dto in dtos)
            {
                Assert.LessOrEqual(dto.Coverage, 1);
                Assert.IsNotEmpty(dto.Ingredients);

                foreach (var ingredient in dto.Ingredients)
                {
                    Assert.AreNotEqual(ingredient, "salt");
                }
            }
        }
    }
}