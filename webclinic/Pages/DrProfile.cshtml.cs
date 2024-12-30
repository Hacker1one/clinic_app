using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using webclinic.Models;

namespace webclinic.Pages

{

    public class DrProfileModel : PageModel
    {
        
        // Properties to hold data for the Razor Page
        public string DoctorName { get; set; }
        public bool IsVerified { get; set; } = true;
        public bool IsActivated { get; set; } = true;
        public string Specialization { get; set; }
        public double Rating { get; set; }
        public int PatientsTreated { get; set; }
        public int Age { get; set; }
        public string DoctorImage { get; set; }
        public string type { get; set; }
        public int id { get; set; }
        public DataTable ClinicDetails { get; set; }
        public int Fee { get; set; }
        public List<string> AvailableDates { get; set; }
        public List<string> AvailableTimes { get; set; }
        public string natIDPic { get; set; }
        public string docCertPic { get; set; }
        public DB db { get; set; }

        public DataTable experiance { get; set; }
        public DataTable education { get; set; }

        public DataTable time { get; set; }
        public DataTable dates { get; set; }
        public DateTime date { get; set; }

        public DateTime booktime { get; set; }


        public IActionResult OnPostToggleStatus()
        {
      
            int doctorId = HttpContext.Session.GetInt32("Dr_ID").Value;

            db.ToggleDoctorStatus(doctorId);

            return RedirectToPage();
        }

        public IActionResult OnPostToggleSSSN()
        {
            int doctorId = HttpContext.Session.GetInt32("Dr_ID").Value;

            db.ToggleSSN(doctorId);

            return RedirectToPage();
        }


        public DrProfileModel(DB db)
        {
            this.db = db;

        }


        public void OnGet()
        {

            type = HttpContext.Session.GetString("user_type");
            if (type == "d")
            {
                id = HttpContext.Session.GetInt32("user_id").Value;
            }
            else
            {
                id = HttpContext.Session.GetInt32("Dr_ID").Value;
            }

            DoctorName = db.getName(id);
            Specialization = db.getMedicalField(id);
            Rating = db.getRating(id);
            PatientsTreated = db.getPatientsTreated(id);
            Age = db.getAge(id);
            DoctorImage = "https://via.placeholder.com/180";
            Fee = db.getFee(id);
            IsActivated = !db.getDrStatus(id);
            IsVerified = db.getSSN(id);
            natIDPic = db.getNationalIDPic(id);
            docCertPic = db.getDocCertPic(id);

            experiance = new DataTable();
            experiance = db.getExperiance(id);

            education = new DataTable();
            education = db.getEducation(id);

            ClinicDetails = db.getClinic(id);





            dates = new DataTable();
            dates = db.getDates(id);



        }

        public DataTable gettimes(DateTime d)
        {
            date = d;
            time = new DataTable();
            if(type == "p")
            {
                time = db.getAvailableTime(date, id);
            }
            else
            {
                time = db.getTime(date, id);

            }
            
            return time;
        }



    }
}



