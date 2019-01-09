using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.EntityFrameworkCore.DataAnnotations;

namespace Ramsey.NET.Models
{
    [MySqlCharset("utf8")]
    public class Ingredient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string IngredientId { get; set; }

        public virtual ICollection<RecipePart> RecipeParts { get; set; }
    }
}
