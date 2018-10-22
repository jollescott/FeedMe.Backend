using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GusteauSharp.Dto
{
    public class RecipeDto
    {
        public int RecipeID { get; set; }
        public string Name { get; set; }

        public int NumberOfServings { get; set; }
        public double TotalTimeInSeconds { get; set; }

        public int Rating { get; set; }

        public string Cuisine { get; set; }
        public string Course { get; set; }

        public string MediumImage { get; set; }
        public string SmallImage { get; set; }

        public float Bitter { get; set; }
        public float Meaty { get; set; }
        public float Piquant { get; set; }
        public float Salty { get; set; }
        public float Sour { get; set; }
        public float Sweet { get; set; }

        public double Coverage { get; set; }
    }
}
