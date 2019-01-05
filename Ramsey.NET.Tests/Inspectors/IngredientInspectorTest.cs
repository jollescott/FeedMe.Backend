using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ramsey.NET.Ingredients.Implementations;
using Ramsey.NET.Ingredients.Interfaces;
using Ramsey.Shared.Dto.V2;

namespace Ramsey.NET.Tests.Inspectors
{
    [TestFixture]
    public class IngredientInspectorTest
    {
        readonly IIngredientInspector<RecipePartDtoV2> _inspector = new BasicIngredientInspector();
        
        [Test]
        public void ParseIngredients()
        {
            IList<RecipePartDtoV2> ingredients = new List<RecipePartDtoV2>
            {
                new RecipePartDtoV2
                {
                    IngredientID = "gurka",
                    Quantity = 2,
                    RecipeID = "test",
                    Unit = ""
                },
                new RecipePartDtoV2
                {
                    IngredientID  = "gurka - hackad",
                    Quantity = 2,
                    RecipeID = "test",
                    Unit = ""
                },
                new RecipePartDtoV2
                {
                    IngredientID = "gurka(salt)",
                    Quantity = 2,
                    RecipeID = "test",
                    Unit = ""
                }
            };

            _inspector.InspectIngredient(ingredients);
            
            Assert.AreEqual(1, ingredients.Select(x => x.IngredientID).Distinct().Count());
        }
    }
}