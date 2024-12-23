using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using webclinic.Models;

namespace webclinic.Pages
{
    public class ListOfDoctorsModel : PageModel
    {
        
        public DataTable AllDoctors { get; set; }

        public DataTable Doctors { get; set; }

        
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchID { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedStatus { get; set; }

        public DB db { get; set; }

        public List<SelectListItem> StatusOptions { get; set; }

        public ListOfDoctorsModel(DB db)
        {
            this.db = db;
        }

        // OnGet method to handle the logic for retrieving and filtering doctor data
        public void OnGet()
        {
            // Retrieve all doctors from the database (simulating with db.getAllDoctors())
            AllDoctors = db.getAllDoctors();

            // Set up the status filter options
            StatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Show All", Value = "" },
                new SelectListItem { Text = "Show Only Verified", Value = "Verified" },
                new SelectListItem { Text = "Show Only Unverified", Value = "Unverified" }
            };

            // Start with all doctors (no filter)
            var filteredRows = AllDoctors.AsEnumerable();

            // Filtering based on the search term (name)
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                filteredRows = filteredRows.Where(row => row.Field<string>("name")
                    .Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Filtering based on the search term (ID)
            if (!string.IsNullOrEmpty(SearchID))
            {
                filteredRows = filteredRows.Where(row => row.Field<object>("id") != DBNull.Value &&
                                                          row.Field<object>("id").ToString()
                                                          .Contains(SearchID, StringComparison.OrdinalIgnoreCase));
            }


            if (SelectedStatus == "Verified")
            {
                filteredRows = filteredRows.Where(row => row.Field<bool>("Unverified") == true);
            }
            else if (SelectedStatus == "Unverified")
            {
                filteredRows = filteredRows.Where(row => row.Field<bool>("Unverified") == false);
            }

            if (filteredRows.Any())
            {
                Doctors = filteredRows.CopyToDataTable();
            }
            else
            {
                Doctors = new DataTable(); 
            }
        }

        public IActionResult OnPostViewDoctor(string DoctorId)
        {
            if (int.TryParse(DoctorId, out int doctorId))
            {
                HttpContext.Session.SetInt32("Dr_ID", doctorId);
                return RedirectToPage("/DrProfile");
            }

            return BadRequest("Invalid Doctor ID");
        }
    }
}
