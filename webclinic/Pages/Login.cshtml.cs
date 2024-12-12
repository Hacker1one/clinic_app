using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webapplication.Pages
{
    [BindProperties]
    public class LoginModel : PageModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public IActionResult OnGet()
        {
			if (string.IsNullOrEmpty(HttpContext.Session.GetString("email")))
			{
                return Page();
            }
            else
            {
                return RedirectToPage("Index");
            }
        }
        public IActionResult OnPostLog()
        {
            HttpContext.Session.SetString("email", email);
            if(email == "admin@gmail.com" && password == "admin")
            {
                HttpContext.Session.SetString("user_type", "a");
            }
            if(email == "doctor@gmail.com" && password == "doctor")
            {
                HttpContext.Session.SetString("user_type", "dr");
            }
            if(email == "patient@gmail.com" && password == "patient")
            {
                HttpContext.Session.SetString("user_type", "p");
            }
            return RedirectToPage("Index");
        }
    }
}
