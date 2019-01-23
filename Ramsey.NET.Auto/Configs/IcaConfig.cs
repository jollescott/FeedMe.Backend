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
    public class IcaConfig : IAutoConfig
    {
        public RamseyLocale ProviderLocale => RamseyLocale.Swedish;

        public string ProviderId => "ICA";

        public string ProviderLogo => "https://upload.wikimedia.org/wikipedia/commons/thumb/7/74/ICA-logotyp.png/250px-ICA-logotyp.png";

        public RecipeProvider ProviderName => RecipeProvider.ICA;

        public int RecipeCount => 21059;

        public int PageItemCount => 16;

        public Func<HtmlDocument, string> LoadImage => (doc) => {
            var image = doc.DocumentNode
                .SelectSingleNode("//div[contains(@class, 'recipe-image-square__image')]");
                
            return Regex.Match(image.GetAttributeValue("style", ""), @"(?<=url\()(.*)(?=\))").Groups[1].Value;
        };

        public Func<string, string> ParseId => null;

        public Func<int, HtmlDocument, HttpClient, Task<HttpResponseMessage>> NextPage => (index, doc, client) =>
        {
            return client.GetAsync("https://www.ica.se/templates/ajaxresponse.aspx?id=12&ajaxFunction=RecipeListMdsa&mdsarowentityid=&sortbymetadata=Relevance&start=" + (index * 16).ToString() + "&num=16");
        };

        public Func<string, string> ProcessIngredient => null;

        public string RootPage => "https://www.ica.se/recept/";

        public string RecipeItemXPath => "/html/body/article//div/header/h2/a";

        public string ImageXPath => "";

        public string DirectionsXPath => "/html/body/form/div[4]/div/div[2]/main/section[3]/div[1]/div[2]/howto-steps/ol//li";

        public string IngredientsXPath => "/html/body/form/div[4]/div/div[2]/main/section[3]/div[3]/div[1]/ul//li/span";

        public string NameXPath => "/html/body/form/div[4]/div/div[2]/main/header/div/div[1]/div[2]/div/h1";
    }
}
