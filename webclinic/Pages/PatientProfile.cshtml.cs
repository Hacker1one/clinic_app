using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webclinic.Pages
{
    public class PatientProfileModel : PageModel
    {
        public string PatientName { get; set; }
        public int PatientAge { get; set; }
        public bool IsActive { get; set; } = true;
        public List<Condition> Conditions { get; set; }
        public List<History> History { get; set; }


        public void OnGet()
        {
            PatientName = "Anna Jones";
            PatientAge = 19;

            // Long-term conditions
            Conditions = new List<Condition>
        {
            new Condition { Name = "????", Description = "Description" },
            new Condition { Name = "Cancer", Description = "Description" }
        };

            // History
            History = new List<History>
        {
            new History { DoctorName = "Dr. Yassin Elbedwihly", Title = "Oncologist", Description = "Stage 2 encephalitis" },
            new History { DoctorName = "Dr. Ahmed Abdelsamee", Title = "Oncologist", Description = "Stage 3 encephalitis" }
        };
        }

        public void OnPostToggleStatus()
        {
            // Toggle the patient's status
            IsActive = !IsActive;
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

}
