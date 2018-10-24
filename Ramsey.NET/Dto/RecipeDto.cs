using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GusteauSharp.Dto
{
    public class RecipeUploadDto
    {
        public List<string> Directions { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Ingredients { get; set; }

        public double Fat { get; set; }
        public double Calories { get; set; }
        public string Desc { get; set; }
        public double Protein { get; set; }
        public double Rating { get; set; }
        
        public string Title { get; set; }
        public double Sodium { get; set; }
    }
}
