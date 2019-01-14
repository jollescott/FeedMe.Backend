using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

        public virtual ICollection<RecipeFavorite> RecipeFavorites { get; set; }
        public virtual ICollection<RecipeRating> RecipeRatings { get; set; }
    }
}
