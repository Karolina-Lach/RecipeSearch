using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipeSearch.Models
{
    public class Unit : BaseModel
    {
        [MaxLength(50, ErrorMessage ="The name is too long.")]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [Display(Name = "Plural name")]
        public string PluralName { get; set; }
        public ICollection<ProductUnit> ProductUnits { get; set; }
        
    }
}
