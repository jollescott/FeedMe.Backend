using Ramsey.Shared.Enums;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ramsey.Shared.Dto.V2
{
    public class RecipeDtoV2
    {
        public string RecipeId { get; set; }
        public string Name { get; set; }

        public IEnumerable<string> Ingredients { get; set; }
        public IEnumerable<RecipePartDtoV2> RecipeParts { get; set; }
        public IEnumerable<string> Directions { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }

        [DefaultValue("")]
        public string Desc { get; set; }

        public string Source { get; set; }
        public RecipeProvider Owner { get; set; }
        public string OwnerLogo { get; set; }
        public string Image { get; set; }
    }
}
