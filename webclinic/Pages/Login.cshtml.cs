using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Common;
using webclinic.Models;

namespace webapplication.Pages
{
    [BindProperties]
    public class LoginModel : PageModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public DB db { get; set; }
        public LoginModel(DB db)
        {
            this.db = db;
        }
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
            string type = db.isValidLogin(email, password);
            if(string.IsNullOrEmpty(type))
            {
                HttpContext.Session.SetString("err", "1");
                return RedirectToPage("Login");
            }
            HttpContext.Session.SetString("email", email);
            HttpContext.Session.SetString("user_type", type);
            return RedirectToPage("Index");
        }
    }
}
