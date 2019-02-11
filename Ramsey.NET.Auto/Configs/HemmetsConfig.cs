using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Ramsey.NET.Auto.Extensions;
using Ramsey.Shared.Enums;

namespace Ramsey.NET.Auto.Configs
{
    public class HemmetsConfig : IAutoConfig
    {
        public RamseyLocale ProviderLocale => RamseyLocale.Swedish;

        public string ProviderId => "HM";

        public string ProviderLogo => "https://upload.wikimedia.org/wikipedia/commons/thumb/d/dc/UU_logo.jpg/628px-UU_logo.jpg";

        public RecipeProvider ProviderName => RecipeProvider.Hemmets;

        public Func<string, string> ParseId => (url) => {
            var index = url.IndexOf("recid=") + 5;
            var end = url.IndexOf('&', index) - 1;

            var id = "HM" + url.Substring(index + 1, end - index);
            return id;
        };

        Func<HtmlDocument, string> IAutoConfig.LoadImage => (doc) => {
            return "https://kokboken.ikv.uu.se/" + doc.DocumentNode
                    .SelectSingleNode(ImageXPath)
                    .Attributes["src"]
                    .Value;
        };

        public Func<int, HtmlDocument, HttpClient, Task<HttpResponseMessage>> NextPage => (index, doc, client) => {
            var postData = $"offset={(index+1) * PageItemCount}";
            return client.PostAsync("https://kokboken.ikv.uu.se/receptlista.php?cat=0", new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded"));
        };

        public Func<string, string> ProcessIngredient => (ing) => Regex.Replace(ing, "([0-9]\\d*(\\,\\d+)? g)", string.Empty);

        public string RootPage => "https://kokboken.ikv.uu.se/";

        public string RecipeItemXPath => "/html/body/div[2]/div[2]/div/form/table[1]//tr/td/strong/a";

        public string ImageXPath => "/html/body/div[2]/div[2]/form/div/div[1]/img";

        public string DirectionsXPath => "//div[@class=\"receptrightcol\"]/table/tr";

        public string IngredientsXPath => "//div[@class=\"receptleftcol\"]/table/tr[position()>1]";

        public int RecipeCount => 764;

        public int PageItemCount => 20;

        public string NameXPath => "/html/body/div[2]/div[2]/form/div/div[2]/h1";

        public string[] TagXPaths => new string[] {
            "//div[@class='info']/table/tbody/tr[2]/td[2]"
        };

        public Func<HtmlDocument, string[]> ProcessTag => (document) => {
            var tag = document.DocumentNode.SelectSingleNode(TagXPaths[0]).InnerText.RemoveSpecialCharacters();
            var tags = tag.Split(',');

            return tags;
        };
    }
}
