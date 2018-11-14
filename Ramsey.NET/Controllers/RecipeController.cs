using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GusteauSharp.Dto;
using GusteauSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Dto;
using Ramsey.NET.Models;

namespace Ramsey.NET.Controllers
{
    [Route("recipe")]
    public class RecipeController : Controller
    {
        private readonly RamseyContext _ramseyContext;

        public RecipeController(RamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
        }

        [Route("upload")]
        [HttpPost]
        public IActionResult Upload([FromBody]RecipeUploadDto recipeDto)
        {
            if (recipeDto == null)
                return new StatusCodeResult(200);

            Recipe recipe = new Recipe
            {
                Name = recipeDto.Title,
                Rating = recipeDto.Rating,
                
                Fat = recipeDto.Fat,
                Protein = recipeDto.Protein,
                Sodium = recipeDto.Sodium
            };

            if (recipeDto.Desc != null)
                recipe.Desc = recipeDto.Desc;

            _ramseyContext.Recipes.Add(recipe);
            _ramseyContext.SaveChanges();
            
            if(recipeDto.Directions != null)
            {
                foreach (var direction in recipeDto.Directions)
                {
                    var recipeDirection = new RecipeDirection();
                    recipeDirection.Instruction = direction;
                    recipeDirection.RecipeID = recipe.RecipeID;

                    _ramseyContext.RecipeDirections.Add(recipeDirection);
                }
            }

            _ramseyContext.SaveChanges();

            if(recipeDto.Categories != null)
            {
                foreach (var category in recipeDto.Categories)
                {
                    var recipeCategory = new RecipeCategory();
                    recipeCategory.Name = category;
                    recipeCategory.RecipeID = recipe.RecipeID;

                    _ramseyContext.RecipeCategories.Add(recipeCategory);
                }
            }

            _ramseyContext.SaveChanges();

            foreach(var ingredientLine in recipeDto.Ingredients)
            {
                var parts = ingredientLine.Split(' ');

                double quantity;

                if(parts[0].Contains('/'))
                {
                    var nums = parts[0].Split('/');

                    double num1, num2;
                    if (!double.TryParse(nums[0], out num1))
                        num1 = 1;

                    if (!double.TryParse(nums[1], out num2))
                        num2 = 1;

                    parts[0] = (num1 / num2).ToString();
                }

                if (!double.TryParse(parts[0], out quantity))
                    quantity = 1;

                string unit;

                try
                {
                    unit = parts[1];
                }
                catch(IndexOutOfRangeException)
                {
                    unit = "";
                }
 
                string name = string.Join(' ', parts.Skip(2));

                Ingredient ingredient;

                if(_ramseyContext.Ingredients.Where(x => x.Name.Equals(name)).FirstOrDefault() == null)
                {
                    ingredient = new Ingredient
                    {
                        Name = name
                    };

                    _ramseyContext.Ingredients.Add(ingredient);
                    _ramseyContext.SaveChanges();
                }
                else
                {
                    ingredient = _ramseyContext.Ingredients.Where(x => x.Name.Equals(name)).First();
                }

                var part = new RecipePart();

                part.Unit = unit;
                part.Quantity = quantity;
                part.IngredientID = ingredient.IngredientID;
                part.RecipeID = recipe.RecipeID;

                _ramseyContext.RecipeParts.Add(part);
            }

            _ramseyContext.SaveChanges();

            return new StatusCodeResult(200);
        }

        [Route("suggest")]
        [HttpPost]
        public async Task<IActionResult> SuggestAsync([FromBody]List<IngredientDto> ingredients)
        {
            /*var ids = new HashSet<int>();
            foreach (var ingredient in ingredients)
            {
                foreach (var part in ingredient.RecipeParts.Where(x => ingredients.Select(y => y.RecipeParts)
                    .All(y => y.Any(z => z.RecipeID.Equals(x.RecipeID)))))
                    ids.Add(part.RecipeID);
            }*/

            /*var recipes = await _ramseyContext.Recipes.Where(x => ids.Contains(x.RecipeID))
                .Include(y => y.RecipeParts)
                .Include(x => x.Categories)
                .Include(x => x.Directions)
                .ToListAsync();*/

            var recipePartIds = new HashSet<int>();

            foreach(var ing in ingredients)
            {
                ing.RecipeParts.ForEach(x => recipePartIds.Add(x.RecipeID));
            }

            var recipes = await _ramseyContext.Recipes.Where(x => recipePartIds.Contains(x.RecipeID))
                .Include(y => y.RecipeParts)
                .Include(x => x.Categories)
                .Include(x => x.Directions)
                .ToListAsync();

            List<RecipeDto> dtos = new List<RecipeDto>();
            foreach(var recipe in recipes)
            {
                var dto = new RecipeDto
                {
                    Name = recipe.Name,
                    RecipeID = recipe.RecipeID,
                    Date = recipe.Date,
                    Desc = recipe.Desc,
                    Fat = recipe.Fat,
                    Protein = recipe.Protein,
                    Rating = recipe.Rating,
                    Sodium = recipe.Sodium,
                };

                if(recipe.Categories != null)
                {
                    foreach(var category in recipe.Categories)
                    {
                        dto.Categories.Add(new RecipeCategoryDto
                        {
                            CategoryID = category.CategoryID,
                            Name = category.Name,
                            RecipeID = category.RecipeID
                        });
                    }
                }

                if(recipe.Directions != null)
                {
                    foreach(var direction in recipe.Directions)
                    {
                        dto.Directions.Add(new RecipeDirectionDto
                        {
                            DirectionID = direction.DirectionID,
                            Instruction = direction.Instruction,
                            RecipeID = direction.RecipeID
                        });
                    }
                }

                if(recipe.RecipeParts != null)
                {
                    foreach(var part in recipe.RecipeParts)
                    {
                        dto.RecipeParts.Add(new RecipePartDto
                        {
                            IngredientID = part.IngredientID,
                            Quantity = part.Quantity,
                            RecipeID = part.RecipeID,
                            Unit = part.Unit
                        });
                    }
                }

                dtos.Add(dto);
            }
            
            return Json(dtos);
        }
    }
}