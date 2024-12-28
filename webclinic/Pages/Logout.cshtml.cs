using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webclinic.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("user_type");
            HttpContext.Session.Remove("user_id");
            return RedirectToPage("Index");
        }
    }
}
