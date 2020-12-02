using RecipeSearch.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.Models.ViewModels
{
    public class FoundRecipesVM
    {
        public FoundRecipesVM(PaginatedList<FoundRecipe> foundRecipes, PaginatedList<FoundRecipe> leftovers)
        {
            FoundRecipes = foundRecipes;
            Leftovers = leftovers;
        }
        public PaginatedList<FoundRecipe> FoundRecipes { get; set; }
        public PaginatedList<FoundRecipe> Leftovers { get; set; }
    }
}
