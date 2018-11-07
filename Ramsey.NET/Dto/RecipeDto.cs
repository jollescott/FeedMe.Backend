using GusteauSharp.Models;
using Ramsey.NET.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Dto
{
    public class RecipeDto
    {
        public RecipeDto()
        {
            RecipeParts = new List<RecipePartDto>();
            Directions = new List<RecipeDirectionDto>();
            Categories = new List<RecipeCategoryDto>();
        }

        public int RecipeID { get; set; }
        public string Name { get; set; }

        public List<RecipePartDto> RecipeParts { get; set; }
        public List<RecipeDirectionDto> Directions { get; set; }
        public List<RecipeCategoryDto> Categories { get; set; }

        public double Fat { get; set; }
        public DateTime Date { get; set; }

        [DefaultValue("")]
        public string Desc { get; set; }

        public double Protein { get; set; }
        public double Rating { get; set; }
        public double Sodium { get; set; }
    }

    public class RecipeCategoryDto
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public int RecipeID { get; set; }
    }

    public class RecipeDirectionDto
    {
        public int DirectionID { get; set; }
        public string Instruction { get; set; }

        public int RecipeID { get; set; }
    }
}
