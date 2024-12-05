using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webapplication.Pages
{
    [BindProperties]
    public class LoginModel : PageModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPostLog()
        {
            return RedirectToPage("Home");
        }
    }
}
