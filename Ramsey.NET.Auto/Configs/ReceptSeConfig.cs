using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Ramsey.Shared.Enums;

namespace Ramsey.NET.Auto.Configs
{
    public class ReceptSeConfig : IAutoConfig
    {
        public RamseyLocale ProviderLocale => RamseyLocale.Swedish;

        public string ProviderId => "RSE";

        public string ProviderLogo => "http://recept.se/sites/default/files/recept-se-citrus-2014.gif";

        public RecipeProvider ProviderName => RecipeProvider.ReceptSe;

        public int RecipeCount => 3220;

        public int PageItemCount => 18;

        public Func<HtmlDocument, string> LoadImage => null;

        public Func<string, string> ParseId => null;

        public Func<int, HtmlDocument, HttpClient, Task<HttpResponseMessage>> NextPage => (index, doc, client) =>
        {
            var url = "http://recept.se/recept?page=" + (index + 1).ToString();
            System.Diagnostics.Debug.WriteLine("URL: " + url);
            return client.GetAsync(url);
        };

        public Func<string, string> ProcessIngredient => (ing) => {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);

            return regex.Replace(ing, " ");
        };

        public string RootPage => "http://recept.se";

        public string RecipeItemXPath => "/html/body/div[3]/section/div/div/div/div/div/div/div/div/div[2]/div//div[1]/span/a";

        public string ImageXPath => "/html/body/div[3]/section/div/div/div/div/div[1]/div/div/article/img";

        public string DirectionsXPath => "/html/body/div[3]/section/div/div/div/div/div[1]/div/div/article/section[2]/ol//li/span";

        public string IngredientsXPath => "/html/body/div[3]/section/div/div/div/div/div[1]/div/div/article/section[1]//div[@class=\"ingredient\"]";

        public string NameXPath => "/html/body/div[3]/section/div/div/div/div/div[1]/div/div/article/header/h1";

        public string[] TagXPaths => new string[]{
            "//span[@class='category']/a"
        };
    }
}
