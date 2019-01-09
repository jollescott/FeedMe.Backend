using MySql.Data.EntityFrameworkCore.DataAnnotations;

namespace Ramsey.NET.Models
{
    [MySqlCharset("utf8")]
    public class RecipePart
    {
        public int RecipePartId { get; set; }

        public string IngredientId { get; set; }
        public string RecipeId { get; set; }

        public virtual RecipeMeta Recipe { get; set; }
        public virtual Ingredient Ingredient { get; set; }

        public string Unit { get; set; }
        public float Quantity { get; set; }
    }
}
