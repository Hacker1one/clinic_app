using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webclinic.Models;

namespace webclinic.Pages
{
    [BindProperties]
    public class DrAppModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int DoctorID { get; set; }
        public DB db { get; set; }

        public DrAppModel(DB db)
        {
            this.db = db;
        }

        public DataTable conapp { get; set; }
        public DataTable penapp { get; set; }
        public DataTable comapp { get; set; }
        public DataTable canapp { get; set; }

        public List<Appointment> ConfirmedAppointments { get; set; }
        public List<Appointment> PendingAppointments { get; set; }
        public List<Appointment> CompletedAppointments { get; set; }
        public List<Appointment> CancelledAppointments { get; set; }

        public List<Appointment> TodayAppointments { get; set; }
        public List<Appointment> TomorrowAppointments { get; set; }
        public List<Appointment> LaterAppointments { get; set; }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("user_type") == "d")
            {
                int? doctorId = HttpContext.Session.GetInt32("id");
                if (doctorId.HasValue)
                {
                    DoctorID = doctorId.Value;
                }

                conapp = db.getupcomingappointment(DoctorID);
                penapp = db.getpendingappointment(DoctorID);
                comapp = db.getcompletedappointment(DoctorID);
                canapp = db.getcanceledappointment(DoctorID);

                ConfirmedAppointments = convertappointment(conapp);
                PendingAppointments = convertappointment(penapp);
                CompletedAppointments = convertappointment(comapp);
                CancelledAppointments = convertappointment(canapp);

                TodayAppointments = ConfirmedAppointments
                    .Where(a => DateTime.Parse(a.Date) == DateTime.Today).ToList();
                TomorrowAppointments = ConfirmedAppointments
                    .Where(a => DateTime.Parse(a.Date) == DateTime.Today.AddDays(1)).ToList();
                LaterAppointments = ConfirmedAppointments
                    .Where(a => DateTime.Parse(a.Date) > DateTime.Today.AddDays(1)).ToList();
            }
            else
            {
                RedirectToPage("/index");
            }
        }
        public IActionResult OnGetCancel(int id)
        {
            db.CancelAppointment(id);
            return Page();
        }

        public IActionResult OnPostDiagnose(int aid, int pid, int did)
        {
            return RedirectToPage("/diagnosis", new { AppointmentID = aid, PatientID = pid, DoctorID = did });
        }
        public IActionResult OnPostView(int aid, int pid, int did)
        {
            return RedirectToPage("/ViewDiagnosis", new { AppointmentID = aid, PatientID = pid, DoctorID = did });
        }
        private List<Appointment> convertappointment(DataTable app)
        {
            var appointments = new List<Appointment>();

            foreach (DataRow row in app.Rows)
            {
                var appointment = new Appointment
                {
                    id = row.Table.Columns.Contains("AppointmentID") ? Convert.ToInt32(row["AppointmentID"]) : 0,

                    Pid = row.Table.Columns.Contains("PatientID") ? Convert.ToInt32(row["PatientID"]) : 0,

                    Did = row.Table.Columns.Contains("DoctorID") ? Convert.ToInt32(row["DoctorID"]) : 0,

                    Name = row.Table.Columns.Contains("FName") && row.Table.Columns.Contains("LName")
                    ? row["FName"].ToString() + " " + row["LName"].ToString()
                    : "Unknown Name",

                    Date = row.Table.Columns.Contains("DatenTime")
                    ? Convert.ToDateTime(row["DatenTime"]).ToString("yyyy-MM-dd")
                    : "Unknown Date",

                    Time = row.Table.Columns.Contains("DatenTime")
                     ? Convert.ToDateTime(row["DatenTime"]).ToString("HH:mm")
                       : "Unknown Time",

                    Status = GetAppointmentStatus(row),

                    image = row.Table.Columns.Contains("ProfileImageUrl")
                    ? row["ProfileImageUrl"].ToString()
                    : "default-image-url.jpg" 
                };

                appointments.Add(appointment);

            }

            return appointments;
        }

        private string GetAppointmentStatus(DataRow row)
        {
            bool isConfirmed = row.Table.Columns.Contains("IsConfirmed") && Convert.ToBoolean(row["IsConfirmed"]);
            bool isFinished = row.Table.Columns.Contains("IsFinished") && Convert.ToBoolean(row["IsFinished"]);

            // Map the status based on the IsConfirmed and IsFinished values
            if (isConfirmed && !isFinished)
            {
                return "Confirmed";
            }
            else if (isConfirmed && isFinished)
            {
                return "Completed";
            }
            else if (!isConfirmed && !isFinished)
            {
                return "Pending";
            }
            else if (!isConfirmed && isFinished)
            {
                return "Cancelled";
            }
            return "Unknown Status";
        }

        public void OnPost()
        {
            // Your post logic here, if necessary
        }
    }
}

public class Appointment
{
    public int id { get; set; }
    public int Pid { get; set; }
    public int Did { get; set; }

    public string Name { get; set; }
    public string Time { get; set; }
    public string Date { get; set; }
    public string Status { get; set; }
    public string image { get; set; }
}
