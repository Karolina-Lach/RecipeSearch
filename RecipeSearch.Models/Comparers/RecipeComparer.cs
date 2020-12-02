using RecipeSearch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeSearch.Models.Comparers
{
    public class RecipeComparer : IEqualityComparer<Recipe>
    {
        public bool Equals([AllowNull] Recipe x, [AllowNull] Recipe y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.Id == y.Id;                           
        }

        public int GetHashCode([DisallowNull] Recipe  recipe)
        {
            if (recipe is null) return 0;

            IngredientComparer ingredientComparer = new IngredientComparer();
            int hashRecipe = recipe.Id.GetHashCode() + 43;

            return hashRecipe;
        }
    }
}
