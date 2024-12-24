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
    public class PatientAppModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int PatientID { get; set; }
        public DB db { get; set; }

        public PatientAppModel(DB db)
        {
            this.db = db;
        }
        public DataTable conapp { get; set; }
        public DataTable penapp { get; set; }
        public DataTable comapp { get; set; }
        public DataTable canapp { get; set; }

        public List<DrAppointment> ConfirmedDrAppointments { get; set; }
        public List<DrAppointment> PendingDrAppointments { get; set; }
        public List<DrAppointment> CompletedDrAppointments { get; set; }
        public List<DrAppointment> CancelledDrAppointments { get; set; }
        
        public List<DrAppointment> TodayDrAppointments { get; set; }
        public List<DrAppointment> TomorrowDrAppointments { get; set; }
        public List<DrAppointment> LaterDrAppointments { get; set; }

        public void OnGet()
        {
             if (HttpContext.Session.GetString("user_type") == "p")
            {
                int? patientid = HttpContext.Session.GetInt32("user_id");
                if (patientid.HasValue)
                {
                    PatientID = patientid.Value;
                }

                conapp = db.getpaupcomingappointment(PatientID);
                penapp = db.getpapendingappointment(PatientID);
                comapp = db.getpacompletedappointment(PatientID);
                canapp = db.getpacanceledappointment(PatientID);

                ConfirmedDrAppointments = convertappointment(conapp);
                PendingDrAppointments = convertappointment(penapp);
                CompletedDrAppointments = convertappointment(comapp);
                CancelledDrAppointments = convertappointment(canapp);

                TodayDrAppointments = ConfirmedDrAppointments
                    .Where(a => DateTime.Parse(a.Date) == DateTime.Today).ToList();
                TomorrowDrAppointments = ConfirmedDrAppointments
                    .Where(a => DateTime.Parse(a.Date) == DateTime.Today.AddDays(1)).ToList();
                LaterDrAppointments = ConfirmedDrAppointments
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
            return RedirectToPage("/PatientApp");
        }
        public IActionResult OnPostView(int aid, int pid, int did)
        {
            HttpContext.Session.SetInt32("appid", aid);
            HttpContext.Session.SetInt32("paid", pid);
            return RedirectToPage("/ViewDiagnosis");
        }
        private List<DrAppointment> convertappointment(DataTable app)
        {
            var appointments = new List<DrAppointment>();

            foreach (DataRow row in app.Rows)
            {
                int appointmentId = row.Table.Columns.Contains("AppointmentID") ?
                    Convert.ToInt32(row["AppointmentID"]) : 0;

                var appointment = new DrAppointment
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
        }

        public IActionResult OnPostReview(int did, int pid, int ns, string com)
        {
            db.AddReview(did, pid, ns, com);
            return RedirectToPage("/PatientApp");
        }

    }
}

public class DrAppointment
{
    public int id { get; set; }
    public int Pid { get; set; }
    public int Did { get; set; }

    public string Name { get; set; }
    public string Time { get; set; }
    public string Date { get; set; }
    public string Status { get; set; }
    public string image { get; set; }
    public string comment { get; set; }
    public int Ns { get; set; }
    
}
