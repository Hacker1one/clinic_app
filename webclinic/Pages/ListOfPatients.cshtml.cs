using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace webclinic.Pages
{
    public class ListOfPatientsModel : PageModel
    {
        // List of all patients (simulating a database)
        private List<Patient> AllPatients { get; set; }

        // List of filtered patients to display
        public List<Patient> Patients { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public void OnGet()
        {
            // Sample data (replace with your database logic)
            AllPatients = new List<Patient>
            {
                new Patient { Name = "Ahmed Ahmed", IsActive = true },
                new Patient { Name = "Basel Ahmed", IsActive = false },
                new Patient { Name = "Caleb Ahmed", IsActive = false },
                new Patient { Name = "David Basel", IsActive = true },
                new Patient { Name = "Endrick Ahmed", IsActive = true },
                new Patient { Name = "Fares Basel", IsActive = true }
            };

            // Filter based on the search term
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Patients = AllPatients
                    .Where(p => p.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                Patients = AllPatients;
            }
        }
    }

    // Patient model
    public class Patient
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
