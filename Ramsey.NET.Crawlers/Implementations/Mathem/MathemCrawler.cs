using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Extensions;

namespace Ramsey.NET.Crawlers.Implementations.Mathem
{
    public class MathemCrawler : AMathemCrawler
    {
        private static readonly string MathemAllRecipesApi = "https://api.mathem.io/ecom-recipe/noauth/search/query?q=&index=0&size=1000";

        private static readonly string MathemRecipeDetailsApi =
            "https://api.mathem.io/ecom-recipe/noauth/recipes/detail?url=";
        
        private readonly HttpClient _httpClient = new HttpClient();
        
        public override Task<IList<RecipeMetaDtoV2>> ScrapeRecipesAsync(int amount = 50)
        {
            throw new System.NotImplementedException();
        }

        public override async Task<RecipeDtoV2> ScrapeRecipeAsync(string url, bool includeAll = false)
        {
            var id = url.Split('/').Last();
            var response = await _httpClient.GetAsync(MathemRecipeDetailsApi + id);
            var json = await response.Content.ReadAsSwedishStringAsync();

            var details = GetDetailsFromJson(json);
            return details.ToRecipeDtoV2(includeAll);
        }

        public override MathemRecipeDetails GetDetailsFromJson(string json)
        {
            return JsonConvert.DeserializeObject<MathemRecipeDetails>(json);
        }
    }
}