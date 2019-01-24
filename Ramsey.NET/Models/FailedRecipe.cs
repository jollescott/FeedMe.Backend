using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Models
{
    public class FailedRecipe
    {
        public int FailedRecipeId { get; set; }
        public string Url { get; set; }
        public string Message { get; set; }
    }
}
