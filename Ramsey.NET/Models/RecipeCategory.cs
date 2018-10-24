
using GusteauSharp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Models
{
    public class RecipeCategory
    {
        [Key]
        public int CategoryID { get; set; }

        public string Name { get; set; }

        public int RecipeID { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
