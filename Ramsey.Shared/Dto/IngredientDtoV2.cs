﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ramsey.Shared.Dto
{
    public class IngredientDtoV2
    {
        public string IngredientId { get; set; }
        public List<RecipePartDtoV2> RecipeParts { get; set; }
    }
}
