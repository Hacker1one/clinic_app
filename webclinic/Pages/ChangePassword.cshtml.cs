using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using webclinic.Models;

namespace webclinic.Pages
{
    [BindProperties]
    public class ChangePasswordModel : PageModel
    {
        [Required]
        [StringLength(40, MinimumLength = 4, ErrorMessage = "The password must be between 4 and 40 characters.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[\W_]).+$", ErrorMessage = "The password must contain at least one uppercase letter and one symbol.")]
        public string newP { get; set; }

        [Required]
        [Compare("newP", ErrorMessage = "The confirmation password must match the new password.")]
        public string confirmNewP { get; set; }
        public DB db { get; set; }
        public void OnGet()
        {
        }
        public ChangePasswordModel(DB db)
        {
            this.db = db;
        }

        public IActionResult OnPost()
        {

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("user_type")))
            {
                return RedirectToPage("Signup");
            }

            if (!ModelState.IsValid)
            {
                // Gather all errors
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                // Pass errors to the view
                TempData["model_errors"] = errors;

                return RedirectToPage();
            }

            int id = (int)HttpContext.Session.GetInt32("user_id")!;

            if (!db.changePassword(newP, id))
            {
                TempData["invChangeP"] = "An error occurred while changing the password. Please try again or contact us.";
                return RedirectToPage();
            }

            return RedirectToPage("LogOut");
        }

    }
}
