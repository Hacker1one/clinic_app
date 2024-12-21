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
        public string type { get; set; }
        public int id { get; set; }
        public DB db { get; set; }
        public string PatientName { get; set; }
        public int PatientAge { get; set; }
        public bool IsVerified { get; set; } = true;


        public PatientProfileModel(DB db)
        {
            this.db = db;

        }
        public void OnGet()
        {

            type = HttpContext.Session.GetString("user_type");
            if (type == "p")
            {
                id = HttpContext.Session.GetInt32("user_id").Value;
            }
            else
            {
                id = HttpContext.Session.GetInt32("Patient_ID").Value;
            }
            

            PatientName = db.getName(id);
            PatientAge = db.getAge(id);


            symptoms = new DataTable();
            symptoms = db.getSymptoms(id);

            history = new DataTable();
            history = db.getHistory(id);
            IsVerified = db.getSSN(id);


        }


        public IActionResult OnPostToggleSSSN()
        {
            int doctorId = HttpContext.Session.GetInt32("Patient_ID").Value;

            db.ToggleSSN(doctorId);

            return RedirectToPage();
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


