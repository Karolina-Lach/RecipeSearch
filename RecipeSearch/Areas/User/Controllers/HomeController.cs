using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeSearch.DataAccess.Repository.IRepository;
using RecipeSearch.Models;
using RecipeSearch.Models.ViewModels;
using RecipeSearch.Search;
using RecipeSearch.Utility;

namespace RecipeSearch.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string search, string currentFilter, int? pageNumber)
        {
            Random rnd = new Random();
            var arraylist = (await GetAll(null)).ToArray();
            
            var recipes = new List<RecipeHeaderVM>
            {
                arraylist[rnd.Next(arraylist.Count())],
                arraylist[rnd.Next(arraylist.Count())]
            };
            
            var list = PaginatedList<RecipeHeaderVM>.Create(recipes.AsQueryable(), pageNumber ?? 1, 2);
            return View(list);
        }

        public async Task<IActionResult> SearchIndex(string search, string currentFilter, int? pageNumber)
        {
            if (search != null)
            {
                pageNumber = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewData["CurrentFilter"] = search;
            var recipes = await GetAll(search);

            int pageSize = 12;
            var list = PaginatedList<RecipeHeaderVM>.Create(recipes.AsQueryable(), pageNumber ?? 1, pageSize);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var userId = claim.Value;
                foreach (var item in list)
                {
                    item.IsLiked = await _unitOfWork.ListOfFavourites.IsLiked(userId, item.RecipeId);
                }
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult GetAllJson()
        {
            var recipeHeaders = GetAll(null);
            return PartialView("_RecipesList", recipeHeaders);
        }

        private async Task<IEnumerable<RecipeHeaderVM>> GetAll(string search)
        {
            IEnumerable<RecipeHeaderVM> recipesVM;
            if (search == null)
            {
                var recipes = await _unitOfWork.Recipe.GetAllAsync(null, true);
                recipesVM = recipes.Select(x => new RecipeHeaderVM(x));
            }
            else
            {
                var recipes = await _unitOfWork.Recipe.GetAllAsync(x => x.Name.Contains(search) || x.Summary.Contains(search));
                recipesVM = recipes.Select(x => new RecipeHeaderVM(x));
            }

            return recipesVM;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
