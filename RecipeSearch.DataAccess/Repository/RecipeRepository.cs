using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RecipeSearch.DataAccess.Data;
using RecipeSearch.DataAccess.Repository.IRepository;
using RecipeSearch.Models;
using RecipeSearch.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RecipeSearch.DataAccess.Repository
{
    public class RecipeRepository : BaseRepositoryAsync<Recipe>, IRecipeRepository
    {
        private readonly ApplicationDbContext _db;

        public RecipeRepository(ApplicationDbContext db):base(db) ///, IConfiguration configuration, IIngredientRepository ingredientRepo, IStepRepository stepRepo) : base(db)
        {
            _db = db;
            //_dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            //_ingredientRepo = ingredientRepo;
            //_stepRepo = stepRepo;
        }

        new public async Task AddAsync(Recipe recipe)
        {
            //using (var transaction = new TransactionScope())
            //{
            //    if(recipe.ImageUrl ==null || recipe.ImageUrl == "")
            //    {
            //        recipe.ImageUrl = @"\images\recipes\no-image.jpg";
            //    }
            //    var sql = "INSERT INTO Recipes (Name, Summary, PrepTime, Servings, Source, ImageUrl, NoDairy, NoGluten, Vegan, Vegetarian, Healthy, CuisinesString, MealTypesString) " +
            //        "VALUES (@Name, @Summary, @PrepTime, @Servings, @Source, @ImageUrl, @NoDairy, @NoGluten, @Vegan, @Vegetarian, @Healthy, @CuisinesString, @MealTypesString);"
            //           + "SELECT CAST(SCOPE_IDENTITY() as int); ";
            //    var id = _dbConnection.Query<int>(sql, recipe).Single();

            //    recipe.Id = id;

            //    recipe.Ingredients.Select(c => { c.RecipeId = id; return c; }).ToList();
            //    var sqlIngredientsWithUnit = "INSERT INTO Ingredients(Name, ProductId, UnitId, RecipeId, Measurement) VALUES(@Name, @ProductId, @UnitId, @RecipeId, @Measurement );" +
            //                "SELECT CAST(SCOPE_IDENTITY() as int); ";
            //    _dbConnection.Execute(sqlIngredientsWithUnit, recipe.Ingredients.Where(i => i.UnitId != 0));

            //    var sqlIngredientsWithoutUnit = "INSERT INTO Ingredients(Name, ProductId, RecipeId, Measurement) VALUES(@Name, @ProductId, @RecipeId, @Measurement );" +
            //                "SELECT CAST(SCOPE_IDENTITY() as int); ";
            //    _dbConnection.Execute(sqlIngredientsWithoutUnit, recipe.Ingredients.Where(i => i.UnitId == 0));

            //    recipe.Steps.Select(c => { c.RecipeId = id; return c; }).ToList();
            //    var sqlSteps = "INSERT INTO Steps(Name, Number, RecipeId) VALUES(@Name, @Number, @RecipeId);" +
            //                "SELECT CAST(SCOPE_IDENTITY() as int); ";
            //    _dbConnection.Execute(sqlSteps, recipe.Steps);
            //    transaction.Complete();

            //}
            if (recipe.ImageUrl == null || recipe.ImageUrl == "")
            {
                recipe.ImageUrl = @"\images\recipes\no-image.jpg";
            }
            recipe.Ingredients = recipe.Ingredients.Select(x => { x.Unit = null; x.Product = null; return x; }).ToList();
            await _db.AddAsync(recipe);
        }
        new public async Task AddAsync(JToken entity)
        {
            Recipe recipe = entity.ToObject<Recipe>();
            await AddAsync(recipe);
        }

        public async Task AddAsync(JToken entity, IEnumerable<Ingredient> ingredients, IEnumerable<Step> steps)
        {
            Recipe recipe = entity.ToObject<Recipe>();
            recipe.Ingredients = ingredients.ToList();
            recipe.Steps = steps.ToList();
            await AddAsync(recipe);
        }
        public async Task Update(Recipe recipe)
        {
            //using (var transaction = new TransactionScope())
            //{
            //    if (recipe.ImageUrl == null || recipe.ImageUrl == "")
            //    {
            //        recipe.ImageUrl = @"\images\recipes\no-image.jpg";
            //    }

            //    var sql = "UPDATE Recipes SET Name = @Name, Summary = @Summary, PrepTime = @PrepTime, Servings =@Servings, " +
            //        "Source = @Source, ImageUrl = @ImageUrl, NoDairy = @NoDairy, NoGluten = @NoGluten, Vegan = @Vegan, Vegetarian = @Vegetarian, Healthy = @Healthy, CuisinesString = @CuisinesString, MealTypesString = @MealTypesString " +
            //        "WHERE Id = @Id";
            //    _dbConnection.Execute(sql, recipe);


            //    var ingredients = _ingredientRepo.GetAll(i => i.RecipeId == recipe.Id);
            //    _ingredientRepo.RemoveRange(ingredients);

            //    recipe.Ingredients.Select(c => { c.RecipeId = recipe.Id; return c; }).ToList();
            //    var sqlIngredientsWithUnit = "INSERT INTO Ingredients(Name, ProductId, UnitId, RecipeId, Measurement) VALUES(@Name, @ProductId, @UnitId, @RecipeId, @Measurement );"; 
            //    _dbConnection.Execute(sqlIngredientsWithUnit, recipe.Ingredients.Where(i => i.UnitId != 0));

            //    var sqlIngredientsWithoutUnit = "INSERT INTO Ingredients(Name, ProductId, RecipeId, Measurement) VALUES(@Name, @ProductId, @RecipeId, @Measurement );";
            //    _dbConnection.Execute(sqlIngredientsWithoutUnit, recipe.Ingredients.Where(i => i.UnitId == 0));

            //    var steps = _stepRepo.GetAll(i => i.RecipeId == recipe.Id);
            //    _stepRepo.RemoveRange(steps);

            //    recipe.Steps.Select(c => { c.RecipeId = recipe.Id; return c; }).ToList();
            //    var sqlSteps = "INSERT INTO Steps(Name, Number, RecipeId) VALUES(@Name, @Number, @RecipeId);";
            //    _dbConnection.Execute(sqlSteps, recipe.Steps);
            //    transaction.Complete();

            //}
            var objFromDb = await _db.Recipes.FirstOrDefaultAsync(x => x.Id == recipe.Id);
            if(objFromDb != null)
            {
                var oldIngredients = _db.Ingredients.Where(x => x.RecipeId == recipe.Id);
                _db.Ingredients.RemoveRange(oldIngredients);

                var oldSteps = _db.Steps.Where(x => x.RecipeId == recipe.Id);
                _db.Steps.RemoveRange(oldSteps);

                if (recipe.ImageUrl != null)
                {
                    objFromDb.ImageUrl = recipe.ImageUrl;
                }

                objFromDb.Name = recipe.Name;
                objFromDb.Summary = recipe.Summary;
                objFromDb.Servings = recipe.Servings;
                objFromDb.PrepTime = recipe.PrepTime;
                objFromDb.NoDairy = recipe.NoDairy;
                objFromDb.NoGluten = recipe.NoGluten;
                objFromDb.Vegan = recipe.Vegan;
                objFromDb.Vegetarian = recipe.Vegetarian;
                objFromDb.Healthy = recipe.Healthy;
                objFromDb.MealTypesString = recipe.MealTypesString;
                objFromDb.CuisinesString = recipe.CuisinesString;
                objFromDb.Ingredients= recipe.Ingredients.Select(x => { x.Unit = null; x.Product = null; return x; }).ToList();
                objFromDb.Steps = recipe.Steps;
            }

        }

        public async Task<Recipe> GetFirstOrDefaultWithIngredientsAndSteps(Expression<Func<Recipe, bool>> filter = null, string includeProperties = null)
        {
            Recipe recipe = await GetFirstOrDefaultAsync(filter, true, includeProperties);

            //recipe.Ingredients = _ingredientRepo.GetAll(i => i.RecipeId == recipe.Id, includeProperties:"Product,Unit").ToList();
            //recipe.Steps = _stepRepo.GetAll(i => i.RecipeId == recipe.Id).ToList();
            recipe.Ingredients = _db.Ingredients
                                        .Where(x => x.RecipeId == recipe.Id)
                                        .Include(ingredient => ingredient.Product)
                                        .Include(ingredient => ingredient.Unit)
                                        .ToList();
            recipe.Steps = _db.Steps
                              .Where(x => x.RecipeId == recipe.Id)
                              .ToList();
            return recipe;
        }
        public Recipe PopulateIngredientsInRecipe(Recipe recipe, bool asNoTracking = false)
        {
            recipe.Ingredients = GetListOfIngredients(recipe.Id, asNoTracking).ToList();
            return recipe;
        }
        public IEnumerable<Ingredient> GetListOfIngredients(int recipeId, bool asNoTracking = false)
        {
            if (!asNoTracking)
            {
                return _db.Ingredients
                          .Where(x => x.RecipeId == recipeId)
                          .Include(ingredient => ingredient.Product)
                          .Include(ingredient => ingredient.Unit);
            }
            else
            {
                return _db.Ingredients
                          .Where(x => x.RecipeId == recipeId)
                          .Include(ingredient => ingredient.Product)
                          .Include(ingredient => ingredient.Unit).AsNoTracking();
            }
        }
        public async Task<IEnumerable<Cuisine>> AvailableCuisines()
        {
            List<Cuisine> availableCuisines = new List<Cuisine>();
            foreach (Cuisine cuisine in Enum.GetValues(typeof(Cuisine)))
            {
                var recipe = await GetFirstOrDefaultAsync(x => x.CuisinesString.Contains(cuisine.ToString()));
                if (recipe != null)
                {
                    availableCuisines.Add(cuisine);
                }
            }

            return availableCuisines;
        }
        public async Task<IEnumerable<MealType>> AvailableMeals()
        {
            List<MealType> availableMeals = new List<MealType>();
            foreach (MealType meal in Enum.GetValues(typeof(MealType)))
            {
                var recipe = await GetFirstOrDefaultAsync(x => x.MealTypesString.Contains(meal.ToString()));
                if (recipe != null)
                {
                    availableMeals.Add(meal);
                }
            }

            return availableMeals;
        }

    }
}
