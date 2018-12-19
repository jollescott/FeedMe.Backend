using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Ramsey.Shared.Extensions;

namespace Ramsey.NET.Tests
{
    [TestFixture]
    public class IngredientParserTests
    {
        [Test]
        public void ParsingTest()
        {
            string test1 = "Avocado(halv)";
            var result1 = test1.ParseHemmetsIngredient();

            Assert.AreEqual("Avocado", result1);

            string test2 = "Avocado, Skivad";
            var result2 = test2.ParseHemmetsIngredient();

            Assert.AreEqual("Avocado", result2);

            string test3 = "Avocado 12%";
            var result3 = test3.ParseHemmetsIngredient();

            Assert.AreEqual("Avocado", result3);
        }
    }
}
