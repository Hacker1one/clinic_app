using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using webclinic.Models;
using static System.Net.Mime.MediaTypeNames;

namespace webclinic.Pages
{
    [BindProperties]
    public class ViewDiagnosisModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public string PatientName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Conditions { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Description { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Prescription { get; set; }
        public int AppointmentID { get; set; }
        public DataTable diagnosis { get; set; }


        public DB db { get; set; }

        public ViewDiagnosisModel(DB db)
        {
            this.db = db;
        }


        public void OnGet()
        {
            if (HttpContext.Session.GetInt32("appid").HasValue && HttpContext.Session.GetInt32("appid").Value > 0 &&
                            HttpContext.Session.GetInt32("paid").HasValue && HttpContext.Session.GetInt32("paid").Value > 0)
            {
                int? appointmentid = HttpContext.Session.GetInt32("appid");

                AppointmentID = appointmentid.Value;

                diagnosis = new DataTable();

                diagnosis = db.getdiagnosis(AppointmentID);
                foreach (DataRow row in diagnosis.Rows)
                   {
                    Conditions = row.Table.Columns.Contains("condition")
                        ? row["condition"].ToString()
                        : "No Conditions available";
                    PatientName = row.Table.Columns.Contains("FName") && row.Table.Columns.Contains("LName")
                        ? row["FName"].ToString() + " " + row["LName"].ToString()
                        : "Unknown Name";
                    Description = row.Table.Columns.Contains("description")
                        ? row["description"].ToString()
                        : "No Description available";
                    Prescription = row.Table.Columns.Contains("prescription")
                       ? row["prescription"].ToString()
                       : "No Prescription available";
                }
            }
        }

    
        public void OnPost()
        {
            // Code to handle POST requests
        }
    }
}