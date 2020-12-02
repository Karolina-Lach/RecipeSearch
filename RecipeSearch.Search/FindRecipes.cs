using RecipeSearch.Models;
using RecipeSearch.Models.Comparers;
using RecipeSearch.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;



namespace RecipeSearch.Search
{
    public class FindRecipes
    {
        
        public static FoundRecipesSession Search(IEnumerable<Recipe> recipes, SearchCriteria criteria)
        {
            List<Product> products = criteria.Products == null ? null : criteria.Products.Select(x => new Product(x)).ToList();
            string[] properties = criteria.Properties;
            string[] mealTypes = criteria.SelectedMeals;
            string[] cuisines = criteria.SelectedCuisines;
            int prepTime = criteria.PrepTime;

            var foundRecipes = GetFoundRecipes(recipes, products, properties, cuisines, mealTypes, prepTime).ToList();
            IEnumerable<FoundRecipe> result;
            if (criteria.AreAllPropertiesMatched)
            {
                result = FoundRecipesWithAllProperties(foundRecipes, properties, mealTypes, cuisines, prepTime, products.Count);
            }
            else
            {
                int numberOfProducts = products == null ? 0 : products.Count;
                result = FoundRecipesWithAnyProperties(foundRecipes, properties, mealTypes, cuisines, prepTime, numberOfProducts);
            }

            var leftovers = GetLeftoverRecipes(result, foundRecipes);
            FoundRecipesSession foundRecipesSession = new FoundRecipesSession(result, leftovers);

            return foundRecipesSession;
        }
        public static IEnumerable<FoundRecipe> GetFoundRecipes(IEnumerable<Recipe> recipes, List<Product> list, string[] properties,
                                                    string[] cuisines, string[] mealTypes, int prepTime)
        {
            var foundRecipes = recipes.Select(x =>
                       new FoundRecipe
                       {
                           Recipe = x,
                           NumberOfProductsMatched = list == null ? 0 : ProductIntersection.NumberOfCommonProducts(x, list),
                           NumberOfPropertiesMatched = PropertiesIntersection.NumberOfCommonProperties(x, properties),
                           HasCuisine = PropertiesIntersection.HasAnyCuisinesRecipe(x, cuisines),
                           HasMealType = PropertiesIntersection.HasAnyMealTypesRecipe(x, mealTypes),
                           IsInTime = x.PrepTime <= prepTime ? true : false
                       });

            return foundRecipes.ToList();
        }

        public static IEnumerable<FoundRecipe> FoundRecipesWithAllProperties(IEnumerable<FoundRecipe> foundRecipes, string[] properties,
                                                                    string[] mealTypes, string[] cuisines, int prepTime, int requiredProducts)
        {
            //IEnumerable<FoundRecipe> found = foundRecipes.AsQueryable().Where(x =>
            //           ((requiredProducts > 0) ? (x.NumberOfIntersection > 0) : true) &&
            //           x.NumberOfPropertiesMatched == properties.Length).Where(x =>
            //           ((mealTypes != null || mealTypes.Length != 0) ? x.HasMealType : true) ||
            //           ((cuisines != null || cuisines.Length != 0) ? x.HasCuisine : true) ||
            //           ((prepTime != 0) ? x.Recipe.PrepTime < prepTime : true));

            IEnumerable<FoundRecipe> found = foundRecipes.AsQueryable().Where(x =>
                       ((requiredProducts > 0) ? (x.NumberOfProductsMatched > 0) : true) &&
                       x.NumberOfPropertiesMatched == properties.Length).Where(HasAny(properties, mealTypes, cuisines, prepTime));
            
                
                found = OrderByIngredients(found);
           
            return found;
        }

