using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
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
        public string ssn { get; set; }

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
        public IActionResult OnPostSignUpPatient()
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("user_type")))
            {
                return RedirectToPage("Signup");
            }

            governorate = HttpContext.Session.GetString("govern")!;
            allCities = db.getCities(governorate);
            city = allCities.Rows[int.Parse(city)]["City"].ToString()!;

            if(!string.IsNullOrEmpty(specialty))
            {
                db.addUser(fname, lname, ssn, password, governorate, city, email, gender, 
                    birthdate, HttpContext.Session.GetString("user_type")!, int.Parse(specialty)+1);
            }
            else
            {
                db.addUser(fname, lname, ssn, password, governorate, city, email, gender, 
                    birthdate, HttpContext.Session.GetString("user_type")!, -1);
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
