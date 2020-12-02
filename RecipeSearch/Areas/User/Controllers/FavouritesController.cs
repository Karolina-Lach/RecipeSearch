using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeSearch.DataAccess.Repository.IRepository;
using RecipeSearch.Models;
using RecipeSearch.Models.ViewModels;
using RecipeSearch.Utility;

namespace RecipeSearch.Areas.User.Controllers
{
    
    [Area("User")]
    [Authorize]
    public class FavouritesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public FavouritesController(IUnitOfWork unitOfWork)
        {
            //_listRepo = listRepo;
            //_recipeRepo = recipeRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var list = await _unitOfWork.ListOfFavourites.GetAllAsync(x => x.ApplicationUserId == claim.Value);

            var favourites = new FavouritesVM()
            {
                AvailableLists = list,
                FavouriteLists = _unitOfWork.ListOfFavourites.PopulateAvailableListDropdown(list)
            };
            return View(favourites);
        } 
        [HttpPost]
        public async Task<bool> AddToFavourites(int id)
        {
            if (id == 0)
            {
                return false;
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return false;
            }

            bool isLiked = await _unitOfWork.ListOfFavourites.IsLiked(claim.Value, id);
            if (isLiked)
            {
                await _unitOfWork.ListOfFavourites.RemoveRecipeFromFavourites(id, claim.Value);
                _unitOfWork.Save();
                return false;
            }
            else
            {
                await _unitOfWork.ListOfFavourites.AddRecipeToFavourites(claim.Value, id);
                _unitOfWork.Save();
                return true;
            }
        }
        
        [HttpPost]
        public async Task<ListOfFavourites> AddNewListJson(string name)
        {
            if(name == null || name == "" || name == "All")
            {
                return null;
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return null;
            }

            ListOfFavourites newList = new ListOfFavourites { Name = name, ApplicationUserId = claim.Value };
            await _unitOfWork.ListOfFavourites.AddAsync(newList);
            _unitOfWork.Save();
            newList.ApplicationUserId = null;
            return newList;
        }
        
        [HttpDelete]
        public async Task<IActionResult> RemoveListJson(int id)
        {
            if(id == 0)
            {
                return Json(new { success = false, message = "Error while deleting a list" });
            }
            var list = await _unitOfWork.ListOfFavourites.GetAsync(id);
            if (list == null)
            {
                return Json(new { success = false, message = "Error while deleting a list" });
            }
            else
            {
                await _unitOfWork.ListOfFavourites.RemoveAsync(id);
                try
                {
                    _unitOfWork.Save();
                }
                catch (Exception)
                {
                    return Json(new { success = false, message = "Error while deleting a list" });
                }

                return Json(new { success = true, message = "List deleted." });
            }
        }


        /************* PARTIAL VIEWS *************/

        [HttpPost]
        public async Task<ActionResult> RemoveFromList(int recipeId, int listId)
        {
            if (recipeId == 0 || listId == 0)
            {
                return PartialView("_StatusMessage", "Error while removing the recipe. Invalid list.");
            }
            var list = await _unitOfWork.ListOfFavourites.GetAsync(listId);

            if (list == null)
            {
                return PartialView("_StatusMessage", "Error while removing the recipe. Invalid list.");
            }
            else
            {
                if (list.Name == "All")
                {
                    var claimsIdentity = (ClaimsIdentity)User.Identity;
                    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                    await _unitOfWork.ListOfFavourites.RemoveRecipeFromFavourites(recipeId, claim.Value);
                    _unitOfWork.Save();
                    return PartialView("_StatusMessage", "Recipe removed from all of your lists.");
                }
                else
                {
                    _unitOfWork.ListOfFavourites.RemoveRecipeFromList(recipeId, listId);
                    _unitOfWork.Save();
                    return PartialView("_StatusMessage", "Recipe removed from list " + list.Name);
                }
            }
        }
        [HttpGet]
        public async Task<ActionResult> SelectFavouritesList(int selectedList, int currentList, int? pageNumber)
        {
            if (selectedList != 0)
            {
                pageNumber = 1;
            }
            else
            {
                selectedList = currentList;
            }
            ViewData["CurrentList"] = selectedList;

            var list = (await _unitOfWork.ListOfFavourites.GetAllRecipesFromList(selectedList)).Select(x=> new RecipeHeaderVM(x));
            int pageSize = 20;
            return PartialView("_RecipesListFav", PaginatedList<RecipeHeaderVM>.Create(list.AsQueryable(), pageNumber ?? 1, pageSize));
        }
        [HttpGet]
        public async Task<IActionResult> ListOfFavouritesListsView()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            //var list = _unitOfWork.ListOfFavourites.GetAllListsForUser(claim.Value);
            var list = await _unitOfWork.ListOfFavourites.GetAllAsync(x => x.ApplicationUserId == claim.Value);
            return PartialView("_ListOfFavouritesLists", list);
        }
        [HttpGet]
        public async Task<IActionResult> FavouritesDropDownView()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var favourites = new FavouritesVM()
            {
                FavouriteLists = await _unitOfWork.ListOfFavourites.PopulateAvailableListDropdown(claim.Value)
            };

            return PartialView("_FavouriteListDropdown", favourites);
        }
        [HttpPost]
        public async Task<IActionResult> AddRecipeToList(int recipeId, int listId)
        {
            var list = await _unitOfWork.ListOfFavourites.GetAsync(listId);
            var recipe = await _unitOfWork.Recipe.GetAsync(recipeId);
            if (list == null || recipe == null)
            {
                string message = "Error while adding recipe.";
                return PartialView("_StatusMessage", message);
            }
            else
            {
                if (!await _unitOfWork.ListOfFavourites.IsRecipeOnTheList(recipeId, listId))
                {
                    await _unitOfWork.ListOfFavourites.AddRecipeToList(recipeId, listId);
                    _unitOfWork.Save();
                    string message = "Recipe added to list " + list.Name + ".";
                    return PartialView("_StatusMessage", message);
                }
                else
                {
                    return PartialView("_StatusMessage", null);
                }
            }

        }
        [HttpGet]
        public IActionResult StatusMessage(string message)
        {
            return PartialView("_StatusMessage", message);
        }
    }
}
