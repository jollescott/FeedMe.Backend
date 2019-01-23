using NUnit.Framework;
using Ramsey.NET.Auto;
using Ramsey.NET.Auto.Configs;
using Ramsey.NET.Auto.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.NET.Tests.Auto
{
    [TestFixture]
    public class AutoScrapeTest
    {
        public IRamseyAuto RamseyAuto = new RamseyAuto();

        [Test]
        public async Task LoadTestAsync()
        {
            RamseyAuto.Init(new IcaConfig());

            var recipe = await RamseyAuto.ScrapeRecipeAsync("https://www.ica.se/recept/kottbullar-i-graddsas-med-citrongurka-724755/", true);
        }
    }
}
