using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Ramsey.NET.Implementations;
using Ramsey.NET.Models;
using Ramsey.Shared.Dto;

namespace Ramsey.NET.Tests.Controllers
{
    public class BaseControllerTests
    {
        protected RamseyContext _context;
        
        [SetUp]
        public virtual void SetUp()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var options = new DbContextOptionsBuilder<RamseyContext>()
                .UseInMemoryDatabase("RamseyInMemory")
                .Options;

            _context = new RamseyContext(options);
            _context.Database.EnsureCreated();

            _context.Recipes.AddRange(new RecipeMeta
            {
                Name = "Ketchup",
                Image = "www.google.com",
                RecipeId = "1",
                Owner = RecipeProvider.Hemmets,
                OwnerLogo = "www.google.com",
                Source = "https://kokboken.ikv.uu.se/receptsida.php?recid=837",
            }, new RecipeMeta
            {
                Name = "Ketchup 2",
                Image = "www.google.com",
                RecipeId = "2",
                Owner = RecipeProvider.ICA,
                OwnerLogo = "www.google.com",
                Source = "https://kokboken.ikv.uu.se/receptsida.php?recid=837"
            });
            
            _context.RecipeParts.AddRange(new RecipePart
            {
                IngredientId = "tomat",
                Quantity = 2,
                RecipeId = "1",
                Unit = "Styck"
            }, new RecipePart
            {
                IngredientId = "salt",
                Quantity = 10,
                RecipeId = "1",
                Unit = "kg"
            }, new RecipePart{
                IngredientId = "tomat",
                Quantity = 3,
                RecipeId = "2",
                Unit = "Styck"
            });
            
            _context.Ingredients.AddRange(new Ingredient
                {
                    IngredientId = "salt"
                },
                new Ingredient
                {
                    IngredientId = "tomat"
                });

            _context.SaveChanges();
        }

        [TearDown]
        public virtual void TearDown()
        {
            _context?.Database.EnsureDeleted();
        }
    }
}