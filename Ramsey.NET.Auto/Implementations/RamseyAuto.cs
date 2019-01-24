using HtmlAgilityPack;
using Ramsey.NET.Auto.Extensions;
using Ramsey.NET.Auto.Interfaces;
using Ramsey.NET.Crawlers.Misc;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ramsey.NET.Auto
{
    public class RamseyAuto : IRamseyAuto
    {
        private readonly HemmetsHttpClient _client;

        public IAutoConfig Config { get; private set; }

        public RamseyAuto(IAutoConfig autoConfig = null)
        {
            _client = new HemmetsHttpClient();
            Config = autoConfig;
        }

        public void Init(IAutoConfig config)
        {
            Config = config;
        }

        public async Task<RecipeDtoV2> ScrapeRecipeAsync(string url, bool includeAll = false)
        {
            Uri uri;
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                uri = new Uri(url);
            else
                uri = new Uri(Config.RootPage + url);

            var recipe = new RecipeDtoV2();
            var httpResponse = await _client.GetAsync(uri);
            var html = await httpResponse.Content.ReadAsStringAsync();

            var document = new HtmlDocument();
            document.LoadHtml(html);

            recipe.Name = document.DocumentNode
                .SelectSingleNode(Config.NameXPath)
                .InnerText
                .RemoveSpecialCharacters();

            if (Config.LoadImage != null)
                recipe.Image = Config.LoadImage(document);
            else
                recipe.Image = document.DocumentNode
                    .SelectSingleNode(Config.ImageXPath)
                    .Attributes["src"]
                    .Value;

            if (Config.ParseId != null)
                recipe.RecipeID = Config.ParseId(url);
            else
            {
                var id = uri.ToString();
                recipe.RecipeID = Config.ProviderId + id.Remove(id.Count() - 1).Split('/').Last();
            }

            if(includeAll)
            {
                recipe.Directions = document.DocumentNode
                    .SelectNodes(Config.DirectionsXPath)
                    .Where(x => !x.InnerHtml.Contains("<strong>") || !x.InnerHtml.Contains("<th>"))
                    .Select(x => WebUtility.HtmlDecode(x.InnerText))
                    .Select(x => x.RemoveSpecialCharacters())
                    .ToList();
            }

            recipe.Ingredients = document.DocumentNode
                    .SelectNodes(Config.IngredientsXPath)
                    .Select(x => WebUtility.HtmlDecode(x.InnerText))
                    .Select(x => x.Replace("\t", ""))
                    .Select(x => x.Replace("\n", string.Empty))
                    .Select(x => x.Replace('.', ','))
                    .ToList();

            var regex = new Regex("([0-9]\\d*(\\,\\d+)? \\w+)");
            var qRegex = new Regex("([0-9]\\d*(\\,\\d+)?)");

            var parts = new List<RecipePartDtoV2>();

            foreach(var ing in recipe.Ingredients)
            {
                var ingredient = ing;

                if (Config.ProcessIngredient != null)
                    ingredient = Config.ProcessIngredient(ingredient);

                var match = regex.Match(ingredient);

                if(match.Success)
                {
                    var amount = match.Value;
                    var name = ingredient.Replace(amount,string.Empty);

                    var quantityMatch = qRegex.Match(amount);

                    double.TryParse(quantityMatch.Value, out double quantity);

                    var unit = amount.Replace(quantityMatch.Value, string.Empty);

                    parts.Add(new RecipePartDtoV2
                    {
                        IngredientName = name.Trim(),
                        Quantity = (float)quantity,
                        Unit = unit.Trim(),
                    });
                }
            }

            recipe.RecipeParts = parts;

            recipe.OwnerLogo = Config.ProviderLogo;
            recipe.Source = uri.ToString();
            recipe.Owner = Config.ProviderName;

            return recipe;
        }

        public async Task<Dictionary<string, bool>> ScrapeRecipesAsync(IRecipeManager recipeManager, int amount = 50)
        {
            var dict = new Dictionary<string, bool>();

            int page = 0;
            var document = new HtmlDocument();

            while (page < Config.RecipeCount / Config.PageItemCount)
            {
                var httpResponseTask = Config.NextPage(page, document, _client);
                var httpResponse = await httpResponseTask;

                var html = await httpResponse.Content.ReadAsStringAsync();

                document.LoadHtml(html);

                var list = document.DocumentNode.SelectNodes(Config.RecipeItemXPath);
                var links = list.Select(x => x.Attributes["href"].Value).ToList();

                foreach (var link in links)
                {
                    try
                    {
                        var recipe = await ScrapeRecipeAsync(link);
                        await recipeManager.UpdateRecipeMetaAsync(recipe);
                    }
                    catch (Exception ex)
                    {
                        dict.Add(link, false);
                    }
                }

                page++;
            }

            return null;
        }
    }
}
