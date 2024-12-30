using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using webclinic.Models;

namespace webclinic.Pages
{
    public class ListOfPatientsModel : PageModel
    {

        public DataTable AllPatients { get; set; }


        public DataTable Patients { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SearchID { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SelectedStatus { get; set; }


        public DB db { get; set; }

        public List<SelectListItem> StatusOptions { get; set; }


        public ListOfPatientsModel(DB db)
        {
            this.db = db;
        }


        public void OnGet()
        {

            AllPatients = db.getAllPatients();


            StatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Show All", Value = "" },
                new SelectListItem { Text = "Show Only Active", Value = "Active" },
                new SelectListItem { Text = "Show Only Banned", Value = "Banned" }
            };


            var filteredRows = AllPatients.AsEnumerable();


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


            if (SelectedStatus == "Active")
            {
                filteredRows = filteredRows.Where(row => row.Field<bool>("SSNValidation") == true);
            }
            else if (SelectedStatus == "Banned")
            {
                filteredRows = filteredRows.Where(row => row.Field<bool>("SSNValidation") == false);
            }


            if (filteredRows.Any())
            {
                Patients = filteredRows.CopyToDataTable();
            }
            else
            {
                Patients = new DataTable();
            }



        }
        public IActionResult OnPostViewPatient(string PatientId)
        {
            if (int.TryParse(PatientId, out int patientId))
            {
                HttpContext.Session.SetInt32("Patient_ID", patientId);
                return RedirectToPage("/PatientProfile");
            }

            return BadRequest("Invalid Patient ID");
        }
    }
}
