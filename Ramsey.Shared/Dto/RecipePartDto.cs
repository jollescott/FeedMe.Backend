namespace Ramsey.Shared.Dto
{
    public class RecipePartDto
    {
        public int IngredientID { get; set; }
        public int RecipeID { get; set; }

        public string Unit { get; set; }
        public double Quantity { get; set; }
    }
}