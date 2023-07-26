using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Ramsey.Core.Interfaces;
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
                
            return Regex.Match(image.GetAttributeValue("style", ""), @"(?<=url\()(.*)(?=\))")
                            .Groups[1]
                            .Value
                            .Replace("&#39;", string.Empty)
                            .Replace("//", string.Empty)
                            .Insert(0, "https://");
        };

        public Func<string, string> ParseId => null;

        public Func<int, HtmlDocument, HttpClient, Task<HttpResponseMessage>> NextPage => (index, doc, client) =>
        {
            var url = "https://www.ica.se/templates/ajaxresponse.aspx?id=12&ajaxFunction=RecipeListMdsa&mdsarowentityid=&sortbymetadata=Relevance&start=" + (index * 16).ToString() + "&num=16";
            return client.GetAsync(url);
        };

        public Func<string, string> ProcessIngredient => (str) => {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("(([1-9]? ?)([1-9]/[1-9]))", options);

            var match = regex.Match(str);

            if (!match.Success)
                return str;
            else
            {
                var quantity = match.Value;
                var parts = quantity.Split(' ');
                float count = 0;

                foreach(var part in parts)
                {
                    if(part.Contains("/"))
                    {
                        var divParts = part.Split('/');

                        float.TryParse(divParts[0], out var num1);
                        float.TryParse(divParts[1], out var num2);

                        if (Math.Abs(num2) < 0.00001) continue;
                        var result = num1 / num2;
                        count += result;
                    }
                    else
                    {
                        float.TryParse(part, out var result);
                        count += result;
                    }
                }

                return str.Replace(quantity, count.ToString(CultureInfo.InvariantCulture));
            }
        };

        public string RootPage => "https://www.ica.se/recept/";

        public string RecipeItemXPath => "//a[@class = \"js-track-listing-recipe\"]";

        public string ImageXPath => "";

        public string DirectionsXPath => "//howto-steps/ol/li";

        public string IngredientsXPath => "//ul[@class='ingredients__list']/li/span";

        public string NameXPath => "/html/body/form/div[4]/div/div[2]/main/header/div/div[1]/div[2]/div/h1";

        public string[] TagXPaths => new string[] {
            "//div[@class='related-recipe-tags__container']/a"
        };

        public Func<HtmlDocument, string[]> ProcessTag => null;
    }
}
