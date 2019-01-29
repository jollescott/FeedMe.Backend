using HtmlAgilityPack;
using Ramsey.Core;
using Ramsey.NET.Auto.Extensions;
using Ramsey.NET.Crawlers.Interfaces;
using Ramsey.NET.Crawlers.Misc;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto.V2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ramsey.NET.Auto
{
    public class RamseyAuto : IRecipeCrawler
    {
        private readonly HemmetsHttpClient _client;
        private readonly IRamseyContext _ramseyContext;
        private readonly IIllegalRemover _illegalRemover;

        public IAutoConfig Config { get; private set; }

        public RamseyAuto(IAutoConfig autoConfig, IRamseyContext ramseyContext, IIllegalRemover illegalRemover)
        {
            _client = new HemmetsHttpClient();
            _ramseyContext = ramseyContext;
            _illegalRemover = illegalRemover;
            Config = autoConfig;
        }


        public async Task<RecipeDtoV2> ScrapeRecipeAsync(string url, bool includeAll = false)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var recipe = new RecipeDtoV2();
            var httpResponse = await _client.GetAsync(url);
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
                var id = url.ToString();
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
                    .Select(x => x.ToLower())
                    .Select(x => x.Trim())
                    .ToList();

            var ingRegex = new Regex("([0-9]\\d*(\\,\\d+)? \\w+)");
            var quantRegex = new Regex("([0-9]\\d*(\\,\\d+)?)");

            var parts = new List<RecipePartDtoV2>();

            foreach(var ing in recipe.Ingredients)
            {
                var ingredient = ing;

                //Optional processing by provider
                if (Config.ProcessIngredient != null)
                    ingredient = Config.ProcessIngredient(ingredient);

                var match = ingRegex.Match(ingredient);

                if(match.Success)
                {
                    var amount = match.Value;

                    //Find ingredient name
                    var name = ingredient.Replace(amount,string.Empty);

                    //Remove illegal words
                    name = _illegalRemover.RemoveIllegals(name);

                    //Match the name
                    var nameMatch = Regex.Match(name, "([a-zåäöèîé]{3,})");

                    if (nameMatch.Success)
                    {
                        name = nameMatch.Value;
                    }
                    else
                        //If pattern is not detected then skip it.
                        continue;

                    var quantityMatch = quantRegex.Match(amount);

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
            recipe.Source = url.ToString();
            recipe.Owner = Config.ProviderName;

            stopWatch.Stop();

            Debug.WriteLine("Recipe {0} took {1} ms to scrape.", recipe.Name, stopWatch.Elapsed.Milliseconds);

            return recipe;
        }

        public async Task ScrapeRecipesAsync(IRecipeManager recipeManager)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var document = new HtmlDocument();

            var pages = Config.RecipeCount / Config.PageItemCount;
            System.Diagnostics.Debug.WriteLine(pages);

            for (var page = 0; page < pages; page++)
            {
                var httpResponseTask = Config.NextPage(page, document, _client);
                var httpResponse = await httpResponseTask;

                var html = await httpResponse.Content.ReadAsStringAsync();

                document.LoadHtml(html);

                var list = document.DocumentNode.SelectNodes(Config.RecipeItemXPath);
                
                if(list != null)
                {
                    var links = list.Select(x => x.Attributes["href"].Value).ToList();

                    foreach (var link in links)
                    {
                        Uri uri = null;

                        try
                        {
                            if (Uri.IsWellFormedUriString(link, UriKind.Absolute))
                                uri = new Uri(link);
                            else
                                uri = new Uri(Config.RootPage + link);

                            var recipe = await ScrapeRecipeAsync(uri.ToString());
                            await recipeManager.UpdateRecipeMetaAsync(recipe);
                        }
                        catch (Exception ex)
                        {
                            var trace = ex.StackTrace != null ? ex.StackTrace : string.Empty;
                            var failingLink = uri != null ? uri.ToString() : string.Empty;

                            await recipeManager.ReportFailedRecipeAsync(failingLink, trace);
                        }
                    }
                }
                else
                {

                }
            }

            stopWatch.Stop();
            Debug.WriteLine("{0} took {1} min to rescrape.", Config.ProviderName, stopWatch.Elapsed.Minutes);
        }
    }
}
