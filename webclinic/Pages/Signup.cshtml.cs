using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using webclinic.Models;

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
        public string city { get; set; }
        public string governorate { get; set; }

        [BindProperty(SupportsGet=true)]
        public DataTable fields { get; set; }
        public DB db { get; set; }
        public IActionResult OnGetChangeToDr(DB db)
        {
            HttpContext.Session.SetString("user_type", "dr");
            this.db = db;
            fields = new DataTable();
            fields = db.getFields();
            return Page();
        }
        public IActionResult OnGetChangeToP()
        {
            return RedirectToPage("Signup");
        }
        public void OnGet(DB db)
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
