
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeSearch.Models.Comparers
{

    public class ProductNameComparer : IEqualityComparer<Product>
    {
        public bool Equals([AllowNull] Product x, [AllowNull] Product y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] Product product)
        {
            if (Object.ReferenceEquals(product, null)) return 0;
            int hashProductName = product.Name == null ? 0 : product.Name.GetHashCode();

            return hashProductName;
        }
    }

}
