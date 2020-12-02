using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RecipeSearch.DataAccess.Data;
using RecipeSearch.DataAccess.Repository.IRepository;
using RecipeSearch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RecipeSearch.DataAccess.Repository
{
    public class ListOfFavouritesRepository : BaseRepositoryAsync<ListOfFavourites>, IListOfFavouritesRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IDbConnection _dbConnection;
        public ListOfFavouritesRepository(ApplicationDbContext db,
                                        IConfiguration configuration) : base(db)
        {
            _db = db;
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task AddListOfFavourites(string name, string applicationUserId)
        {
            ListOfFavourites list = new ListOfFavourites() { Name = name, ApplicationUserId = applicationUserId };
            await AddAsync(list);

        }
        public async Task<IEnumerable<Recipe>> GetAllRecipesFromList(int listId)
        {
            //var sql = "SELECT Recipe FROM ListOfFavouritesRecipe WHERE ListOfFavouritesId = @ListOfFavouritesId" +
            //    " AS L INNER JOIN Recipes AS R ON L.RecipeId = R.Id";
            var sql = "SELECT Recipes.* FROM ListOfFavouritesRecipe INNER JOIN Recipes ON ListOfFavouritesRecipe.RecipeId = Recipes.Id WHERE ListOfFavouritesId = @ListOfFavouritesId";
            return await _dbConnection.QueryAsync<Recipe>(sql, new { @ListOfFavouritesId = listId });

        }
        public async Task<bool> IsLiked(string userId, int recipeId)
        {
            var list = await GetFirstOrDefaultAsync(x => x.ApplicationUserId == userId && x.Name == "All");
            if (list == null)
            {
                return false;
            }

            return await IsRecipeOnTheList(recipeId, list.Id);
        }
        public async Task<bool> IsRecipeOnTheList(int recipeId, int listId)
        {
            var liked = _db.Set<ListOfFavouritesRecipe>().Where(x => x.ListOfFavouritesId == listId && x.RecipeId == recipeId);
            if (liked == null || liked.Count() == 0)
            {
                return false;
            }

            return true;
        }
        public async Task AddRecipeToFavourites(string userId, int recipeId)
        {
            var list = await GetFirstOrDefaultAsync(x => x.ApplicationUserId == userId && x.Name == "All");
            if (list != null)
            {
                await AddRecipeToList(recipeId, list.Id);

            }
        }
        public async Task AddRecipeToList(int recipeId, int listId)
        {
            var listRecipe = new ListOfFavouritesRecipe() { ListOfFavouritesId = listId, RecipeId = recipeId };
            await _db.Set<ListOfFavouritesRecipe>().AddAsync(listRecipe);
        }
        public async Task RemoveRecipeFromFavourites(int recipeId, string userId)
        {
            var listIds = (await GetAllAsync(x => x.ApplicationUserId == userId)).Select(x => x.Id).ToArray();
            if (listIds.Count() != 0)
            {
                var itemsToRemove = _db.Set<ListOfFavouritesRecipe>().Where(x => listIds.Contains(x.ListOfFavouritesId) && x.RecipeId == recipeId).AsEnumerable();
                _db.Set<ListOfFavouritesRecipe>().RemoveRange(itemsToRemove);

            }
        }
        public void RemoveRecipeFromList(int recipeId, int listId)
        {
            var itemToRemove = _db.Set<ListOfFavouritesRecipe>().FirstOrDefault(x => x.RecipeId == recipeId && x.ListOfFavouritesId == listId);
            _db.Set<ListOfFavouritesRecipe>().Remove(itemToRemove);
        }
        public IEnumerable<SelectListItem> PopulateAvailableListDropdown(IEnumerable<ListOfFavourites> list)
        {
            var selectList = list.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
                Selected = i.Name == "All"
            });

            return selectList;
        }
        public async Task<IEnumerable<SelectListItem>> PopulateAvailableListDropdown(string userId)
        {
            var list = await GetAllAsync(x => x.ApplicationUserId == userId);
            var selectList = list.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
                Selected = i.Name == "All"
            });

            return selectList;
        }
    }
}
