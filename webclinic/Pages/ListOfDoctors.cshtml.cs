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

        [BindProperty(SupportsGet = true)]
        public string SelectedStatus2 { get; set; }

        public DB db { get; set; }

        public List<SelectListItem> StatusOptions { get; set; }

        public ListOfDoctorsModel(DB db)
        {
            this.db = db;
        }

        public void OnGet()
        {

            AllDoctors = db.getAllDoctors();


            StatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Show All", Value = "" },
                new SelectListItem { Text = "Show Only Verified", Value = "Verified" },
                new SelectListItem { Text = "Show Only Unverified", Value = "Unverified" }
            };

            // Start with all doctors (no filter)
            var filteredRows = AllDoctors.AsEnumerable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                filteredRows = filteredRows.Where(row => row.Field<string>("name")
                    .Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(SearchID))
            {
                filteredRows = filteredRows.Where(row => row.Field<object>("id") != DBNull.Value &&
                                                          row.Field<object>("id").ToString()
                                                          .Contains(SearchID, StringComparison.OrdinalIgnoreCase));
            }


            if (SelectedStatus == "Banned")
            {
                filteredRows = filteredRows.Where(row => row.Field<bool>("Banned") == false);
            }
            else if (SelectedStatus == "Active")
            {
                filteredRows = filteredRows.Where(row => row.Field<bool>("Banned") == true);
            }



            if (SelectedStatus2 == "Active")
            {
                filteredRows = filteredRows.Where(row => row.Field<bool>("SSNValidation") == true);
            }
            else if (SelectedStatus2 == "Banned")
            {
                filteredRows = filteredRows.Where(row => row.Field<bool>("SSNValidation") == false);
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
