using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using RecipeSearch.DataAccess.Repository.IRepository;
using RecipeSearch.Models;
using RecipeSearch.Models.ViewModels;
using RecipeSearch.Search;
using RecipeSearch.Utility;

namespace RecipeSearch.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class GroceriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        public GroceriesController(IEmailSender emailSender,
            UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var userId = claim.Value;
            

            return View(new GroceriesListVM() { Groceries = _unitOfWork.GroceryList.GetGroceriesDictionary(userId)});
        }

        [HttpDelete]
        public IActionResult ClearList()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var userId = claim.Value;
           

            IEnumerable<GroceryItem> list = _unitOfWork.GroceryList.GetAll(x => x.ApplicationUserId == userId);
            if (list.Count() == 0)
            {
                return Json(new { success = false, message = "The list is clear!" });
            }

            _unitOfWork.GroceryList.RemoveRange(list);
            try
            {
                _unitOfWork.Save();
            }
            catch(Exception exception)
            {
                return Json(new { success = false, message = "Oops, something went wrong. Please try again later." });
            }

            return Json(new { success = true, message = "Everything is fine." });
        }

        [HttpGet]
        public async Task<IActionResult> SendListToEmail()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var user = await _userManager.FindByIdAsync(claim.Value);
            var email = await _userManager.GetEmailAsync(user);
            if (email == null)
            {
                return Json(new { success = false, message = "Your email is not set. To set your email, please go to profile settings." });
            }

            var list = _unitOfWork.GroceryList.GetGroceriesDictionary(claim.Value);
            if(list.Count() == 0)
            {
                return Json(new { success = false, message = "Your list is empty." });
            }

            var subject = "Grocery list";
            string messageBody = CreateEmailBody(list, subject);
            await _emailSender.SendEmailAsync(email, subject, messageBody);

            return Json(new { success = true, message = "Email has been sent. Do you want to clear your list?" });
        }
    
        [HttpDelete]
        public IActionResult DeleteGroceryListItem(string productName)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var id = claim.Value;

            IEnumerable<GroceryItem> itemsToRemove = _unitOfWork.GroceryList.GetAll(productName, id);
            _unitOfWork.GroceryList.RemoveRange(itemsToRemove);
            try
            {
                _unitOfWork.Save();
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Oops, something went wrong. Please try again later." });
            }

            return Json(new { success = true, message = "Everything is fine." });
        }

        [HttpGet]
        public IActionResult GetGroceryList()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var userId = claim.Value;
            

            return PartialView("_GroceryList", _unitOfWork.GroceryList.GetGroceriesDictionary(userId));
        }

        private string CreateEmailBody(Dictionary<string, List<Ingredient>> list, string subject)
        {
            string listEmail = "";
            foreach (var product in list)
            {
                listEmail += "<li style=\"font-size:20px;\">";
                listEmail += product.Key;
                listEmail += "<ul>";
                foreach (var item in product.Value)
                {
                    listEmail += "<li style=\"font-size:16px; padding:10px\">" + item.DisplayMeasurement() + "</li>";
                }
                listEmail += "</ul></li>";
            }

            var pathToFile = _hostEnvironment.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "templates"
                    + Path.DirectorySeparatorChar.ToString()
                    + "groceryList.html";

            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            
            var date = DateTime.Today.ToString("dd.MM.yyyy");

            string messageBody = string.Format(builder.HtmlBody,
                subject,
                date,
                listEmail
                );

            return messageBody;
        }
    }
}
