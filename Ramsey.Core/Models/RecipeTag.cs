using Ramsey.NET.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ramsey.Core.Models
{
    public class RecipeTag
    {
        public int RecipeTagId { get; set; }

        public int RecipeId { get; set; }
        public int TagId { get; set; }

        public virtual RecipeMeta Recipe { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
