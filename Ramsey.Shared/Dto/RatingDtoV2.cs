﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ramsey.Shared.Dto
{
    public class RatingDtoV2
    {
        public string RecipeId { get; set; }
        public string UserId { get; set; }
        public double Score { get; set; }
    }
}