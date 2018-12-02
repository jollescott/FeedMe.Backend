using Ramsey.NET.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Models
{
    public class Recipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string RecipeId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Ingredient> RecipeParts { get; set; }

        public string Source { get; set; }
        public string Owner { get; set; }
        public string Image { get; set; }
    }
}
