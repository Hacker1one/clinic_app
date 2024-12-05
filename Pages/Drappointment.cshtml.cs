using Microsoft.AspNetCore.Mvc.RazorPages;

namespace db2.Pages
{
    public class DrappointmentModel : PageModel
    {
        public List<Appointment> ConfirmedAppointments { get; set; }
        public List<Appointment> PendingAppointments { get; set; }
        public List<Appointment> CompletedAppointments { get; set; }
        public List<Appointment> CancelledAppointments { get; set; }
        
        public List<Appointment> TodayAppointments { get; set; }
        public List<Appointment> TomorrowAppointments { get; set; }
        public List<Appointment> LaterAppointments { get; set; }

        public void OnGet()
        {
/*             if (!string.IsNullOrEmpty(HttpContext.Session.GetString("email")))
 */            {

                Page();
                ConfirmedAppointments = new List<Appointment>
                {
                    new Appointment { Name = "John Doe", Status = "Confirmed", Time = "10:00 AM", Date = DateTime.Today.ToString("MM/dd/yyyy") },
                    new Appointment { Name = "Jane Smith", Status = "Confirmed", Time = "11:00 AM", Date = DateTime.Today.AddDays(1).ToString("MM/dd/yyyy") },
                    new Appointment { Name = "Bob Johnson", Status = "Confirmed", Time = "02:00 PM", Date = DateTime.Today.AddDays(2).ToString("MM/dd/yyyy") }
                };
        
                TodayAppointments = ConfirmedAppointments
                    .Where(a => DateTime.Parse(a.Date) == DateTime.Today).ToList();
                TomorrowAppointments = ConfirmedAppointments
                    .Where(a => DateTime.Parse(a.Date) == DateTime.Today.AddDays(1)).ToList();
                LaterAppointments = ConfirmedAppointments
                    .Where(a => DateTime.Parse(a.Date) > DateTime.Today.AddDays(1)).ToList();
                PendingAppointments = new List<Appointment>
                    {
                        new Appointment { Name = "Amina", Time = "02:30 PM", Date = "04/12/2024", Status = "Pending" },
                        new Appointment { Name = "Ahmed Ahmed", Time = "05:00 PM", Date = "07/12/2024", Status = "Pending" }
                    };
                CompletedAppointments = new List<Appointment>
                    {
                        new Appointment { Name = "Ahmed Mahmoud", Time = "03:30 PM", Date = "30/11/2024", Status = "Completed" },
                        new Appointment { Name = "Ahmed Hossam", Time = "04:30 PM", Date = "30/11/2024", Status = "Completed" }
                    };
                CancelledAppointments = new List<Appointment>
                    {
                        new Appointment { Name = "Ahmed Mahmoud", Time = "03:30 PM", Date = "30/11/2024", Status = "Cancelled" },
                        new Appointment { Name = "Ahmed Hossam", Time = "04:30 PM", Date = "30/11/2024", Status = "Cancelled" }
                    };
            }
        }

        public void OnPost()
        {
        }
    }
}
public class Appointment
{
    public string Name { get; set; }
    public string Time { get; set; }
    public string Date { get; set; }
    public string Status { get; set; }
}