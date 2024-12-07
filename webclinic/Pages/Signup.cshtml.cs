using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webapplication.Pages
{
    [BindProperties]
    public class SignupPatientModel : PageModel
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
        public void OnGet()
        {
            HttpContext.Session.SetString("user_type", "p");
        }
        public IActionResult OnPostSignUpPatient()
        {
            Console.WriteLine(specialty);
            return RedirectToPage("DocumentUpload");
        }
    }
}
