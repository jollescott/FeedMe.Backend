using System.Collections.Generic;

namespace Ramsey.NET.Models
{
    public class Ingredient 
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public virtual ICollection<RecipePart> RecipeParts { get; set; }
    }
}
