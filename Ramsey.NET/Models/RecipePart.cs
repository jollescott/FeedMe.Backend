using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GusteauSharp.Models
{
    public class RecipePart
    {
        public int RecipePartID { get; set; }

        public int IngredientID { get; set; }
        public int RecipeID { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual Ingredient Ingredient { get; set; }

        public string Unit { get; set; }
        public double Quantity { get; set; }
    }
}
