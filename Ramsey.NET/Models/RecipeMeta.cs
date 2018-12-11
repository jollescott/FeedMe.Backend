using Ramsey.NET.Models;
using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Models
{
    public class RecipeMeta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string RecipeId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<RecipePart> RecipeParts { get; set; }

        public string Source { get; set; }
        public RecipeProvider Owner { get; set; }
        public string OwnerLogo { get; set; }
        public string Image { get; set; }
    }
}
