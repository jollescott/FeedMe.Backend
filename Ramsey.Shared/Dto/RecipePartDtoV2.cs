using System;
using System.Collections.Generic;
using System.Text;

namespace Ramsey.Shared.Dto
{
    public class RecipePartDtoV2
    {
        public string IngredientID { get; set; }
        public string RecipeID { get; set; }

        public string Unit { get; set; }
        public float Quantity { get; set; }
    }
}
