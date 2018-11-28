using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.Shared.Dto
{
    public class IngredientDto
    {
        public string Name { get; set; }
        public List<RecipePartDto> RecipeParts { get; set; }
    }
}
