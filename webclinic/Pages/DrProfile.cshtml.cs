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
        public string Specialization { get; set; }
        public double Rating { get; set; }
        public int PatientsTreated { get; set; }
        public int Age { get; set; }
        public string DoctorImage { get; set; }
        
        public DataTable ClinicDetails { get; set; }
        public int Fee { get; set; }
        public List<string> AvailableDates { get; set; }
        public List<string> AvailableTimes { get; set; }
        public DB db { get; set; }

        public DataTable experiance { get; set; }
        public DataTable education { get; set; }


        public void OnPostToggleStatus()
        {
            // Toggle the patient's status
            IsVerified = !IsVerified;
        }
        public class Education
        {
            public string Institution { get; set; }
            public string Degree { get; set; }
        }

        public class Experience
        {
            public string Position { get; set; }
            public string Organization { get; set; }
            public string Description { get; set; }
            
        }

        public class Clinic
        {
            public string Name { get; set; }
            public string Address { get; set; }
        }

        public DrProfileModel(DB db)
        {
            this.db = db;

        }


        public void OnGet()
        {

            DoctorName = db.getName(HttpContext.Session.GetString("email"));
            Specialization = db.getMedicalField(HttpContext.Session.GetInt32("id").Value);
            Rating = db.getRating(HttpContext.Session.GetInt32("id").Value);
            PatientsTreated = db.getPatientsTreated(HttpContext.Session.GetInt32("id").Value);
            Age = db.getAge(HttpContext.Session.GetString("email"));
            DoctorImage = "https://via.placeholder.com/180";
            Fee = db.getFee(HttpContext.Session.GetInt32("id").Value);

            experiance = new DataTable();
            experiance = db.getExperiance(HttpContext.Session.GetInt32("id").Value);

            education = new DataTable();
            education = db.getEducation(HttpContext.Session.GetInt32("id").Value);

            ClinicDetails = db.getClinic(HttpContext.Session.GetInt32("id").Value);

            // Available dates and times section
            AvailableDates = new List<string>
            {
                "17 Mon", "18 Tue", "19 Wed", "20 Thu", "21 Fri", "22 Sat"
            };

            AvailableTimes = new List<string>
            {
                "8:00", "10:30", "13:00", "14:30", "15:00", "16:30"
            };

        }
    }
}



