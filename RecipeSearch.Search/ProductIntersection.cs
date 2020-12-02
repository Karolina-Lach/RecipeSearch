using RecipeSearch.Models;
using RecipeSearch.Models.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeSearch.Search
{
    public class ProductIntersection
    {
        public static IEnumerable<Product> ListOfCommonProducts(IEnumerable<Product> list1, IEnumerable<Product> list2)
        {
            return list1.Intersect(list2, new ProductNameComparer());
        }
        public static int NumberOfCommonProducts(List<Product> list1, List<Product> list2)
        {
            return ListOfCommonProducts(list1, list2).Count();
        }
        public static int NumberOfCommonProducts(Recipe recipe1, Recipe recipe2)
        {
            List<Product> list1 = recipe1.GetListOfProducts().ToList();
            List<Product> list2 = recipe2.GetListOfProducts().ToList();

            return NumberOfCommonProducts(list1, list2);
        }

        public static int NumberOfCommonProducts(Recipe recipe, List<Product> list)
        {
            List<Product> list1 = recipe.GetListOfProducts().ToList();

            return NumberOfCommonProducts(list1, list);
        }

        public static IEnumerable<Product> MissingProducts(Recipe recipe, IEnumerable<Product> products)
        {
            var result = recipe.GetListOfProducts().Except(products, new ProductNameComparer());
            return result;
        }
    }
}
