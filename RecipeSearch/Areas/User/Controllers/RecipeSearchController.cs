using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecipeSearch.DataAccess.Repository.IRepository;
using RecipeSearch.Models;
using RecipeSearch.Models.ViewModels;
//using RecipeSearch.Queries;
using RecipeSearch.Search;
using RecipeSearch.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RecipeSearch.Areas.User.Controllers
{
    [Area("User")]

    public class RecipeSearchController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public RecipeSearchController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString(SD.SearchCriteriaSession) != null)
            {
                if (HttpContext.Session.GetString(SD.SearchResultSession) != null)
                {
                    SearchCriteria searchCriteria = JsonConvert.DeserializeObject<SearchCriteria>(HttpContext.Session.GetString(SD.SearchCriteriaSession));
                    FoundRecipesSession foundRecipes = JsonConvert.DeserializeObject<FoundRecipesSession>(HttpContext.Session.GetString(SD.SearchResultSession));
                    
                    ViewBag.Result = new FoundRecipesVM(
                        PaginatedList<FoundRecipe>.Create(foundRecipes.FoundRecipes.AsQueryable(), 1, 20),
                        PaginatedList<FoundRecipe>.Create(foundRecipes.Leftovers.AsQueryable(), 1, 20));

                    searchCriteria.AvailableProducts = (await _unitOfWork.Product.GetAllAsync(null, true)).Select(x => x.Name).ToList();
                    searchCriteria.AvailableCuisines = (await _unitOfWork.Recipe.AvailableCuisines()).ToList();
                    searchCriteria.AvailableMealTypes = (await _unitOfWork.Recipe.AvailableMeals()).ToList();
                    return View(searchCriteria);
                }
                else
                {
                    HttpContext.Session.Remove(SD.SearchCriteriaSession);
                    SearchCriteria searchCriteria = new SearchCriteria()
                    {
                        AvailableProducts = (await _unitOfWork.Product.GetAllAsync(null, true)).Select(x => x.Name).ToList(),
                        AvailableCuisines = (await _unitOfWork.Recipe.AvailableCuisines()).ToList(),
                        AvailableMealTypes = (await _unitOfWork.Recipe.AvailableMeals()).ToList()
                    };
                    return View(searchCriteria);
                }
            }
            else
            {
                if (HttpContext.Session.GetString(SD.SearchResultSession) != null)
                {
                    HttpContext.Session.Remove(SD.SearchResultSession);
                }
                SearchCriteria searchCriteria = new SearchCriteria()
                {
                    AvailableProducts = (await _unitOfWork.Product.GetAllAsync(null, true)).Select(x => x.Name).ToList(),
                    AvailableCuisines = (await _unitOfWork.Recipe.AvailableCuisines()).ToList(),
                    AvailableMealTypes = (await _unitOfWork.Recipe.AvailableMeals()).ToList()
                };
                return View(searchCriteria);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Clear()
        {
            if (HttpContext.Session.GetString(SD.SearchCriteriaSession) != null)
            {
                HttpContext.Session.Remove(SD.SearchCriteriaSession);
            }
            if (HttpContext.Session.GetString(SD.SearchResultSession) != null)
            {
                HttpContext.Session.Remove(SD.SearchResultSession);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var recipe = await _unitOfWork.Recipe.GetFirstOrDefaultWithIngredientsAndSteps(x => x.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            SearchCriteria criteria = null;
            if (HttpContext.Session.GetString(SD.SearchCriteriaSession) != null)
            {
                criteria = JsonConvert.DeserializeObject<SearchCriteria>(HttpContext.Session.GetString(SD.SearchCriteriaSession));
            }
            
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                ViewBag.IsLiked = false;
            }
            else
            {
                ViewBag.IsLiked = await _unitOfWork.ListOfFavourites.IsLiked(claim.Value, id);
            }

            var missingIngredients = criteria != null ? ProductIntersection.MissingProducts(recipe, criteria.Products
                                    .Select(x => new Product(x)).ToList()) : null;
            var missingProps = criteria != null ? PropertiesIntersection.GetMissingProperties(criteria.Properties.ToList(), recipe) : null;
            FoundRecipeDetailsVM found = new FoundRecipeDetailsVM()
            {
                Recipe = recipe,
                MissingIngredients = missingIngredients.Select(x => { return x.Name; }),
                MissingProperties = missingProps
            };
            return View(found);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Search(string products, string properties, bool matchAll, string cuisines, string meals, int time)   //[FromBody] SearchCriteria criteria)                ///[FromBody]string[] selectedMeals)
        {
            if (HttpContext.Session.GetString(SD.SearchCriteriaSession) != null)
            {
                HttpContext.Session.Remove(SD.SearchCriteriaSession);
            }
            if (HttpContext.Session.GetString(SD.SearchResultSession) != null)
            {
                HttpContext.Session.Remove(SD.SearchResultSession);
            }

            var recipes = (await _unitOfWork.Recipe.GetAllAsync(null, true));
            recipes = recipes.Select(i => _unitOfWork.Recipe.PopulateIngredientsInRecipe(i, true));
            //var foundRecipes = Search(recipes, new SearchCriteria(products, properties, cuisines, meals, matchAll, time));
            
            var criteria = new SearchCriteria(products, properties, cuisines, meals, matchAll, time);
            var foundRecipes = FindRecipes.Search(recipes, criteria);
            foreach (var recipe in foundRecipes.FoundRecipes)
            {
                recipe.ProductsDisplay = criteria.Products.Length > 0;
                recipe.PropertiesDisplay = criteria.Properties.Length > 0;
                recipe.MealDisplay = criteria.SelectedCuisines.Length > 0;
                recipe.CuisineDisplay = criteria.SelectedMeals.Length > 0;
                recipe.TimeDisplay = criteria.PrepTime > 0;
            }
            foreach (var recipe in foundRecipes.Leftovers)
            {
                recipe.ProductsDisplay = criteria.Products.Length > 0;
                recipe.PropertiesDisplay = criteria.Properties.Length > 0;
                recipe.MealDisplay = criteria.SelectedCuisines.Length > 0;
                recipe.CuisineDisplay = criteria.SelectedMeals.Length > 0;
                recipe.TimeDisplay = criteria.PrepTime > 0;
            }
            var serializeObject1 = JsonConvert.SerializeObject(foundRecipes);
            HttpContext.Session.SetString(SD.SearchResultSession, serializeObject1);

            var serializeObject2 = JsonConvert.SerializeObject(criteria);
            HttpContext.Session.SetString(SD.SearchCriteriaSession, serializeObject2);
            
            return PartialView("_FoundRecipes", new FoundRecipesVM(
                        PaginatedList<FoundRecipe>.Create(foundRecipes.FoundRecipes.AsQueryable(), 1, 8),
                        PaginatedList<FoundRecipe>.Create(foundRecipes.Leftovers.AsQueryable(), 1, 8)));
        }

        [HttpGet]
        public IActionResult ChangeResultPage(int? pageNumber)
        {
            if (HttpContext.Session.GetString(SD.SearchCriteriaSession) != null)
            {
                if (HttpContext.Session.GetString(SD.SearchResultSession) != null)
                {
                    FoundRecipesSession foundRecipes = JsonConvert.DeserializeObject<FoundRecipesSession>(HttpContext.Session.GetString(SD.SearchResultSession));
                    var foundRecipesVM = new FoundRecipesVM(
                        PaginatedList<FoundRecipe>.Create(foundRecipes.FoundRecipes.AsQueryable(), pageNumber ?? 1, 8),
                        PaginatedList<FoundRecipe>.Create(foundRecipes.Leftovers.AsQueryable(), pageNumber ?? 1, 8));
                    return PartialView("_FoundRecipes", foundRecipesVM);
                }
                else
                {
                    HttpContext.Session.Remove(SD.SearchCriteriaSession);

                }
            }
            else
            {
                if (HttpContext.Session.GetString(SD.SearchResultSession) != null)
                {
                    HttpContext.Session.Remove(SD.SearchResultSession);
                }
            }
            return null;
        }
        [HttpGet]
        public async Task<JsonResult> GetProducts()
        {
            List<string> products = (await _unitOfWork.Product.GetAllAsync(null, true)).Select(x => x.Name).ToList();
            return Json(products);
        }
        public async Task<IActionResult> AddToGroceryList(int recipeId, string[] products)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return Json(new { success = false, message = "Error while adding ingredients to your grocery list. You need to be logged in to do that." });
                //return PartialView("_StatusMessage", "Error while adding ingredients to grocery list. You need to be logged in to do that.");
            }
            var recipe = await _unitOfWork.Recipe.GetAsync(recipeId);
            if (recipe == null)
            {
                return Json(new { success = false, message = "Error while adding ingredients to your grocery list" });
                //return PartialView("_StatusMessage", "Error while adding ingredients to your grocery list");
            }
            _unitOfWork.Recipe.PopulateIngredientsInRecipe(recipe);
            _unitOfWork.GroceryList.AddGroceries(recipe, products, claim.Value);
            try
            {
                _unitOfWork.Save();
            }
            catch(Exception)
            {
                return Json(new { success = false, message = "Error while adding ingredients to your grocery list" });
                //return PartialView("_StatusMessage", "Error while adding ingredients to your grocery list.");
            }

            return Json(new { success = true, message = "Ingredients were added to your grocery list." });
            //return PartialView("_StatusMessage", "Ingredients were added to your grocery list.");
        }



        /*********************************/
        /* private FoundRecipesSession Search(IEnumerable<Recipe> recipes, SearchCriteria criteria)
         {
             List<Product> products = criteria.Products == null ? null : criteria.Products.Select(x => new Product(x)).ToList();
             string[] properties = criteria.Properties;
             string[] mealTypes = criteria.SelectedMeals;
             string[] cuisines = criteria.SelectedCuisines;
             int prepTime = criteria.PrepTime;


             var foundRecipes = FindRecipes.GetFoundRecipes(recipes, products, properties, cuisines, mealTypes, prepTime).ToList();

             IEnumerable<FoundRecipe> result;
             if (criteria.AreAllPropertiesMatched)
             {
                 result = FindRecipes.FoundRecipesWithAllProperties(foundRecipes, properties, mealTypes, cuisines, prepTime, products.Count);
             }
             else
             {
                 int numberOfProducts = products == null ? 0 : products.Count;
                 result = FindRecipes.FoundRecipesWithAnyProperties(foundRecipes, properties, mealTypes, cuisines, prepTime, numberOfProducts);
             }

             var leftovers = FindRecipes.GetLeftoverRecipes(result, foundRecipes);
             FoundRecipesSession foundRecipesSession = new FoundRecipesSession(result, leftovers);

             var serializeObject1 = JsonConvert.SerializeObject(foundRecipesSession);
             HttpContext.Session.SetString(SD.SearchResultSession, serializeObject1);

             var serializeObject2 = JsonConvert.SerializeObject(criteria);
             HttpContext.Session.SetString(SD.SearchCriteriaSession, serializeObject2);
             return foundRecipesSession;
         }
    */
    }
}
