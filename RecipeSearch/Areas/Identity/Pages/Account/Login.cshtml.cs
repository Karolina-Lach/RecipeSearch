using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace RecipeSearch.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailSender _emailSender;
        public LoginModel(SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {

            [Required]
            [Display(Name = "Username or email address")]
            public string UserNameOrEmail { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (Input.UserNameOrEmail.IndexOf('@') > -1)
            {
                // email
                string emailRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
                Regex regex = new Regex(emailRegex);
                if (!regex.IsMatch(Input.UserNameOrEmail))
                {
                    ModelState.AddModelError(Input.UserNameOrEmail, "Email address is not valid");
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                    return Page();
                }
            }
            else
            {
                // username
                string userNameRegex = @"^[a-zA-Z0-9]*$";
                Regex regex = new Regex(userNameRegex);
                if (!regex.IsMatch(Input.UserNameOrEmail))
                {
                    ModelState.AddModelError(Input.UserNameOrEmail, "Username is not valid");
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                    return Page();
                }
            }
            if (ModelState.IsValid)
            {
                var userName = Input.UserNameOrEmail;
                if (Input.UserNameOrEmail.IndexOf('@') > -1)
                {
                    var user = await _userManager.FindByEmailAsync(Input.UserNameOrEmail);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                        return Page();
                    }
                    else
                    {
                        userName = user.UserName;
                    }
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(userName, Input.Password, Input.RememberMe, lockoutOnFailure: false);    //(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                //var user = await _userManager.FindByNameAsync(Input.UserName);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (Input.UserNameOrEmail.IndexOf('@') > -1)
            {
                // email
                string emailRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
                Regex regex = new Regex(emailRegex);
                if (!regex.IsMatch(Input.UserNameOrEmail))
                {
                    ModelState.AddModelError(Input.UserNameOrEmail, "Email address is not valid");
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                    return Page(); 
                }
            }
            else
            {
                // username
                string userNameRegex = @"^[a-zA-Z0-9]*$";
                Regex regex = new Regex(userNameRegex);
                if (!regex.IsMatch(Input.UserNameOrEmail))
                {
                    ModelState.AddModelError(Input.UserNameOrEmail, "Username is not valid");
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                    return Page();
                }
            }
            if (ModelState.IsValid)
            {
                IdentityUser user;
                if (Input.UserNameOrEmail.IndexOf('@') > -1)
                {
                    user = await _userManager.FindByEmailAsync(Input.UserNameOrEmail);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                        return Page();
                    }
                }
                else
                {
                    user = await _userManager.FindByNameAsync(Input.UserNameOrEmail);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                        return Page();
                    }
                }
               
                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, Input.Password);
                if(!isPasswordCorrect)
                {
                    ModelState.AddModelError(string.Empty, "Password is incorrect");
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                    return Page();
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //var confirmationLink = Url.Action("ConfirmEmail", "Account", new { code, email = user.Email }, Request.Scheme);

                var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code, returnUrl },
                protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                return Page();

            }
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return Page();
        }
    }
}
