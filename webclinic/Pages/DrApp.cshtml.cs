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
                int? doctorId = HttpContext.Session.GetInt32("user_id");
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
        public IActionResult OnPostCancel(int id)
        {
            db.CancelAppointment(id);
            return RedirectToPage("/DrApp");
        }
        public IActionResult OnPostConfirm(int id)
        {
            db.confirmappointment(id);
            return RedirectToPage("/DrApp");
        }
        public IActionResult OnPostDiagnose(int aid, int pid, int did)
        {
            HttpContext.Session.SetInt32("appid", aid);
            HttpContext.Session.SetInt32("paid", pid);
            return RedirectToPage("/diagnosis");
        }
        public IActionResult OnPostView(int aid, int pid, int did)
        {
            HttpContext.Session.SetInt32("appid", aid);
            HttpContext.Session.SetInt32("paid", pid);
            return RedirectToPage("/ViewDiagnosis");
        }
        private List<Appointment> convertappointment(DataTable app)
        {
            var appointments = new List<Appointment>();

            foreach (DataRow row in app.Rows)
            {
                int appointmentId = row.Table.Columns.Contains("AppointmentID") ?
                    Convert.ToInt32(row["AppointmentID"]) : 0;

                var appointment = new Appointment
                {
                    id = appointmentId,
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
                    Status = db.GetAppointmentStatus(appointmentId),
                    image = row.Table.Columns.Contains("ProfileImageUrl")
                        ? row["ProfileImageUrl"].ToString()
                        : "default-image-url.jpg"
                };

                appointments.Add(appointment);
            }

            return appointments;
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
