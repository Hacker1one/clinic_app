using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace webclinic.Pages
{
    public class ListOfDoctorsModel : PageModel
    {
        // List of all doctors (simulating a database)
        private List<Doctor> AllDoctors { get; set; }

        // List of filtered doctors to display
        public List<Doctor> Doctors { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public void OnGet()
        {
            // Sample data (replace with your database logic)
            AllDoctors = new List<Doctor>
            {
                new Doctor { Name = "Dr. Ahmed Ali", IsVerified = true },
                new Doctor { Name = "Dr. Sarah Ahmed", IsVerified = false },
                new Doctor { Name = "Dr. Yassin Omar", IsVerified = true },
                new Doctor { Name = "Dr. Maha Sameer", IsVerified = false },
                new Doctor { Name = "Dr. Khalid Basem", IsVerified = true },
                new Doctor { Name = "Dr. Amal Nour", IsVerified = true }
            };

            // Filter based on the search term
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Doctors = AllDoctors
                    .Where(d => d.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                Doctors = AllDoctors;
            }
        }

        // Optional: Add methods for actions like toggling verification status
    }

    // Doctor model
    public class Doctor
    {
        public string Name { get; set; }
        public bool IsVerified { get; set; }
    }
}

