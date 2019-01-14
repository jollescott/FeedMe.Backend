using Ramsey.NET.Models;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Enums;
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
        public string RecipeId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<RecipePart> RecipeParts { get; set; }

        public string Source { get; set; }
        public RecipeProvider Owner { get; set; }
        public RamseyLocale Locale { get; set; }
        public string OwnerLogo { get; set; }
        public string Image { get; set; }
        public double Rating { get; set; }
    }
}
