using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public List<Condition> Conditions { get; set; }
        public List<History> History { get; set; }
        public IActionResult OnGet()
        {
            PatientName = "Anna Jones";
            PatientAge = 19;
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

           /*  if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToPage("/Login");
            }
            else
            {
                return Page();
            } */
            return Page();
        }

        public void OnPost()
        {
            // Code to handle POST requests
        }
        
    }
    
}