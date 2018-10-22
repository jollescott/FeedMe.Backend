using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GusteauSharp.Models
{
    public class RecommendationRating
    {
        public int RatingID { get; set; }
        public string UserID { get; set; }

        public int RecipeID { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
