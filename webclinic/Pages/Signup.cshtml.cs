using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data;
using webclinic.Models;

namespace webapplication.Pages
{
    [BindProperties]
    public class SignupPatientModel : PageModel
    {
        [StringLength(20, MinimumLength = 2, ErrorMessage = "First name length must be between 2 and 20 characters.")]
        [Required]
        public string fname { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Last name length must be between 2 and 20 characters.")]
        public string lname { get; set; }

        [Required]
        public string phone_number { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string email { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 4, ErrorMessage = "The password must be between 4 and 40 characters.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[\W_]).+$", ErrorMessage = "The password must contain at least one uppercase letter and one symbol.")]
        public string password { get; set; }

        [Required]
        public string gender { get; set; }

        [ValidateNever]
        public string? specialty { get; set; }

        public string city { get; set; }

        public string governorate { get; set; }

        [StringLength(14, MinimumLength = 14, ErrorMessage = "We only support egyptian national IDs. Please make sure you have entered a valid National ID.")]
        [Required]
        public string ssn { get; set; }

        [Required]
        public IFormFile nationalIDPic { get; set; }

        [ValidateNever]
        public IFormFile? docCertPic { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required]
        public DateTime birthdate { get; set; }

        [BindProperty(SupportsGet=true)]
        public DataTable fields { get; set; }
        public DB db { get; set; }

        public DataTable allGovernorates {  get; set; }

        public DataTable allCities { get; set; }
        public SignupPatientModel(DB db)
        {
            this.db = db;
            allGovernorates = new DataTable();
            allGovernorates = db.getGovernorates();
            allCities = new DataTable();
        }
        public IActionResult OnGetChangeToDr()
        {
            allCities = new DataTable();

            fields = new DataTable();
            fields = db.getFields();

            HttpContext.Session.SetString("user_type", "d");
            return Page();
        }
        public IActionResult OnGetChangeToP()
        {
            return RedirectToPage("Signup");
        }
        public void OnGet()
        {
            HttpContext.Session.SetString("user_type", "p");
        }
        public async Task<IActionResult> OnPostSignUpAsync()
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("user_type")))
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

            governorate = HttpContext.Session.GetString("govern")!;
            allCities = db.getCities(governorate);
            city = allCities.Rows[int.Parse(city)]["City"].ToString()!;

            bool r = true;
            if(!string.IsNullOrEmpty(specialty))
            {
                r = await db.addUser(fname, lname, ssn, password, governorate, city, email, gender,
                    birthdate, HttpContext.Session.GetString("user_type")!, int.Parse(specialty) + 1, nationalIDPic, docCertPic);
            }
            else
            {
                r = await db.addUser(fname, lname, ssn, password, governorate, city, email, gender, 
                    birthdate, HttpContext.Session.GetString("user_type")!, -1, nationalIDPic, docCertPic);
            }

            if (!r)
            {
                TempData["invSignUp"] = "An error occurred while changing the password. Please try again or contact us.";
                return RedirectToPage();
            }

            return RedirectToPage("Login");
        }

        public JsonResult OnGetChangeCities(string govern)
        {

            string selectedGovernorate = allGovernorates.Rows[int.Parse(govern)]["Governorate"].ToString()!;
            HttpContext.Session.SetString("govern", selectedGovernorate);
            DataTable cities = db.getCities(selectedGovernorate); // Returns a DataTable
            List<string> cityList = cities.AsEnumerable().Select(row => row["City"].ToString()).ToList()!;
            return new JsonResult(cityList);
        }

    }
}
