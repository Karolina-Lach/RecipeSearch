using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeSearch.Models;
using RecipeSearch.Models.ViewModels;
using RecipeSearch.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipesTest
{
    [TestClass]
    public class RecipeFindTest
    {
        List<Recipe> Recipes = new List<Recipe>()
        {
            new Recipe()
            {
                Ingredients = new List<Ingredient>()
                {
                     new Ingredient()
                    {
                        Product = new Product(){Name="orange", PluralName = "oranges"}
                    },
                     new Ingredient()
                    {
                        Product = new Product(){Name="onion", PluralName = "onions"}
                    },
                      new Ingredient()
                    {
                        Product = new Product(){Name="tomato", PluralName = "tomatoes"}
                    },
                },
                CuisinesString = "american,british",
                MealTypesString = "dinner",
                NoDairy = true,
                NoGluten = false,
                Vegan =true,
                Vegetarian = false,
                Healthy = true,
                PrepTime = 10,
                Id = 1
            },
            new Recipe()
            {
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient()
                    {
                        Product = new Product() { Name = "apple", PluralName = "apples" }
                    },
                     new Ingredient()
                    {
                        Product = new Product(){Name="orange", PluralName = "oranges"}
                    },
                     new Ingredient()
                    {
                        Product = new Product(){Name="potato", PluralName = "potatoes"}
                    },
                      new Ingredient()
                    {
                        Product = new Product(){Name="bread", PluralName = "breads"}
                    },
                },
                CuisinesString = "american,british",
                MealTypesString = "dinner,lunch",
                NoDairy = true,
                NoGluten = false,
                Vegan =true,
                Vegetarian = false,
                Healthy = true,
                PrepTime = 10,
                Id = 2
            },
            new Recipe()
            {
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient()
                    {
                        Product = new Product() { Name = "apple", PluralName = "apples" }
                    },
                     new Ingredient()
                    {
                        Product = new Product(){Name="orange", PluralName = "oranges"}
                    },
                     new Ingredient()
                    {
                        Product = new Product(){Name="potato", PluralName = "potatoes"}
                    },
                      new Ingredient()
                    {
                        Product = new Product(){Name="bread", PluralName = "breads"}
                    },
                },
                CuisinesString = "american,british",
                MealTypesString = "dinner,lunch",
                NoDairy = false,
                NoGluten = false,
                Vegan =true,
                Vegetarian = false,
                Healthy = true,
                PrepTime = 10,
                Id = 3
            },
            new Recipe()
            {
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient()
                    {
                        Product = new Product() { Name = "pita", PluralName = "pitas" }
                    },
                      new Ingredient()
                    {
                        Product = new Product(){Name="bread", PluralName = "breads"}
                    },
                },
                CuisinesString = "greek,british",
                MealTypesString = "lunch,breakfast",
                NoDairy = true,
                NoGluten = true,
                Vegan =true,
                Vegetarian = true,
                Healthy = true,
                PrepTime = 10,
                Id = 4
            }
        };

        List<Product> Products = new List<Product>() { new Product() { Name = "apple", PluralName = "apples" },
                                                            new Product(){Name="orange", PluralName = "oranges"} };

        [TestMethod]
        public void FindRecipesWithAllProperties_Test()
        {
            
            List<Product> products = new List<Product>() { new Product() { Name = "apple", PluralName = "apples" },
                                                            new Product(){Name="orange", PluralName = "oranges"} };
            string[] properties = new string[] { "NoDairy" };
            string[] mealTypes = new string[] { "dinner" };
            string[] cuisines = new string[] { "american" };
            int prepTime = 120;

            var foundRecipes = FindRecipes.GetFoundRecipes(Recipes, products, properties, cuisines, mealTypes, prepTime);
            var result = FindRecipes.FoundRecipesWithAllProperties(foundRecipes, properties,mealTypes, cuisines, prepTime, products.Count);
            var leftovers = FindRecipes.GetLeftoverRecipes(result, foundRecipes);

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(2, leftovers.Count());
        }
        [TestMethod]
        public void FindRecipesWithAllProperties_Case2_Test()
        {

            // Assert
            //List<Product> products = new List<Product>() { new Product() { Name = "apple", PluralName = "apples" },
            //                                                new Product(){Name="orange", PluralName = "oranges"} };
            string[] products = new string[] { "apple", "orange" };
            string[] properties = new string[] { "NoDairy" };
            string[] mealTypes = new string[] { };
            string[] cuisines = new string[] { };
            int prepTime = 0;
            SearchCriteria criteria = new SearchCriteria()
            {
                Products = products,
                Properties = properties,
                SelectedCuisines = cuisines,
                SelectedMeals = mealTypes,
                PrepTime = prepTime,
                AreAllPropertiesMatched = true
            };

            // Act
            FoundRecipesSession result = FindRecipes.Search(Recipes, criteria);
            // Assert
            Assert.AreEqual(2, result.FoundRecipes.Count());
            Assert.AreEqual(2, result.Leftovers.Count());
        }
        [TestMethod]
        public void FindRecipesWithAnyProperties_Case1_Test()
        {
            //List<Product> products = new List<Product>() {  new Product() { Name = "bread" },
            //                                                new Product(){Name="garlic" },
            //                                                new Product(){Name="olive oil" }};
            string[] products = new string[] { "bread", "garlic", "olive oil" };
            string[] properties = new string[] { "NoGluten" };
            string[] mealTypes = new string[] { "appetizer" };
            string[] cuisines = new string[] { "british" };
            int prepTime = 120;
            SearchCriteria criteria = new SearchCriteria()
            {
                Products = products,
                Properties = properties,
                SelectedCuisines = cuisines,
                SelectedMeals = mealTypes,
                PrepTime = prepTime,
                AreAllPropertiesMatched = false
            };

            var result = FindRecipes.Search(Recipes, criteria);

            Assert.AreEqual(3, result.FoundRecipes.Count());
            Assert.AreEqual(1, result.Leftovers.Count());
        }
        [TestMethod]
        public void FindRecipesWithAnyProperties_Case2_Test()
        {
            // Arrange   
            string[] products = new string[] { "orange", "apple" };
            string[] properties = new string[] { "NoDairy", "Vegan" };
            string[] mealTypes = new string[] { };
            string[] cuisines = new string[] { };
            int prepTime = 120;
            SearchCriteria criteria = new SearchCriteria()
            {
                Products = products,
                Properties = properties,
                SelectedCuisines = cuisines,
                SelectedMeals = mealTypes,
                PrepTime = prepTime,
                AreAllPropertiesMatched = false
            };
            // Act
            var result = FindRecipes.Search(Recipes, criteria);

            Assert.AreEqual(3, result.FoundRecipes.Count());
            Assert.AreEqual(1, result.Leftovers.Count());
        }

        [TestMethod]
        public void Query_Test()
        {
            
            List<Product> products = new List<Product>() { new Product() { Name = "apple", PluralName = "apples" },
                                                            new Product(){Name="orange", PluralName = "oranges"} };
            string[] properties = new string[] { };
            string[] mealTypes = new string[] { "dinner"};
            string[] cuisines = new string[] { "greek" };
            int prepTime = 100;
            int requiredProducts = 2;

            var foundRecipes = FindRecipes.GetFoundRecipes(Recipes, products, properties, cuisines, mealTypes, prepTime);
            var found = FindRecipes.FoundRecipesWithAllProperties(foundRecipes, properties, mealTypes, cuisines, prepTime, requiredProducts);

        }


    }
}
