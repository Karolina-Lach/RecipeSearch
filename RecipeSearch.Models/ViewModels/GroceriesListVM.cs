using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.Models.ViewModels
{
    public class GroceriesListVM
    {
        public Dictionary<string, List<Ingredient>> Groceries { get; set; }
    }
}
