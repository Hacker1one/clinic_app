using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webclinic.Pages
{
    [BindProperties]
    public class SignupPatient1Model : PageModel
    {
        [BindProperty (SupportsGet=true)]
        public string user_type { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string gender { get; set; }
        public string specialty { get; set; }
        public IActionResult OnGetChangeToDr()
        {
            HttpContext.Session.SetString("user_type", "dr");
            return Page();
        }
        public IActionResult OnGetChangeToP()
        {
            HttpContext.Session.SetString("user_type", "p");
            return Page();
        }
        public void OnGet(string type)
        {
            if (type == "p")
            {
                HttpContext.Session.SetString("user_type", "p");
            }
            else if (type == "dr")
            {
                HttpContext.Session.SetString("user_type", "dr");
            }
        }
        public IActionResult OnPostSignUpPatient()
        {
            Console.WriteLine(specialty);
            return RedirectToPage("Login");
        }
    }
}
