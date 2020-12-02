using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeSearch.Models;
using RecipeSearch.Models.Comparers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipesTest
{
    [TestClass]
    public class RecipeComparerTest
    {
        private RecipeComparer recipeComparer = new RecipeComparer();
        [TestMethod]
        public void Equals_TheSame()
        {
            var t1 = new Recipe()
            {
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient()
                    {
                        Product = new Product() { Name = "apple", PluralName = "apples" }
                    }
                },
                CuisinesString = "american,british",
                MealTypesString = "dinner,lunch",
                NoDairy = true,
                NoGluten = false,
                Vegan = true,
                Vegetarian = false,
                Healthy = true,
                PrepTime = 10,
                Id = 1
            };
            var t2 = new Recipe()
            {
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient()
                    {
                        Product = new Product() { Name = "apple", PluralName = "apples" }
                    }
                },
                CuisinesString = "american,british",
                MealTypesString = "dinner,lunch",
                NoDairy = true,
                NoGluten = false,
                Vegan = true,
                Vegetarian = false,
                Healthy = true,
                PrepTime = 10,
                Id = 1
            };

            var result = recipeComparer.Equals(t1, t2);

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void Equals_NotTheSameProduct()
        {
            var t1 = new Recipe()
            {
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient()
                    {
                        Product = new Product() { Name = "apple", PluralName = "apples" }
                    }
                },
                CuisinesString = "american,british",
                MealTypesString = "dinner,lunch",
                NoDairy = true,
                NoGluten = false,
                Vegan = true,
                Vegetarian = false,
                Healthy = true,
                PrepTime = 10,
                Id = 1
            };
            var t2 = new Recipe()
            {
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient()
                    {
                        Product = new Product() { Name = "apple", PluralName = "apples" }
                    },
                    new Ingredient()
                    {
                        Product = new Product() { Name = "onion", PluralName = "onions" }
                    }
                },
                CuisinesString = "american,british",
                MealTypesString = "dinner,lunch",
                NoDairy = true,
                NoGluten = false,
                Vegan = true,
                Vegetarian = false,
                Healthy = true,
                PrepTime = 10,
                Id = 2
            };

            var result = recipeComparer.Equals(t1, t2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetHashCode_NotTheSameProduct()
        {
            var t1 = new Recipe()
            {
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient()
                    {
                        Product = new Product() { Name = "apple", PluralName = "apples" }
                    }
                },
                CuisinesString = "american,british",
                MealTypesString = "dinner,lunch",
                NoDairy = true,
                NoGluten = false,
                Vegan = true,
                Vegetarian = false,
                Healthy = true,
                PrepTime = 10,
                Id = 1
            };
            var t2 = new Recipe()
            {
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient()
                    {
                        Product = new Product() { Name = "apple", PluralName = "apples" }
                    },
                    new Ingredient()
                    {
                        Product = new Product() { Name = "onion", PluralName = "onions" }
                    }
                },
                CuisinesString = "american,british",
                MealTypesString = "dinner,lunch",
                NoDairy = true,
                NoGluten = false,
                Vegan = true,
                Vegetarian = false,
                Healthy = true,
                PrepTime = 10,
                Id = 2
            };

            var result = recipeComparer.GetHashCode(t1);
            var result2 = recipeComparer.GetHashCode(t2);

            Assert.AreNotEqual(result, result2);
        }
        //[TestMethod]
        //public void GetHashCode_NotTheSameRecipe()
        //{
        //    var t1 = new Ingredient()
        //    {
        //        Product = new Product() { Name = "apple", PluralName = "apples" },
        //        UnitId = 1,
        //        Measurement = 1,
        //        RecipeId = 2
        //    };
        //    var t2 = new Ingredient()
        //    {
        //        Product = new Product() { Name = "apple", PluralName = "apples" },
        //        UnitId = 1,
        //        Measurement = 1,
        //        RecipeId = 1
        //    };


        //    var result = ingredientComparer.GetHashCode(t1);
        //    var result2 = ingredientComparer.GetHashCode(t2);

        //    Assert.AreNotEqual(result, result2);
        //}
        //[TestMethod]
        //public void GetHashCode_TheSameName()
        //{
        //    var t1 = new Ingredient()
        //    {
        //        Product = new Product() { Name = "apple", PluralName = "apples" },
        //        UnitId = 1,
        //        Measurement = 1,
        //        RecipeId = 1
        //    };
        //    var t2 = new Ingredient()
        //    {
        //        Product = new Product() { Name = "apple", PluralName = "apples" },
        //        UnitId = 1,
        //        Measurement = 1,
        //        RecipeId = 1
        //    };


        //    var result = ingredientComparer.GetHashCode(t1);
        //    var result2 = ingredientComparer.GetHashCode(t2);


        //    Assert.AreEqual(result2, result);
        //}
        //[TestMethod]
        //public void Equals_TheSameReference()
        //{
        //    var t1 = new Ingredient()
        //    {
        //        Product = new Product() { Name = "apple", PluralName = "apples" },
        //        UnitId = 1,
        //        Measurement = 1,
        //        RecipeId = 1
        //    };
        //    var t2 = t1;
        //    var result = ingredientComparer.Equals(t1, t2);
        //    Assert.IsTrue(result);
        //}
    }
}
