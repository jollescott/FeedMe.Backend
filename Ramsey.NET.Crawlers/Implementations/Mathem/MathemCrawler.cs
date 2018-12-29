using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Extensions;

namespace Ramsey.NET.Crawlers.Implementations.Mathem
{
    public class MathemCrawler : AMathemCrawler
    {
        private static readonly string MathemAllRecipesApi = "https://api.mathem.io/ecom-recipe/noauth/search/query?q=&index=0&size=";

        private static readonly string MathemRecipeDetailsApi =
            "https://api.mathem.io/ecom-recipe/noauth/recipes/detail?url=";
        
        private readonly HttpClient _httpClient = new HttpClient();
        
        public override async Task<Dictionary<string, bool>> ScrapeRecipesAsync(IRecipeManager recipeManager,int amount = 1000)
        {
            var response = await _httpClient.GetAsync(MathemAllRecipesApi + amount);
            var json = await response.Content.ReadAsSwedishStringAsync();

            var mRecipes = JsonConvert.DeserializeObject<IEnumerable<MathemRecipeDetails>>(json);

            var recipes = mRecipes.Select(x => (RecipeMetaDtoV2)x.ToRecipeDtoV2());
            var updateTasks = recipes.Select(recipeManager.UpdateRecipeMetaAsync);
            await Task.WhenAll(updateTasks);

            await recipeManager.SaveRecipeChangesAsync();
            
            return new Dictionary<string, bool>();
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