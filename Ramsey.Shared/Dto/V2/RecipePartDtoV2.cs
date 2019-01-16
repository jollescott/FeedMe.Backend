﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ramsey.Shared.Dto.V2
{
    public class RecipePartDtoV2
    {
        public int IngredientID { get; set; }
        public string IngredientName { get; set; }
        public string RecipeID { get; set; }

        public string Unit { get; set; }
        public float Quantity { get; set; }
    }
}
