using Ramsey.NET.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GusteauSharp.Models
{
    public class Recipe
    {
        public int RecipeID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<RecipePart> RecipeParts { get; set; }
        public virtual ICollection<RecipeDirection> Directions { get; set; }

        public int Fat { get; set; }
        public DateTime Date { get; set; }

        [DefaultValue("")]
        public string Desc { get; set; }

        public int Protein { get; set; }
        public double Rating { get; set; }
        public string Title { get; set; }
        public int Sodium { get; set; }
    }
}
