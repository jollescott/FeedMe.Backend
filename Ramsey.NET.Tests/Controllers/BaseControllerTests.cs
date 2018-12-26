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

            _context.Recipes.Add(new RecipeMeta
            {
                Name = "Ketchup",
                Image = "www.google.com",
                RecipeId = "1",
                Owner = RecipeProvider.Hemmets,
                OwnerLogo = "www.google.com",
                Source = "https://kokboken.ikv.uu.se/receptsida.php?recid=837",
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
            });
            
            _context.Ingredients.AddRange(new Ingredient
                {
                    IngredientID = "salt"
                },
                new Ingredient
                {
                    IngredientID = "tomat"
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