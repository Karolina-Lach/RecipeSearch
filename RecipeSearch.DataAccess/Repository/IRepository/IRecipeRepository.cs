using Newtonsoft.Json.Linq;
using RecipeSearch.Models;
using RecipeSearch.Utility;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSearch.DataAccess.Repository.IRepository
{
    public interface IRecipeRepository : IBaseRepositoryAsync<Recipe>
    {
        new Task AddAsync(Recipe recipe);
        new Task AddAsync(JToken entity);
        Task AddAsync(JToken entity, IEnumerable<Ingredient> ingredients, IEnumerable<Step> steps);
        Task Update(Recipe recipe);
        Task<Recipe> GetFirstOrDefaultWithIngredientsAndSteps(Expression<Func<Recipe, bool>> filter = null, string includeProperties = null);
        Recipe PopulateIngredientsInRecipe(Recipe recipe, bool asNoTracking = false);
        IEnumerable<Ingredient> GetListOfIngredients(int recipeId, bool asNoTracking = false);
        Task<IEnumerable<Cuisine>> AvailableCuisines();
        Task<IEnumerable<MealType>> AvailableMeals();
    }
}
