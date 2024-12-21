using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data;
using webclinic.Models;
namespace webclinic.Pages;

[BindProperties]
public class bookModel : PageModel
{
    private readonly ILogger<bookModel> _logger;

    public bookModel(ILogger<bookModel> logger, DB db)
    {

        _logger = logger;
        this.db = db;
    }

    // Public property for therapists
    [BindProperty(SupportsGet = true)]
    public DataTable fields { get; set; }
    public DataTable Doctors { get; set; }
    public List<Therapist> Therapists { get; private set; }
    public DB db { get; set; }
    public DataTable allGovernorates { get; set; }
    public string governorate { get; set; }
    public string special { get; set; }
    public string city { get; set; }
    public DataTable allCities { get; set; }
    public void OnGet()
    {
        fields = new DataTable();
        fields = db.getdrspecialities();
        allGovernorates = new DataTable();
        allGovernorates = db.getdrgovernments();
        allCities = new DataTable();
        Doctors = new DataTable();
        Doctors = db.GetDoctorData();
       Therapists = MapDoctorsToTherapists(Doctors);
    }
    public IActionResult OnPostB()
    {
        return RedirectToPage("/book");
    }
    public IActionResult OnPostP()
    {
        return RedirectToPage("/DrProfile");
    }

    public JsonResult OnGetChangeCities(string govern)
    {

        string selectedGovernorate = allGovernorates.Rows[int.Parse(govern)]["Governorate"].ToString()!;
        HttpContext.Session.SetString("govern", selectedGovernorate);
        DataTable cities = db.getCities(selectedGovernorate); // Returns a DataTable
        List<string> cityList = cities.AsEnumerable().Select(row => row["City"].ToString()).ToList()!;
        return new JsonResult(cityList);
    }
    private List<Therapist> MapDoctorsToTherapists(DataTable doctorsData)
    {
        var therapistsList = new List<Therapist>();

        foreach (DataRow row in doctorsData.Rows)
        {
            var therapist = new Therapist
            {
                Id = Convert.ToInt32(row["ID"]),
                Name = $"{row["FName"]} {row["LName"]}", // Assuming first and last name columns
                Specialization = new Specialization
                {
                    Id = Convert.ToInt32(row["FieldCode"]),
                    Name = row["FieldName"].ToString() // FieldName column
                },
                SessionFees = Convert.ToDouble(row["PricePA"]),
                Ratings = Convert.ToDouble(row["AverageRating"]),
                ProfileImageUrl = "img/author1.jpg", // You can replace this with the actual profile image path if available
                NextDrAppointment = DateTime.Now.AddDays(1) // This is a placeholder, you can replace with actual appointment date
            };

            therapistsList.Add(therapist);
        }

        return therapistsList;
    }
};

// Therapist class
public class Therapist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Governerate { get; set; }
    public string City { get; set; }
    public int SpecializationId { get; set; }
    public double SessionFees { get; set; }
    public double Ratings { get; set; }

    // Navigation property
    public Specialization Specialization { get; set; }

    // Default to an empty list if no interests are provided
    public List<string> Interests { get; set; } = new List<string>();

    // New properties
    public string ProfileImageUrl { get; set; } // Add the ProfileImageUrl property
    public DateTime NextDrAppointment { get; set; } // Add the NextDrAppointment property
}

// Specialization class
public class Specialization
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Therapist> Therapists { get; set; }
}
