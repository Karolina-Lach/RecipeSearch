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
    public class PropertiesIntersectionTest
    {
        List<Recipe> Recipes = new List<Recipe>()
        {
            new Recipe()
            {
                Ingredients = new List<Ingredient>()
                {
                    //new Ingredient()
                    //{
                    //    Product = new Product() { Name = "apple", PluralName = "apples" }
                    //},
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
                Healthy = true
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
                Healthy = true
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
                Healthy = true
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
                Healthy = true
            }
        };
        
        List<Product> Products = new List<Product>() { new Product() { Name = "apple", PluralName = "apples" },
                                                            new Product(){Name="orange", PluralName = "oranges"} };

        [TestMethod]
        public void HasAllPropertiesRecipeExpression_Test()
        {
            string[] properties = new string[] { "NoDairy", "Vegan", "Healthy" };
            var result = Recipes.AsQueryable().Where(PropertiesIntersection.HasAllPropertiesRecipe(properties)).AsEnumerable();

            Assert.AreEqual(result.Count(), 3);
        }

        [TestMethod]
        public void HasAnyPropertiesRecipeExpression_Test()
        {
            string[] properties = new string[] { "NoDairy", "Vegan","Vegetarian","Healthy","NoGluten"};
            var result = Recipes.AsQueryable().Where(PropertiesIntersection.HasAnyPropertiesRecipe(properties)).AsEnumerable();

            Assert.AreEqual(result.Count(), 4);
        }

        
        [TestMethod]
        public void NumberOfCommonProperties_Test()
        {
            string[] properties = new string[] { "NoDairy", "Vegan", "Vegetarian", "Healthy", "NoGluten" };
            Recipe recipe = new Recipe()
            {
                Ingredients = new List<Ingredient>()
                {
                    //new Ingredient()
                    //{
                    //    Product = new Product() { Name = "apple", PluralName = "apples" }
                    //},
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
                Vegan = true,
                Vegetarian = false,
                Healthy = true
            };

            var result = PropertiesIntersection.NumberOfCommonProperties(recipe, properties);
            Assert.AreEqual(result, 3);
        }

        [TestMethod]
        public void HasAnyMealTypesRecipe_Test()
        {
            string[] properties = new string[] { "dinner"};
            Recipe recipe = new Recipe()
            {
                Ingredients = new List<Ingredient>()
                {
                    //new Ingredient()
                    //{
                    //    Product = new Product() { Name = "apple", PluralName = "apples" }
                    //},
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
                Vegan = true,
                Vegetarian = false,
                Healthy = true
            };

            var result = PropertiesIntersection.HasAnyMealTypesRecipe(recipe, properties);
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void HasAnyCuisinesRecipe_Test()
        {
            string[] properties = new string[] { "american" };
            Recipe recipe = new Recipe()
            {
                Ingredients = new List<Ingredient>()
                {
                    //new Ingredient()
                    //{
                    //    Product = new Product() { Name = "apple", PluralName = "apples" }
                    //},
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
                Vegan = true,
                Vegetarian = false,
                Healthy = true
            };

            var result = PropertiesIntersection.HasAnyCuisinesRecipe(recipe, properties);
            Assert.AreEqual(result, true);
        }
    }
}
