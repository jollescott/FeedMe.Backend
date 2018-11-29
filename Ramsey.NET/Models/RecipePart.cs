namespace Ramsey.NET.Models
{
    public class RecipePart
    {
        public int RecipePartID { get; set; }

        public int IngredientID { get; set; }
        public int RecipeID { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual Ingredient Ingredient { get; set; }
    }
}
