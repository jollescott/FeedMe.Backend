using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GusteauSharp.Models
{
    public class Ingredient
    {
        public int IngredientID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RecipePart> RecipeParts { get; set; }
    }
}
