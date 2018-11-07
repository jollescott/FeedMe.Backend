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
        public int RecipeID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<RecipePart> RecipeParts { get; set; }
        public virtual ICollection<RecipeDirection> Directions { get; set; }
        public virtual ICollection<RecipeCategory> Categories { get; set; }

        public double Fat { get; set; }
        public DateTime Date { get; set; }

        [DefaultValue("")]
        public string Desc { get; set; }

        public double Protein { get; set; }
        public double Rating { get; set; }
        public double Sodium { get; set; }
    }
}
