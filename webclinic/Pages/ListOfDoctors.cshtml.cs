using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using webclinic.Models;

namespace webclinic.Pages
{
    public class ListOfDoctorsModel : PageModel
    {
        // The DataTable to hold all doctors data
        public DataTable AllDoctors { get; set; }

        // The DataTable to hold filtered doctors data to display
        public DataTable Doctors { get; set; }

        // Property to bind search term for doctor name
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        // Property to bind search term for doctor ID
        [BindProperty(SupportsGet = true)]
        public string SearchID { get; set; }

        // Property to bind selected status filter (Active, Banned, or All)
        [BindProperty(SupportsGet = true)]
        public string SelectedStatus { get; set; }

        // Database access instance
        public DB db { get; set; }

        // List of status options for filtering (Active, Banned, or All)
        public List<SelectListItem> StatusOptions { get; set; }

        // Constructor to initialize the DB object
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
                new SelectListItem { Text = "Show Only Active", Value = "Active" },
                new SelectListItem { Text = "Show Only Banned", Value = "Banned" }
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

            // Filtering based on the selected status (Active/Banned/All)
            if (SelectedStatus == "Active")
            {
                filteredRows = filteredRows.Where(row => row.Field<bool>("Banned") == true);
            }
            else if (SelectedStatus == "Banned")
            {
                filteredRows = filteredRows.Where(row => row.Field<bool>("Banned") == false);
            }

            // Apply the filtered rows to the Doctors DataTable
            if (filteredRows.Any())
            {
                Doctors = filteredRows.CopyToDataTable();
            }
            else
            {
                Doctors = new DataTable(); // Return an empty DataTable if no results
            }
        }
    }
}
