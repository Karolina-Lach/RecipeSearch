using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.Models.ViewModels
{
    public class FoundRecipeDetailsVM
    {
        public IEnumerable<string> MissingIngredients { get; set; }
        public IEnumerable<string> MissingProperties { get; set; }
        public Recipe Recipe { get; set; }
    }
}
