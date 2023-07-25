namespace Ramsey.Shared.Dto.V2
{
    public class RecipePartDtoV2
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public string RecipeId { get; set; }

        public string Unit { get; set; }
        public float Quantity { get; set; }
    }
}
