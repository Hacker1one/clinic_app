using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using webclinic.Models;

namespace webclinic.Pages
{
    public class AddAppointmentsModel : PageModel
    {
        private readonly DB db;  // Make sure to inject the DB class

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        [BindProperty]
        public TimeSpan StartHour { get; set; }

        [BindProperty]
        public TimeSpan EndHour { get; set; }

        [BindProperty]
        public int Duration { get; set; }  // Duration in minutes

        public AddAppointmentsModel(DB db)
        {
            this.db = db;
        }

        public IActionResult OnPost()
        {

            db.GenerateAppointments(StartDate, EndDate, StartHour, EndHour, Duration, HttpContext.Session.GetInt32("user_id").Value);

            // Redirect to a confirmation page or back to the profile page
            return RedirectToPage("DrProfile");
        }
    }
}
