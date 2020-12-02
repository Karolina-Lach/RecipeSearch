using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.Models.ViewModels
{
    public class FoundRecipesSession
    {
        public FoundRecipesSession(IEnumerable<FoundRecipe> foundRecipes, IEnumerable<FoundRecipe> leftovers)
        {
            FoundRecipes = foundRecipes;
            Leftovers = leftovers;
        }
        public IEnumerable<FoundRecipe> FoundRecipes { get; set; }
        public IEnumerable<FoundRecipe> Leftovers { get; set; }
    }
}
