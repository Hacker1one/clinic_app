using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Http;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web.Helpers;
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
        public string ssn { get; set; }
        public IFormFile nationalIDPic { get; set; }
        public IFormFile docCertPic { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
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

            governorate = HttpContext.Session.GetString("govern")!;
            allCities = db.getCities(governorate);
            city = allCities.Rows[int.Parse(city)]["City"].ToString()!;

            bool r = true;
            if(!string.IsNullOrEmpty(specialty))
            {
                r = await db.addUserAsync(fname, lname, ssn, password, governorate, city, email, gender,
                    birthdate, HttpContext.Session.GetString("user_type")!, int.Parse(specialty) + 1, nationalIDPic, docCertPic);
            }
            else
            {
                r = await db.addUserAsync(fname, lname, ssn, password, governorate, city, email, gender, 
                    birthdate, HttpContext.Session.GetString("user_type")!, -1, nationalIDPic, docCertPic);
            }

            if (!r)
            {
                HttpContext.Session.SetString("invsignup", "1");
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
