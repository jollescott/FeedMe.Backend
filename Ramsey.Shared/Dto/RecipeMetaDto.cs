namespace Ramsey.Shared.Dto
{
    public class RecipeMetaDto
    {
        public string RecipeID { get; set; }
        public string Name { get; set; }

        public string Source { get; set; }
        public RecipeProvider Owner { get; set; }
        public string OwnerLogo { get; set; }
        public string Image { get; set; }

        public static implicit operator RecipeMetaDto(RecipeDto recipe)
        {
            RecipeMetaDto recipeMetaDto = new RecipeMetaDto();
            recipeMetaDto.Name = recipe.Name;
            recipeMetaDto.Image = recipe.Image;
            recipeMetaDto.Owner = recipe.Owner;
            recipeMetaDto.OwnerLogo = recipe.OwnerLogo;
            recipeMetaDto.RecipeID = recipe.RecipeID;
            recipeMetaDto.Source = recipe.Source;

            return recipeMetaDto;
        }
    }
}
