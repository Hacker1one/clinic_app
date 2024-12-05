using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webclinic.Pages
{
    public class PatientAppModel : PageModel
    {
        public List<DrAppointment> ConfirmedDrAppointments { get; set; }
        public List<DrAppointment> PendingDrAppointments { get; set; }
        public List<DrAppointment> CompletedDrAppointments { get; set; }
        public List<DrAppointment> CancelledDrAppointments { get; set; }
        
        public List<DrAppointment> TodayDrAppointments { get; set; }
        public List<DrAppointment> TomorrowDrAppointments { get; set; }
        public List<DrAppointment> LaterDrAppointments { get; set; }

        public void OnGet()
        {
/*             if (!string.IsNullOrEmpty(HttpContext.Session.GetString("email")))
 */            {

                Page();
                ConfirmedDrAppointments = new List<DrAppointment>
                {
                    new DrAppointment { Name = "Dr. Anna Jones", Status = "Confirmed", Time = "10:00 AM", Date = DateTime.Today.ToString("MM/dd/yyyy") },
                    new DrAppointment { Name = "Dr. Belal Smith", Status = "Confirmed", Time = "11:00 AM", Date = DateTime.Today.AddDays(1).ToString("MM/dd/yyyy") },
                    new DrAppointment { Name = "Dr. Bob Ahmed", Status = "Confirmed", Time = "02:00 PM", Date = DateTime.Today.AddDays(2).ToString("MM/dd/yyyy") }
                };
        
                TodayDrAppointments = ConfirmedDrAppointments
                    .Where(a => DateTime.Parse(a.Date) == DateTime.Today).ToList();
                TomorrowDrAppointments = ConfirmedDrAppointments
                    .Where(a => DateTime.Parse(a.Date) == DateTime.Today.AddDays(1)).ToList();
                LaterDrAppointments = ConfirmedDrAppointments
                    .Where(a => DateTime.Parse(a.Date) > DateTime.Today.AddDays(1)).ToList();
                PendingDrAppointments = new List<DrAppointment>
                    {
                        new DrAppointment { Name = "Dr. Amina", Time = "02:30 PM", Date = "04/12/2024", Status = "Pending" },
                        new DrAppointment { Name = "Dr. Ahmed Ahmed", Time = "05:00 PM", Date = "07/12/2024", Status = "Pending" }
                    };
                CompletedDrAppointments = new List<DrAppointment>
                    {
                        new DrAppointment { Name = "Dr. Ahmed Mahmoud", Time = "03:30 PM", Date = "30/11/2024", Status = "Completed" },
                        new DrAppointment { Name = "Dr. Ahmed Hossam", Time = "04:30 PM", Date = "30/11/2024", Status = "Completed" }
                    };
                CancelledDrAppointments = new List<DrAppointment>
                    {
                        new DrAppointment { Name = "Dr. Ahmed Mahmoud", Time = "03:30 PM", Date = "30/11/2024", Status = "Cancelled" },
                        new DrAppointment { Name = "Dr. Ahmed Hossam", Time = "04:30 PM", Date = "30/11/2024", Status = "Cancelled" }
                    };
            }
        }

        public void OnPost()
        {
        }
    }
}
public class DrAppointment
{
    public string Name { get; set; }
    public string Time { get; set; }
    public string Date { get; set; }
    public string Status { get; set; }
}