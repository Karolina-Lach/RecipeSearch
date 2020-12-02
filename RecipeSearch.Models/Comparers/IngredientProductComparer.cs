using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace RecipeSearch.Models.Comparers
{
    public class IngredientProductComparer : IEqualityComparer<Ingredient>
    {
        public bool Equals([AllowNull] Ingredient x, [AllowNull] Ingredient y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            ProductNameComparer productNameComparer = new ProductNameComparer();
            return productNameComparer.Equals(x.Product, y.Product);
        }

        public int GetHashCode([DisallowNull] Ingredient ingredient)
        {
            if (ingredient is null) return 0;

            ProductNameComparer productNameComparer = new ProductNameComparer();
            int hashIngredient = productNameComparer.GetHashCode(ingredient.Product);

            return hashIngredient;
        }
    }
}
