using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalFinanceTracker.Models;
using Microsoft.AspNetCore.Mvc.Routing;

namespace PersonalFinanceTracker.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender<ApplicationUser> _sender;

        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender<ApplicationUser> sender)
        {
            _userManager = userManager;
            _sender = sender;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return;
            }

            Email = email;
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            DisplayConfirmAccountLink = false;
            if (DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                EmailConfirmationUrl = Url.PageLink("/Account/ConfirmEmail", null, new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl }, Request.Scheme);
            }
        }
    }
}
