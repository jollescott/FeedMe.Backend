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
using System.Threading.Tasks;

namespace Ramsey.NET.Auto
{
    public class RamseyAuto : IRamseyAuto
    {
        private readonly HemmetsHttpClient _client;

        public IAutoConfig Config { get; private set; }

        public RamseyAuto()
        {
            _client = new HemmetsHttpClient();
        }

        public void Init(IAutoConfig config)
        {
            Config = config;
        }

        public async Task<RecipeDtoV2> ScrapeRecipeAsync(string url, bool includeAll = false)
        {
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
                recipe.RecipeID = Config.ProviderId + url.Split('/').Last();

            if(includeAll)
            {
                var nodes = document.DocumentNode
                    .SelectNodes(Config.DirectionsXPath)
                    .Where(x => !x.InnerHtml.Contains("<strong>") || !x.InnerHtml.Contains("<th>"))
                    .Select(x => WebUtility.HtmlDecode(x.InnerText))
                    .Select(x => x.RemoveSpecialCharacters())
                    .ToList();
            }

            return recipe;
        }

        public async Task<Dictionary<string, bool>> ScrapeRecipesAsync(IRecipeManager recipeManager, int amount = 50)
        {
            int page = 0;

            while(page < Config.RecipeCount / Config.PageItemCount)
            {
                var document = new HtmlDocument();
                document.Load(Config.NextPage(page, document));

                var links = document.DocumentNode.SelectNodes(Config.RecipeItemXPath)
                    .Select(x => x.Attributes["href"].Value).ToList();

                foreach(var link in links)
                {
                    var recipe = await ScrapeRecipeAsync(link);
                    await recipeManager.UpdateRecipeMetaAsync(recipe);
                }
            }

            return null;
        }
    }
}
