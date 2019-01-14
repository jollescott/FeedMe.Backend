using NUnit.Framework;
using Ramsey.NET.Ingredients.Implementations;
using Ramsey.NET.Ingredients.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ramsey.NET.Tests.Misc
{
    [TestFixture]
    public class IngredientResolverTests
    {
        private IIngredientResolver _ingredientResolver;

        [OneTimeSetUp]
        public void SetupResolver()
        {
            _ingredientResolver = new BasicIngredientResolver();
        }

        [Test]
        public async System.Threading.Tasks.Task ResolveTestAsync()
        {
            var input = "tomater";
            var output = await _ingredientResolver.ResolveIngredientAsync(input);

            Assert.AreEqual("tomat", output);

            string test1 = "Avocado(halv)";
            var result1 = await _ingredientResolver.ResolveIngredientAsync(test1);

            Assert.AreEqual("Avocado", result1);

            string test2 = "Avocado, Skivad";
            var result2 = await _ingredientResolver.ResolveIngredientAsync(test2);

            Assert.AreEqual("Avocado", result2);

            string test3 = "Avocado - 12%";
            var result3 = await _ingredientResolver.ResolveIngredientAsync(test3);

            Assert.AreEqual("Avocado", result3);
        }
    }
}
