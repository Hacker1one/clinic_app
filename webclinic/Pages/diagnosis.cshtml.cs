using System.Reflection.Metadata;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webclinic.Models;
using static System.Net.WebRequestMethods;

namespace webclinic.Pages
{
    [BindProperties]
    public class DiagnosisModel : PageModel
    {
        public string PatientName { get; set; }
        [BindProperty(SupportsGet =true)]
        public int PatientAge { get; set; }
        [BindProperty(SupportsGet = true)]
        public int AppointmentID { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PatientID { get; set; }
        [BindProperty(SupportsGet = true)]
        public int DoctorID { get; set; }

        [BindProperty(SupportsGet = true)]
        
        public string condition1 { get; set; }

        [BindProperty(SupportsGet = true)]
        public string description { get; set; }

        [BindProperty(SupportsGet = true)]
        public string prescription { get; set; }

        public DB db { get; set; }
        public DiagnosisModel(DB db)
        {
            this.db = db;
        }

        public List<Condition> Conditions { get; set; }
        public List<History> History { get; set; }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("appid").HasValue && HttpContext.Session.GetInt32("appid").Value > -1 &&
                HttpContext.Session.GetInt32("paid").HasValue && HttpContext.Session.GetInt32("paid").Value > 0)
            {
                int? doctorId = HttpContext.Session.GetInt32("user_id");
                if (doctorId.HasValue)
                {
                    DoctorID = doctorId.Value;
                }
                int? patientId = HttpContext.Session.GetInt32("paid");
                if (patientId.HasValue)
                {
                    PatientID = patientId.Value;
                }
                int? appointmentid = HttpContext.Session.GetInt32("appid");
                if (appointmentid.HasValue)
                {
                    AppointmentID = appointmentid.Value;
                }

                PatientName = "Anna Jones";
                PatientAge = 19;
                Conditions = new List<Condition>
                 {
                    new Condition { Name = "????", Description = "Description" },
                    new Condition { Name = "Cancer", Description = "Description" }
                    };

                History = new List<History>
                {
                    new History { DoctorName = "Dr. Yassin Elbedwihly", Title = "Oncologist", Description = "Stage 2 encephalitis" },
                    new History { DoctorName = "Dr. Ahmed Abdelsamee", Title = "Oncologist", Description = "Stage 3 encephalitis" }
                    };

                return Page(); // Return the current page
            }
            else
            {
                return RedirectToPage("/index");
            }
        }

        public IActionResult OnPostComplete()
        {
            db.AddDiagnosis(AppointmentID, DoctorID, PatientID, condition1, description, prescription);
            HttpContext.Session.SetInt32("appid", -1);
            HttpContext.Session.SetInt32("paid", 0);
            db.completeappointment(AppointmentID);
            return RedirectToPage("/DrApp");
        }
        public void OnPost()
        {
            // Code to handle POST requests
        }
        
    }
    
}