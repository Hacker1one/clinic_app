using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public string About { get; set; }
        public string DoctorImage { get; set; }
        public List<Education> Education_list { get; set; }
        public List<Experience> Experience_list { get; set; }
        public Clinic ClinicDetails { get; set; }
        public string Fee { get; set; }
        public List<string> AvailableDates { get; set; }
        public List<string> AvailableTimes { get; set; }



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

        public void OnGet()
        {
            // Dummy data, you would typically get this from a database
            DoctorName = "Dr. Anna Jones";
            Specialization = "General Practitioner";
            Rating = 4.5;
            PatientsTreated = 1000;
            About = "Dr. Anna Jones is a renowned general practitioner with extensive experience...";
            DoctorImage = "https://via.placeholder.com/180";
            Fee = "800 EGP";

            // Education section
            Education_list = new List<Education>
            {
                new Education { Institution = "Mansoura University", Degree = "Bachelor's Degree" },
                new Education { Institution = "Cairo University", Degree = "Doctorate Degree" }
            };

            // Experience section
            Experience_list = new List<Experience>
            {
                new Experience { Position = "Consultant", Organization = "Salam Hospital", Description = "Extensive work in general practice..." },
                new Experience { Position = "Professor", Organization = "Mansoura University", Description = "Lecturing and research in medical fields..." }
            };

            // Clinic details section
            ClinicDetails = new Clinic
            {
                Name = "Care Medical Center",
                Address = "1284 W. Grey St, D1"
            };

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



