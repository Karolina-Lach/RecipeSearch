using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RecipeSearch.DataAccess.Repository.IRepository;
using RecipeSearch.Models;
using RecipeSearch.Models.ViewModels;
using RecipeSearch.Utility;
using Microsoft.EntityFrameworkCore;

namespace RecipeSearch.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public RecipeController(IUnitOfWork unitOfWork,
            IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager)
        {
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var recipe = await _unitOfWork.Recipe.GetFirstOrDefaultWithIngredientsAndSteps(i => i.Id == id);
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
            return View(recipe);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJson(int? pageNumber)
        {
            int pageSize = 12;
            var recipeHeaders = await GetAll();
            return PartialView("_RecipesListRec", PaginatedList<RecipeHeaderVM>.Create(recipeHeaders.AsQueryable(), pageNumber ?? 1, pageSize));
        }

        private async Task<IEnumerable<RecipeHeaderVM>> GetAll()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return null;
            }
            var user = await _userManager.FindByIdAsync(claim.Value);

            IEnumerable<RecipeHeaderVM> recipeHeaders;
            if (User.IsInRole(SD.Role_Admin))
            {
                recipeHeaders = (await _unitOfWork.Recipe.GetAllAsync(null, true)).Select(x => new RecipeHeaderVM(x, false));
            }
            else
            {
                recipeHeaders = (await _unitOfWork.Recipe.GetAllAsync(x => x.Source == user.UserName, true)).Select(x => new RecipeHeaderVM(x));
            }

            return recipeHeaders;
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            if (HttpContext.Session.GetString(SD.CreateRecipeSession) != null)
            {
                HttpContext.Session.Remove(SD.CreateRecipeSession);
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return Redirect(@"/Identity/Account/AccessDenied");
            }
            var user = await _userManager.FindByIdAsync(claim.Value);

            Recipe recipe;

            if (id == null || id == 0)
            {
                recipe = new Recipe() { Source = user.UserName };
            }
            else
            {
                recipe = await _unitOfWork.Recipe.GetFirstOrDefaultWithIngredientsAndSteps(x => x.Id == id);
                if (recipe == null)
                {
                    return NotFound();
                }
                if (recipe.Source != user.UserName)
                {
                    return Redirect(@"/Identity/Account/AccessDenied");
                }

                IngredientUpsertVM ingredientVM = new IngredientUpsertVM() { Ingredients = recipe.Ingredients };
                var serializeObject = JsonConvert.SerializeObject(ingredientVM);
                HttpContext.Session.SetString(SD.CreateRecipeSession, serializeObject);

            }

            RecipeUpsertVM model = new RecipeUpsertVM()
            {
                ProductList = await _unitOfWork.Product.PopulateProductData(),
                UnitList = await _unitOfWork.Unit.PopulateUnitData(),
                Recipe = recipe
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(string[] selectedCuisines, string[] selectedMeals, string[] steps, [Bind("Recipe")] RecipeUpsertVM recipeVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return Redirect(@"/Identity/Account/AccessDenied");
            }
            await AddImage(recipeVM.Recipe);
            UpdateCuisines(selectedCuisines, recipeVM.Recipe);
            UpdateMeals(selectedMeals, recipeVM.Recipe);
            AddSteps(steps, recipeVM.Recipe);
            IngredientUpsertVM ingredients = JsonConvert.DeserializeObject<IngredientUpsertVM>(HttpContext.Session.GetString(SD.CreateRecipeSession));
            recipeVM.Recipe.Ingredients = ingredients.Ingredients;
            ViewBag.Ingredients = ingredients.Ingredients;
            if (recipeVM.Recipe.Id == 0)
            {
                await _unitOfWork.Recipe.AddAsync(recipeVM.Recipe);
            }
            else
            {
                await _unitOfWork.Recipe.Update(recipeVM.Recipe);
            }


            _unitOfWork.Save();


            HttpContext.Session.Remove(SD.CreateRecipeSession);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> AddIngredients(string measurement, int productId, int unitId, string comment)
        {
            Ingredient newIngredient = new Ingredient();

            if (!ValidateMeasurementInput(measurement))
            {
                return Json(new { success = false, message = "Measurement must be a number between 0.01 and 99999.99 with maximum 2 decimal digits." });
            }
            if (productId == 0)
            {
                return Json(new { success = false, message = "Product cannot be null! " });
            }
            Product productFromDb = await _unitOfWork.Product.GetAsync(productId);

            if (productFromDb == null)
            {
                return Json(new { success = false, message = "Invalid product." });
            }

            if (unitId == 0 && productFromDb.PluralName == null)
            {
                return Json(new { success = false, message = "Unit cannot be null." });
            }
            else
            {
                if (unitId != 0)
                {
                    Unit unitFromDb = await _unitOfWork.Unit.GetAsync(unitId);
                    if (unitFromDb == null)
                    {
                        return Json(new { success = false, message = "Invalid unit." });
                    }

                    newIngredient.UnitId = unitId;
                    newIngredient.Unit = unitFromDb;
                }

            }

            newIngredient.ProductId = productId;
            newIngredient.Product = productFromDb;
            measurement = measurement.Contains('.') ? measurement.Replace('.', ',') : measurement;
            decimal measurementDecimal = decimal.Parse(measurement);
            newIngredient.Measurement = measurementDecimal;
            if (comment != null)
            {
                newIngredient.Comment = comment;
            }
            IngredientUpsertVM ingredientVM;
            if (HttpContext.Session.GetString(SD.CreateRecipeSession) != null)
            {
                ingredientVM = JsonConvert.DeserializeObject<IngredientUpsertVM>(HttpContext.Session.GetString(SD.CreateRecipeSession));
                if (ingredientVM.Ingredients == null)
                {
                    ingredientVM.Ingredients = new List<Ingredient>();
                }

                ingredientVM.Ingredients.Add(newIngredient);

            }
            else
            {
                ingredientVM = new IngredientUpsertVM() { Ingredients = new List<Ingredient>() { newIngredient } };

            }
            var serializeObject = JsonConvert.SerializeObject(ingredientVM);
            HttpContext.Session.SetString(SD.CreateRecipeSession, serializeObject);
            ViewBag.Ingredients = ingredientVM.Ingredients;
            return Json(new { success = true, message = newIngredient.Display() });
        }

        [HttpPost]
        public IActionResult RemoveIngredients(string measurement, int productId, int unitId, string comment)
        {
            measurement = measurement.Contains('.') ? measurement.Replace('.', ',') : measurement;
            decimal measurementDecimal = Decimal.Parse(measurement);

            if (HttpContext.Session.GetString(SD.CreateRecipeSession) != null)
            {
                IngredientUpsertVM ingredientVM = JsonConvert.DeserializeObject<IngredientUpsertVM>(HttpContext.Session.GetString(SD.CreateRecipeSession));
                if (ingredientVM.Ingredients == null)
                {
                    return Json(new { success = false, message = "Unexpected error!" });
                }


                if (unitId == 0)
                {
                    ingredientVM.Ingredients.RemoveAll(x => x.ProductId == productId && x.Comment == comment && x.Measurement == measurementDecimal && x.UnitId == null);
                }
                else
                {

                    ingredientVM.Ingredients.RemoveAll(x => x.ProductId == productId && x.Comment == comment && x.Measurement == measurementDecimal && x.UnitId == unitId);
                }


                var serializeObject = JsonConvert.SerializeObject(ingredientVM);
                HttpContext.Session.SetString(SD.CreateRecipeSession, serializeObject);
                return Json(new { success = true, message = "" });
            }

            return Json(new { success = false, message = "Unexpected error!" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _unitOfWork.Recipe.GetAsync(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Oops, something went wrong. Please, try again later." });
            }
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                if (!imagePath.Contains("no-image"))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            await _unitOfWork.Recipe.RemoveAsync(id);
            try
            {
                _unitOfWork.Save();
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Oops, something went wrong. Please, try again later." });
            }

            return Json(new { success = true, message = "Recipe deleted successfully." });

        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(string name, string pluralName)
        {
            if (name == null || name == "")
            {
                return Json(new { success = false, message = "Name cannot be empty!" });
            }
            var nameLower = name.ToLower();
            var productFromDB = _unitOfWork.Product.GetFirstOrDefaultAsync(x => x.Name == nameLower, true);
            if (productFromDB != null)
            {
                return Json(new { success = false, message = "Product already exists. Please choose it from the dropdown list." });
            }
            Product newProduct;
            if (pluralName == null || pluralName == "")
            {
                newProduct = new Product(nameLower, null);
            }
            else
            {
                newProduct = new Product(nameLower, pluralName.ToLower());
            }

            await _unitOfWork.Product.AddAsync(newProduct);
            try
            {
                _unitOfWork.Save();
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Something went wrong. Please, try again later." });
            }
            //var productId = await _unitOfWork.Product.GetFirstOrDefaultAsync(x => x.Name == nameLower);
            return Json(new { success = true, message = "Product added to database.", id = newProduct.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetProductDropdownList()
        {
            return PartialView("_ProductDropdownList", new RecipeUpsertVM() { ProductList = await _unitOfWork.Product.PopulateProductData() });
        }

        /******************************************************/
        private bool ValidateMeasurementInput(string measurement)
        {
            if (measurement == null || measurement == "")
            {
                return false;
            }
            Regex regex = new Regex("^([0-9]+(?:[.,][0-9]{1,2})?)$");
            if (regex.IsMatch(measurement))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task AddImage(Recipe recipe)
        {
            string webRootPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"images\recipes");
                var extension = Path.GetExtension(files[0].FileName);

                if (recipe.ImageUrl != null)
                {
                    // this is an edit - remove old image
                    var imagePath = Path.Combine(webRootPath, recipe.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStreams);
                }
                recipe.ImageUrl = @"\images\recipes\" + fileName + extension;
            }
            else
            {
                // update without changing image
                if (recipe.Id != 0)
                {
                    Recipe recipeFromDb = await _unitOfWork.Recipe.GetAsync(recipe.Id);
                    recipe.ImageUrl = recipeFromDb.ImageUrl;
                }
                // new recipe without image
                else
                {
                    var imagePath = Path.Combine(webRootPath, @"\images\recipes\no-image.jpg");
                    if (System.IO.File.Exists(imagePath))
                    {
                        recipe.ImageUrl = @"\images\recipes\no-image.jpg";
                    }
                }
            }
        }
        private void UpdateCuisines(string[] selectedCuisines, Recipe recipe)
        {
            HashSet<string> selectedCuisinesHS;
            if (selectedCuisines == null)
            {
                selectedCuisinesHS = new HashSet<string>();
            }
            else
            {
                selectedCuisinesHS = new HashSet<string>(selectedCuisines);
            }

            recipe.CuisinesString = null;
            foreach (var cuisine in Enum.GetValues(typeof(Cuisine)))
            {
                if (selectedCuisinesHS.Contains(cuisine.ToString()))
                {
                    recipe.AddCuisine(cuisine.ToString());
                }
            }
        }
        private void UpdateMeals(string[] selectedMeals, Recipe recipe)
        {
            HashSet<string> selectedMealsHS;
            if (selectedMeals == null)
            {
                selectedMealsHS = new HashSet<string>();
            }
            else
            {
                selectedMealsHS = new HashSet<string>(selectedMeals);
            }

            recipe.MealTypesString = null;
            foreach (var meal in Enum.GetValues(typeof(MealType)))
            {
                if (selectedMealsHS.Contains(meal.ToString()))
                {
                    recipe.AddMealType(meal.ToString());
                }
            }
        }
        private void AddSteps(string[] steps, Recipe recipe)
        {
            List<Step> stepsList = new List<Step>();
            int i = 1;
            foreach (var step in steps)
            {
                if (step != null && step != "")
                {
                    stepsList.Add(new Step() { Number = i, Name = step });
                    i++;
                }
            }
            recipe.Steps = stepsList;
        }
    }
}