        public static Expression<Func<FoundRecipe, bool>> HasAny(string[] properties, string[] mealTypes, string[] cuisines, int prepTime)
        {
            var parameterExpression = Expression.Parameter(typeof(FoundRecipe), "foundRecipe");
            var constant = Expression.Constant(true);

            BinaryExpression expression = null;
            
            if (mealTypes != null)
            {
                if(mealTypes.Length != 0)
                {
                    var property = Expression.Property(parameterExpression, "HasMealType");
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
            if (cuisines != null)
            {
                if (cuisines.Length != 0)
                {
                    var property = Expression.Property(parameterExpression, "HasCuisine");
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
            if (properties != null)
            {
                if (properties.Length != 0)
                {
                    constant = Expression.Constant(0);
                    var property = Expression.Property(parameterExpression, "NumberOfPropertiesMatched");
                    if (expression == null)
                    {
                        expression = Expression.GreaterThan(property, constant);
                    }
                    else
                    {
                        expression = Expression.Or(expression, Expression.GreaterThan(property, constant));
                    }
                }
            }
            if (prepTime != 0)
            {
                Expression property = parameterExpression;
                foreach (var member in "Recipe.PrepTime".Split('.'))
                {
                    property = Expression.PropertyOrField(property, member);
                    
                }

                constant = Expression.Constant(prepTime);
                if (expression == null)
                {
                    expression = Expression.LessThanOrEqual(property, constant);
                }
                else
                {
                    expression = Expression.Or(expression, Expression.LessThanOrEqual(property, constant));
                }
            }
            if(expression == null)
            {
                return null;
            }
            return Expression.Lambda<Func<FoundRecipe, bool>>(expression, parameterExpression);
        }

        public static IEnumerable<FoundRecipe> FoundRecipesWithAnyProperties(IEnumerable<FoundRecipe> foundRecipes, string[] properties,
                                                                    string[] mealTypes, string[] cuisines, int prepTime, int requiredProducts)
        {
            IEnumerable<FoundRecipe> found = foundRecipes.AsQueryable().Where(x =>
                       ((requiredProducts > 0) ? (x.NumberOfProductsMatched > 0) : true));
            var expression = HasAny(properties, mealTypes, cuisines, prepTime);
            if(expression !=null)
            {
                found = found.AsQueryable().Where(expression);
            }
            
                
                //.Where( x=>
                //       (((properties != null || properties.Length != 0) ? x.NumberOfPropertiesMatched > 0 : false) ||
                //       ((mealTypes != null || mealTypes.Length != 0) ? x.HasMealType : false) ||
                //       ((cuisines != null || cuisines.Length != 0) ? x.HasCuisine : false) ||
                //       ((prepTime != 0) ? x.Recipe.PrepTime < prepTime : false)));
            
            found = OrderByIngredients(found);
            //var found = foundRecipes.AsQueryable();
            //if (properties != null)
            //{
            //    if (properties.Length != 0)
            //    {
            //        found = found.Where(x => x.NumberOfPropertiesMatched > 0 && x.HasMealType && x.HasCuisine && x.Recipe.PrepTime < prepTime);
            //    }
            //}
            //if (mealTypes != null)
            //{
            //    if (mealTypes.Length != 0)
            //    {
            //        found = found.Where(x => x.HasMealType);
            //    }
            //}
            //if (cuisines != null)
            //{
            //    if(cuisines.Length != 0)
            //    {
            //        found = found.Where(x => x.HasCuisine);
            //    }
            //}
            //if (prepTime != 0) { found = found.Where(x => x.Recipe.PrepTime < prepTime); }

            return found.AsEnumerable();
        }

        public static IEnumerable<FoundRecipe> OrderByIngredients(IEnumerable<FoundRecipe> foundRecipes)
        {
            foundRecipes = foundRecipes.OrderByDescending(x => x.NumberOfProductsMatched)
                .ThenByDescending(x => x.NumberOfPropertiesMatched).ToList();

            return foundRecipes;
        }
        public static IEnumerable<FoundRecipe> GetLeftoverRecipes(IEnumerable<FoundRecipe> matchingRecipes, IEnumerable<FoundRecipe> foundRecipes)
        {
            var result = foundRecipes.Except(matchingRecipes, new FoundRecipeComparer());
            result = OrderByIngredients(result);
            return result;
        }
    }
}
