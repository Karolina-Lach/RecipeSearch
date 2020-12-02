using RecipeSearch.Models;
using RecipeSearch.Models.Comparers;
using RecipeSearch.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeSearch.Search
{
    public static class IngredientIntersection
    {
        public static IEnumerable<Ingredient> CommonIngredientsBasedOnProduct(IEnumerable<Ingredient> ingredientsInRecipe, IEnumerable<Ingredient> availableIngredients)
        {
            return ingredientsInRecipe.Intersect(availableIngredients, new IngredientProductComparer());
        }
    }
}
