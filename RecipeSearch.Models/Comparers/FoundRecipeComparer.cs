using RecipeSearch.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeSearch.Models.Comparers
{
    
    public class FoundRecipeComparer : IEqualityComparer<FoundRecipe>
    {
        
        public bool Equals([AllowNull] FoundRecipe x, [AllowNull] FoundRecipe y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            RecipeComparer recipeComparer = new RecipeComparer();
            return recipeComparer.Equals(x.Recipe, y.Recipe);
        }

        public int GetHashCode([DisallowNull] FoundRecipe obj)
        {
            if (obj is null) return 0;
            RecipeComparer recipeComparer = new RecipeComparer();

            int hashRecipe = recipeComparer.GetHashCode(obj.Recipe);

            return hashRecipe;
        }
    }
}
