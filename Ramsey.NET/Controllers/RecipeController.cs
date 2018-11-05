using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GusteauSharp.Dto;
using GusteauSharp.Models;
using Microsoft.AspNetCore.Mvc;
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

    }
}