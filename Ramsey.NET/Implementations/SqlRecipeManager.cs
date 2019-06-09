using Ramsey.NET.Extensions;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Models;
using System.Threading.Tasks;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto.V2;
using System.Diagnostics;

namespace Ramsey.NET.Implementations
{
    public class SqlRecipeManager : IRecipeManager
    {
        private readonly IRamseyContext _context;

        public SqlRecipeManager(IRamseyContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateRecipeMetaAsync(RecipeMetaDtoV2 recipeMetaDto)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var recipe = _context.Recipes.AddIfNotExists(new RecipeMeta
            {
                RecipeId = recipeMetaDto.RecipeId,
            }, x => x.RecipeId == recipeMetaDto.RecipeId);

            await SaveRecipeChangesAsync();

            //Basic Properties
            recipe.Image = recipeMetaDto.Image;
            recipe.Name = recipeMetaDto.Name;
            recipe.Owner = recipeMetaDto.Owner;
            recipe.OwnerLogo = recipeMetaDto.OwnerLogo;
            recipe.Rating = 0;
            recipe.Source = recipeMetaDto.Source;

            //Ingredients
            foreach(var partDto in recipeMetaDto.RecipeParts)
            {
                if (string.IsNullOrEmpty(partDto.IngredientName))
                    continue;

                string ingredientName = partDto.IngredientName;
                string recipeId = recipeMetaDto.RecipeId;

                var ingredient = _context.Ingredients.AddIfNotExists(new Ingredient
                {
                    IngredientName = ingredientName
                }, x => x.IngredientName == ingredientName);

                await SaveRecipeChangesAsync();

                var part = _context.RecipeParts.AddIfNotExists(new RecipePart
                {
                    RecipeId = recipeId,
                    IngredientId = ingredient.IngredientId
                }, x => x.RecipeId == recipeId && x.Ingredient.IngredientName == ingredientName);

                await SaveRecipeChangesAsync();

                part.IngredientId = ingredient.IngredientId;
                part.Recipe = recipe;

                part.Quantity = partDto.Quantity;
                part.Unit = partDto.Unit;

                _context.RecipeParts.Update(part);

                await SaveRecipeChangesAsync();
                
                recipe.RecipeParts.Add(part);
            }

            //Tags
            foreach(var tagDto in recipeMetaDto.Tags)
            {
                var tag = _context.Tags.AddIfNotExists(new Core.Models.Tag
                {
                    Name = tagDto.Name,
                }, x => x.Name == tagDto.Name);

                await _context.SaveChangesAsync();

                _context.RecipeTags.AddIfNotExists(new Core.Models.RecipeTag
                {
                    RecipeId = recipe.RecipeId,
                    TagId = tag.TagId
                }, x => x.TagId == tag.TagId && x.Recipe.RecipeId == recipe.RecipeId);

                await _context.SaveChangesAsync();
            }

            _context.Recipes.Update(recipe);
            await SaveRecipeChangesAsync();

            stopWatch.Stop();

            Debug.WriteLine("Recipe {0} took {1} ms to reload.", recipe.Name, stopWatch.Elapsed.Milliseconds);

            return true;
        }

        public async Task<bool> SaveRecipeChangesAsync()
        {
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task ReportFailedRecipeAsync(string url, string message)
        {
            _context.FailedRecipes.Add(new FailedRecipe
            {
                Url = url,
                Message = message
            });

            await _context.SaveChangesAsync();
        }
    }
}
