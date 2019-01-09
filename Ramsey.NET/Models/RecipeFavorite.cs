﻿using MySql.Data.EntityFrameworkCore.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Models
{
    [MySqlCharset("utf8")]
    public class RecipeFavorite
    {
        public int RecipeFavoriteId { get; set; }

        public string UserId { get; set; }
        public string RecipeId { get; set; }

        public virtual RecipeMeta Recipe { get; set; }
        public virtual User Ingredient { get; set; }
    }
}
