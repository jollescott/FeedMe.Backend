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
            RamseyAuto.Init(new HemmetsConfig());

            var recipe = await RamseyAuto.ScrapeRecipeAsync("https://kokboken.ikv.uu.se/receptsida.php?recid=391", true);
        }
    }
}
