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
        public int id { get; set; }
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
                HttpContext.Session.Remove("invlogin");
                return RedirectToPage("Index");
            }
        }
        public IActionResult OnPostLog()
        {
            string type = db.isValidLogin(email, password);
            if(string.IsNullOrEmpty(type))
            {
                HttpContext.Session.SetString("invlogin", "1");
                return RedirectToPage("Login");
            }
            HttpContext.Session.SetString("email", email);
            if (type != "a")
            {
                id = db.getID(email);
                HttpContext.Session.SetInt32("user_id", id);
            }
            HttpContext.Session.SetString("user_type", type);
            HttpContext.Session.Remove("invlogin");
            return RedirectToPage("Index");
        }
    }
}
