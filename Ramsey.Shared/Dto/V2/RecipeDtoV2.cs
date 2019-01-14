﻿using Ramsey.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ramsey.Shared.Dto.V2
{
    public class RecipeDtoV2
    {
        public string RecipeID { get; set; }
        public string Name { get; set; }

        public IEnumerable<string> Ingredients { get; set; }
        public IEnumerable<RecipePartDtoV2> RecipeParts { get; set; }

        public IEnumerable<string> Directions { get; set; }

        [DefaultValue("")]
        public string Desc { get; set; }

        public string Source { get; set; }
        public RecipeProvider Owner { get; set; }
        public string OwnerLogo { get; set; }
        public string Image { get; set; }
    }
}
