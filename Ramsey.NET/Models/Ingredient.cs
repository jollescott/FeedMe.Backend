using System.Collections.Generic;

namespace Ramsey.NET.Models
{
    public class Ingredient 
    {
        public string IngredientId { get; set; }
        public virtual ICollection<RecipePart> RecipeParts { get; set; }
    }
}
