using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace RecipeSearch.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ChangePasswordModel> _logger;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        public string Username { get; set; }
        public bool HasPassword { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [EmailAddress]
            [Display(Name = "Email")]
            public string  NewEmail{ get; set; }

            //[Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            //[Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var hasPassword = await _userManager.HasPasswordAsync(user);
            
            Username = userName;
            HasPassword = hasPassword;
            Input = new InputModel
            {
                NewEmail = email
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostEmailChangeAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, Input.Password);
                if (!isPasswordCorrect)
                {
                    StatusMessage = "Password is incorrect.";
                    return RedirectToPage();
                }

                var result = await _userManager.SetEmailAsync(user, Input.NewEmail);
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                       "/Account/ConfirmEmail",
                       pageHandler: null,
                       values: new { area = "Identity", userId, code },
                       protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(Input.NewEmail, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                
                
                if(result.Succeeded)
                {
                    StatusMessage = "Email has been changed. Please check your mailbox to confirm it.";
                    return RedirectToPage();
                }
            }

            StatusMessage = "Your email is unchanged.";
            return RedirectToPage(); 
        }

        public async Task<IActionResult> OnPostPasswordChangeAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if(hasPassword)
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.Password, Input.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }

                await _signInManager.RefreshSignInAsync(user);
                _logger.LogInformation("User changed their password successfully.");
                StatusMessage = "Your password has been changed.";

                return RedirectToPage();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
                if (!addPasswordResult.Succeeded)
                {
                    foreach (var error in addPasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }

                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Your password has been set.";

                return RedirectToPage();
            }
            
        }
    }
}
