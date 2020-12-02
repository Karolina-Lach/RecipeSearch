using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeSearch.Models;
using RecipeSearch.Models.ViewModels;
using RecipeSearch.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RecipesTest
{
    [TestClass]
    public class IngredientIntersectionTests
    {
        
        
        [TestMethod]
        public void NumberOfCommonProducts_HasCommonTest()
        {
            List<Product> products = new List<Product>() { new Product() { Name = "apple", PluralName = "apples" },
                                                            new Product(){Name="orange", PluralName = "oranges"} };

            List<Product> products1 = new List<Product>() { new Product() { Name = "apple", PluralName = "apples" } };

            var result = ProductIntersection.NumberOfCommonProducts(products1, products);

            Assert.AreEqual(result, 1);
        }
        [TestMethod]
        public void NumberOfCommonProducts_HasNoCommonTest()
        {
            List<Product> products = new List<Product>() { new Product() { Name = "apple", PluralName = "apples" },
                                                            new Product(){Name="orange", PluralName = "oranges"} };

            List<Product> products1 = new List<Product>() { new Product() { Name = "onion", PluralName = "onions" } };

            var result = ProductIntersection.NumberOfCommonProducts(products1, products);

            Assert.AreEqual(result, 0);
        }

    }
}
