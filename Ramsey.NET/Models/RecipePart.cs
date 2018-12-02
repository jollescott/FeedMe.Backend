namespace Ramsey.NET.Models
{
    public class RecipePart
    {
        public int RecipePartId { get; set; }

        public string IngredientId { get; set; }
        public string RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual Ingredient Ingredient { get; set; }
    }
}
