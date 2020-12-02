using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RecipesTest
{
    [TestClass]
    public class RecipeModelTest
    {
        [TestMethod]
        public void GetProductList_Test()
        {
            // Arrange
            Recipe recipe = new Recipe(){ Ingredients = new List<Ingredient>() };
            recipe.Ingredients.Add(new Ingredient()
            {
                Product = new Product() { Name = "apple", PluralName = "apples" }
            });

            List<Product> products = new List<Product>() { 
                new Product() { Name = "apple", PluralName = "apples" }};

            // Act
            List<Product> products1 = recipe.GetListOfProducts().ToList();

            // Assert
            Assert.AreEqual(products1.Count, products.Count);
        }

        [TestMethod]
        public void AddCuisine_StartStringNull_Test()
        {
            // Arrange
            string startString = null;
            string toAdd = "american";
            string expected = "american";
            Recipe recipe = new Recipe() { CuisinesString = startString };

            // Act
            recipe.AddCuisine(toAdd);

            // Assert
            Assert.AreEqual(recipe.CuisinesString, expected);
        }
        [TestMethod]
        public void AddCuisine_StartString_Test()
        {
            // Arrange
            string startString = "british";
            string toAdd = "american";
            string expected = "british,american";
            Recipe recipe = new Recipe() { CuisinesString = startString };

            // Act
            recipe.AddCuisine(toAdd);

            // Assert
            Assert.AreEqual(recipe.CuisinesString, expected);
        }
        [TestMethod]
        public void AddCuisine_NothingToAdd_Test()
        {
            // Arrange
            string startString = "british";
            string toAdd = "nothing";
            string expected = "british";
            Recipe recipe = new Recipe() { CuisinesString = startString };

            // Act
            recipe.AddCuisine(toAdd);

            // Assert
            Assert.AreEqual(recipe.CuisinesString, expected);
        }
        
    }
}
