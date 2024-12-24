using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;
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
    [BindProperty(SupportsGet = true)]

    public DataTable Doctors { get; set; }
    [BindProperty(SupportsGet = true)]

    public List<Therapist> Therapists { get; private set; }
    [BindProperty(SupportsGet = true)]

    public DB db { get; set; }
    [BindProperty(SupportsGet = true)]

    public DataTable allGovernorates { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string governorate { get; set; }
    [BindProperty(SupportsGet = true)]
    public string special { get; set; }
    [BindProperty(SupportsGet = true)]
    public string city { get; set; }
    public int maxp { get; set; }
    public int feesRange { get; set; }
    public int minp { get; set; }
    public DataTable maxprice { get; set; }
    public DataTable minprice { get; set; }

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
        maxprice = new DataTable();
        minprice = new DataTable();

        //maxprice = db.getmaxprice();
        //minprice = db.getminprice();

        

        var filteredRows = Doctors.AsEnumerable();

        // Filtering based on the search term (name)
        if (!string.IsNullOrEmpty(special))
        {
            filteredRows = filteredRows.Where(row => row.Field<object>("FieldName") != DBNull.Value && row.Field<object>("FieldName").ToString().Contains(special, StringComparison.OrdinalIgnoreCase));

        }

        if (!string.IsNullOrEmpty(governorate))
        {
            filteredRows = filteredRows.Where(row => row.Field<object>("Governorate") != DBNull.Value && row.Field<object>("Governorate").ToString().Contains(governorate, StringComparison.OrdinalIgnoreCase));
        }

        
        if (filteredRows.Any())
        {
            Doctors = filteredRows.CopyToDataTable();
        }
        else
        {
            Doctors = new DataTable(); 
        }

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

    public IActionResult OnPostResetFilters()
    {
        // Reset all filters
        fields = db.getdrspecialities();
        allGovernorates = db.getdrgovernments();
        allCities = new DataTable();
        Doctors = db.GetDoctorData();
        Therapists = MapDoctorsToTherapists(Doctors);
        //maxprice = db.getmaxprice();
        //minprice = db.getminprice();

        return Page();
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
                ProfileImageUrl = $"{row["ProfileImageUrl"]}", 
                Governerate = $"{row["Governorate"]}",
                City = $"{row["City"]}",
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
