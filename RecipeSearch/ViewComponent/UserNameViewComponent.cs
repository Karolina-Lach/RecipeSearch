using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using RecipeSearch.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace RecipeSearch.ViewComponents
{
    public class UserNameViewComponent : ViewComponent
    {
        //private readonly IRecipeRepository _recipeRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public UserNameViewComponent(IUnitOfWork unitOfWork,
                                     UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(claim.Value);

            return View(user);
        }
    }
}
