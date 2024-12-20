using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using webclinic.Models;

namespace webclinic.Pages
{
    public class PatientProfileModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public DataTable symptoms { get; set; }
        public DataTable history { get; set; }

        public DB db { get; set; }
        public string PatientName { get; set; }
        public int PatientAge { get; set; }
        public bool IsActive { get; set; } = true;


        public PatientProfileModel(DB db)
        {
            this.db = db;

        }
        public void OnGet()
        {


            PatientName = db.getName(HttpContext.Session.GetString("email"));
            PatientAge = db.getAge(HttpContext.Session.GetString("email"));


            symptoms = new DataTable();
            symptoms = db.getSymptoms(HttpContext.Session.GetString("email"));

            history = new DataTable();
            history = db.getHistory(HttpContext.Session.GetString("email"));


        }

    }

}

public class Condition
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class History
{
    public string DoctorName { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}


