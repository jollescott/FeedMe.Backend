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
    public class TastelineConfig : IAutoConfig
    {
        public RamseyLocale ProviderLocale => RamseyLocale.Swedish;

        public string ProviderId => "TSL";

        public string ProviderLogo => "https://cdn1.tasteline.com/Tasteline_Logotyp_Svart.png";

        public RecipeProvider ProviderName => RecipeProvider.Tasteline;

        public int RecipeCount => 28284;

        public int PageItemCount => 12;

        public Func<HtmlDocument, string> LoadImage => null;

        public Func<string, string> ParseId => null;

        public Func<int, HtmlDocument, HttpClient, Task<HttpResponseMessage>> NextPage => (index, doc, client) => client.GetAsync("https://www.tasteline.com/recept/?sida=" + index + 1);

        public Func<string, string> ProcessIngredient => (ing) => {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);

            return regex.Replace(ing, " ");
        };

        public string RootPage => "https://www.tasteline.com/recept/";

        public string RecipeItemXPath => "//div[contains(@class, 'recipe-description')]/a";

        public string ImageXPath => "/html/body/div[1]/div[1]/div/div[2]/div[1]/div/div[1]/img";

        public string DirectionsXPath => "/html/body/div[1]/div[1]/div/div[4]/div[1]/div[2]/div[2]/div[1]/ul//li";

        public string IngredientsXPath => "/html/body/div[1]/div[1]/div/div[4]/div[1]/div[2]/div[1]/div/ul//li";

        public string NameXPath => "/html/body/div[1]/div[1]/div/div[2]/div[1]/div/div[2]/h1";

        public string[] TagXPaths => new string[] {
            "//*[@class='category-value']"
        };

        public Func<HtmlDocument, string[]> ProcessTag => null;
    }
}
