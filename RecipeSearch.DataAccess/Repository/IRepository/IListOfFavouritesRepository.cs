using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeSearch.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSearch.DataAccess.Repository.IRepository
{
    public interface IListOfFavouritesRepository : IBaseRepositoryAsync<ListOfFavourites>
    {
        //void RemoveListOfFavourites(int id);
        Task AddListOfFavourites(string name, string applicationUserId);
        Task<IEnumerable<Recipe>> GetAllRecipesFromList(int listId);

        Task<bool> IsLiked(string userId, int recipeId);
        Task<bool> IsRecipeOnTheList(int recipeId, int listId);
        Task AddRecipeToFavourites(string userId, int recipeId);
        Task RemoveRecipeFromFavourites(int recipeId, string userId);
        Task AddRecipeToList(int recipeId, int listId);
        void RemoveRecipeFromList(int recipeId, int listId);
        IEnumerable<SelectListItem> PopulateAvailableListDropdown(IEnumerable<ListOfFavourites> list);
        Task<IEnumerable<SelectListItem>> PopulateAvailableListDropdown(string userId);
    }
}
