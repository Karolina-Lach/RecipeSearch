using RecipeSearch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeSearch.Models.Comparers
{
    public class IngredientComparer : IEqualityComparer<Ingredient>
    {
        public bool Equals([AllowNull] Ingredient x, [AllowNull] Ingredient y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            ProductNameComparer productNameComparer = new ProductNameComparer();
            return (productNameComparer.Equals(x.Product, y.Product)
                                && (x.RecipeId == y.RecipeId)
                                && (x.Id == x.Id));
        }

        public int GetHashCode([DisallowNull] Ingredient ingredient)
        {
            if (ingredient is null) return 0;

            ProductNameComparer productNameComparer = new ProductNameComparer();
            int hashIngredient = productNameComparer.GetHashCode(ingredient.Product) + ingredient.RecipeId + ingredient.Id;

            return hashIngredient;
        }
    }
}
