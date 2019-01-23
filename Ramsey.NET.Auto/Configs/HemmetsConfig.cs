using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Ramsey.Shared.Enums;

namespace Ramsey.NET.Auto.Configs
{
    public class HemmetsConfig : IAutoConfig
    {
        public RamseyLocale ProviderLocale => RamseyLocale.Swedish;

        public string ProviderId => "HM";

        public string ProviderLogo => "https://upload.wikimedia.org/wikipedia/commons/thumb/d/dc/UU_logo.jpg/628px-UU_logo.jpg";

        public string ProviderName => "Hemmets Kokbok";

        public Func<string, string> ParseId => (url) => {
            return "HM" + url.Split('=').Last();
        };

        Func<HtmlDocument, string> IAutoConfig.LoadImage => (doc) => {
            return "https://kokboken.ikv.uu.se/" + doc.DocumentNode
                    .SelectSingleNode(ImageXPath)
                    .Attributes["src"]
                    .Value;
        };

        Func<int, HtmlDocument, string> IAutoConfig.NextPage => null;

        public string RootPage => "https://kokboken.ikv.uu.se/receptlista.php?cat=0";

        public string RecipeItemXPath => "/html/body/div[2]/div[2]/div/form/table[1]/tbody/tr/td/strong/a";

        public string ImageXPath => "/html/body/div[2]/div[2]/form/div/div[1]/img";

        public string DirectionsXPath => "//div[@class=\"receptrightcol\"]/table/tr";

        public string IngredientsXPath => "/html/body/div[2]/div[2]/form/div/div[4]/table/tbody/tr/td";

        public int RecipeCount => 40;

        public int PageItemCount => 20;

        public string NameXPath => "/html/body/div[2]/div[2]/form/div/div[2]/h1";
    }
}
