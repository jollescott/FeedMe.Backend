using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Ramsey.NET.Controllers;
using Ramsey.NET.Controllers.V2;
using Ramsey.NET.Implementations;
using Ramsey.NET.Models;
using Ramsey.Shared.Dto;

namespace Ramsey.NET.Tests.Controllers
{
    [TestFixture]
    public class IngredientControllerTests : BaseControllerTests
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
        public void SuggestIngredients()
        {
            var search = "tomat";
            var controller = new IngredientControllerV2(_context);

            var result = controller.Suggest(search);

            var jsonResult = result as JsonResult;
            var dtos = jsonResult.Value as List<IngredientDtoV2>;
            
            Assert.IsNotNull(dtos);
            Assert.IsNotEmpty(dtos);

            foreach (var dto in dtos)
            {
                Assert.IsTrue(dto.IngredientId.Contains(search));
            }
        }
    }
}