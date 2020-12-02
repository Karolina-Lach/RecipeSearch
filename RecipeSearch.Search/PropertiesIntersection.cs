using RecipeSearch.Models;
using RecipeSearch.Models.ViewModels;
using RecipeSearch.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RecipeSearch.Search
{
    public static class PropertiesIntersection
    {
        public static Expression<Func<Recipe, bool>> HasAllPropertiesRecipe(string[] properties)
        {
            if (properties == null || properties.Length == 0)
            {
                return null;
            }
            var parameterExpression = Expression.Parameter(typeof(Recipe), "recipe");
            var constant = Expression.Constant(true);

            BinaryExpression expression = null;
            foreach (var item in properties)
            {
                if (typeof(Recipe).GetProperty(item) != null)
                {
                    var property = Expression.Property(parameterExpression, item);
                    if (expression == null)
                    {
                        expression = Expression.Equal(property, constant);
                    }
                    else
                    {
                        expression = Expression.And(expression, Expression.Equal(property, constant));
                    }
                }
            }

            var lambda = Expression.Lambda<Func<Recipe, bool>>(expression, parameterExpression);

            return lambda;
        }
        public static Expression<Func<Recipe, bool>> HasAnyPropertiesRecipe(string[] properties)
        {
            if (properties == null || properties.Length == 0)
            {
                return null;
            }
            var parameterExpression = Expression.Parameter(typeof(Recipe), "recipe");
            var constant = Expression.Constant(true);

            BinaryExpression expression = null;
            foreach (var item in properties)
            {
                if (typeof(Recipe).GetProperty(item) != null)
                {
                    var property = Expression.Property(parameterExpression, item);
                    if (expression == null)
                    {
                        expression = Expression.Equal(property, constant);
                    }
                    else
                    {
                        expression = Expression.Or(expression, Expression.Equal(property, constant));
                    }
                }
            }

            var lambda = Expression.Lambda<Func<Recipe, bool>>(expression, parameterExpression);

            return lambda;
        }
        public static Expression<Func<Recipe, bool>> HasAnyMealTypesRecipe(string[] mealTypes)
        {

            var methodInfo = typeof(Recipe).GetMethod("ContainsMealType", new Type[] { typeof(string) });
            var parameter = Expression.Parameter(typeof(Recipe), "recipe");
           

            //Expression method = Expression.Call(parameter, methodInfo, constant);

            Expression expression = null;
            foreach (var item in mealTypes)
            {
                    var constant = Expression.Constant(item);
                    if (expression == null)
                    {
                        expression = Expression.Call(parameter, methodInfo, constant);
                    }
                    else
                    {
                        expression = Expression.Or(expression, Expression.Call(parameter, methodInfo, constant));
                    }
                
            }

            var lambda = Expression.Lambda<Func<Recipe, bool>>(expression, parameter);
            return lambda;
        }
        public static Expression<Func<Recipe, bool>> HasAnyCuisinesRecipe(string[] cuisineTypes)
        {

            var methodInfo = typeof(Recipe).GetMethod("ContainsCuisine", new Type[] { typeof(string) });
            var parameter = Expression.Parameter(typeof(Recipe), "recipe");


            //Expression method = Expression.Call(parameter, methodInfo, constant);

            Expression expression = null;
            foreach (var item in cuisineTypes)
            {
                var constant = Expression.Constant(item);
                if (expression == null)
                {
                    expression = Expression.Call(parameter, methodInfo, constant);
                }
                else
                {
                    expression = Expression.Or(expression, Expression.Call(parameter, methodInfo, constant));
                }

            }

            var lambda = Expression.Lambda<Func<Recipe, bool>>(expression, parameter);
            return lambda;
        }


        public static Expression<Func<FoundRecipe, bool>> HasAllPropertiesFoundRecipe(string[] properties)
        {
            if (properties == null || properties.Length == 0)
            {
                return null;
            }
            var parameterExpression = Expression.Parameter(typeof(FoundRecipe), "foundRecipe");
            var constant = Expression.Constant(true);

            BinaryExpression expression = null;
            foreach (var item in properties)
            {
                if (typeof(Recipe).GetProperty(item) != null)
                {
                    Expression recipeProperty = parameterExpression;
                    foreach (var member in ("Recipe."+item).Split('.'))
                    {
                        recipeProperty = Expression.PropertyOrField(recipeProperty, member);
                    }
                    
                    if (expression == null)
                    {
                        expression = Expression.Equal(recipeProperty, constant);
                    }
                    else
                    {
                        expression = Expression.And(expression, Expression.Equal(recipeProperty, constant));
                    }
                }
            }

            var lambda = Expression.Lambda<Func<FoundRecipe, bool>>(expression, parameterExpression);

            return lambda;
        }
        public static Expression<Func<FoundRecipe, bool>> HasAnyPropertiesFoundRecipe(string[] properties)
        {
            if (properties == null || properties.Length == 0)
            {
                return null;
            }
            var parameterExpression = Expression.Parameter(typeof(FoundRecipe), "foundRecipe");
            var constant = Expression.Constant(true);

            BinaryExpression expression = null;
            foreach (var item in properties)
            {
                if (typeof(Recipe).GetProperty(item) != null)
                {
                    Expression recipeProperty = parameterExpression;
                    foreach (var member in ("Recipe." + item).Split('.'))
                    {
                        recipeProperty = Expression.PropertyOrField(recipeProperty, member);
                    }
                    if (expression == null)
                    {
                        expression = Expression.Equal(recipeProperty, constant);
                    }
                    else
                    {
                        expression = Expression.Or(expression, Expression.Equal(recipeProperty, constant));
                    }
                }
            }

            var lambda = Expression.Lambda<Func<FoundRecipe, bool>>(expression, parameterExpression);

            return lambda;
        }
        public static Expression<Func<FoundRecipe, bool>> HasAnyMealTypesFoundRecipe(string[] mealTypes)
        {

            var methodInfo = typeof(Recipe).GetMethod("ContainsMealType", new Type[] { typeof(string) });
            var parameterExpression = Expression.Parameter(typeof(FoundRecipe), "foundRecipe");
           
            Expression recipeProperty = Expression.Property(parameterExpression, "Recipe");
            Expression expression = null;
            foreach (var item in mealTypes)
            {
                var constant = Expression.Constant(item);
                if (expression == null)
                {
                    expression = Expression.Call(recipeProperty, methodInfo, constant);
                }
                else
                {
                    expression = Expression.Or(expression, Expression.Call(recipeProperty, methodInfo, constant));
                }

            }

            var lambda = Expression.Lambda<Func<FoundRecipe, bool>>(expression, parameterExpression);
            return lambda;
        }
        public static Expression<Func<FoundRecipe, bool>> HasAnyCuisinesFoundRecipe(string[] cuisineTypes)
        {
            if(cuisineTypes.Length == 0 || cuisineTypes == null)
            {
                return null;
            }
            var methodInfo = typeof(Recipe).GetMethod("ContainsCuisine", new Type[] { typeof(string) });
            var parameterExpression = Expression.Parameter(typeof(FoundRecipe), "foundRecipe");
            Expression recipeProperty = Expression.Property(parameterExpression, "Recipe");
            Expression expression = null;
            foreach (var item in cuisineTypes)
            {
                var constant = Expression.Constant(item);
                if (expression == null)
                {
                    expression = Expression.Call(recipeProperty, methodInfo, constant);
                }
                else
                {
                    expression = Expression.Or(expression, Expression.Call(recipeProperty, methodInfo, constant));
                }

            }

            var lambda = Expression.Lambda<Func<FoundRecipe, bool>>(expression, parameterExpression);
            return lambda;
        }

        public static int NumberOfCommonProperties(Recipe recipe, string[] properties)
        {
            if(properties == null)
            {
                return 0;
            }
            int common = 0;
            foreach(var item in properties)
            {
                switch(item)
                {
                    case "NoDairy":
                        if (recipe.NoDairy) common++;
                        break;
                    case "Vegan":
                        if (recipe.Vegan) common++;
                        break;
                    case "Vegetarian":
                        if (recipe.Vegetarian) common++;
                        break;
                    case "NoGluten":
                        if (recipe.NoGluten) common++;
                        break;
                    case "Healthy":
                        if (recipe.Healthy) common++;
                        break;
                }
            }

            return common;
        }
        public static bool HasAnyMealTypesRecipe(Recipe recipe, string[] mealTypes)
        {
            if(mealTypes != null)
            {
                foreach (var item in mealTypes)
                {
                    if (recipe.ContainsMealType(item)) return true;
                }
            }
            return false;
        }
        public static bool HasAnyCuisinesRecipe(Recipe recipe, string[] cuisineTypes)
        {
            if(cuisineTypes != null)
            {
                foreach (var item in cuisineTypes)
                {
                    if (recipe.ContainsCuisine(item)) return true;
                }
            }

            return false;
        }

        public static IEnumerable<FoundRecipe> GetNumberOfMatchedProperties(IEnumerable<FoundRecipe> recipes, string[] properties)
        {
            List<FoundRecipe> foundRecipes = recipes.Select(x =>
                        new FoundRecipe { Recipe = x.Recipe, NumberOfPropertiesMatched = NumberOfCommonProperties(x.Recipe, properties) }).ToList();

            return foundRecipes;
        }
        public static IEnumerable<FoundRecipe> GetNumberOfMatchedProperties(IEnumerable<Recipe> recipes, string[] properties)
        {
            List<FoundRecipe> foundRecipes = recipes.Select(x =>
                        new FoundRecipe { Recipe = x, NumberOfPropertiesMatched = NumberOfCommonProperties(x, properties) }).ToList();

            return foundRecipes;
        }

        public static IEnumerable<string> GetMissingProperties(IEnumerable<string> reqiredProperties, Recipe recipe)
        {
            var missingProps = new List<string>();
            foreach (var item in reqiredProperties)
            {
                switch (item)
                {
                    case "NoDairy":
                        if (!recipe.NoDairy) missingProps.Add(item);
                        break;
                    case "Vegan":
                        if (!recipe.Vegan) missingProps.Add(item);
                        break;
                    case "Vegetarian":
                        if (!recipe.Vegetarian) missingProps.Add(item);
                        break;
                    case "NoGluten":
                        if (!recipe.NoGluten) missingProps.Add(item);
                        break;
                    case "Healthy":
                        if (!recipe.Healthy) missingProps.Add(item);
                        break;
                }
            }

            return missingProps;
        }

        // Methods for FoundRecipe
        public static bool HasAllPropertiesRecipe(FoundRecipe foundRecipe, string[] properties)
        {
            if (properties == null || properties.Length == 0)
            {
                return true;
            }

            if (foundRecipe.NumberOfPropertiesMatched == properties.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool HasAnyPropertiesRecipe(FoundRecipe foundRecipe, string[] properties)
        {
            if (properties == null || properties.Length == 0)
            {
                return true;
            }

            if (foundRecipe.NumberOfPropertiesMatched > 0)
            {
                return true;
            }
            else { return false; }
        }

    }
}
