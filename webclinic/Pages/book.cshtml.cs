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
    public int maxp { get; set; }
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
       Therapists = MapDoctorsToTherapists(Doctors);
        maxprice = new DataTable();
        minprice = new DataTable();

        maxprice = db.getmaxprice();
        minprice = db.getminprice();
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
        maxprice = db.getmaxprice();
        minprice = db.getminprice();

        return Page();
    }
    public async Task<IActionResult> OnPostFilter(List<string> availability, DateTime? dateRange, int? specialization, int? governorate, int? feesRange)
    {
        // Get all doctors from the database
        DataTable originalDoctors = db.GetDoctorData(); // Load original data only once.
        DataTable filteredDoctors = originalDoctors.Clone(); // Initialize with the same structure as the original.

        if (specialization.HasValue)
        {
            var filteredRows = originalDoctors.AsEnumerable()
                .Where(row => row.Field<int>("FieldCode") == specialization.Value);

            if (filteredRows.Any())
            {
                filteredDoctors = filteredRows.CopyToDataTable();
            }
            else
            {
                filteredDoctors.Clear(); // No rows match specialization filter.
            }
        }

        if (governorate.HasValue)
        {
            var sourceTable = filteredDoctors.Rows.Count > 0 ? filteredDoctors : originalDoctors;
            var governorateRow = allGovernorates.Rows.Count > governorate.Value ? allGovernorates.Rows[governorate.Value] : null;

            if (governorateRow != null)
            {
                string selectedGovernorate = governorateRow["Governorate"].ToString();

                var filteredRows = sourceTable.AsEnumerable()
                    .Where(row => row.Field<string>("Governorate") == selectedGovernorate);

                if (filteredRows.Any())
                {
                    filteredDoctors = filteredRows.CopyToDataTable();
                }
                else
                {
                    filteredDoctors.Clear(); // No rows match governorate filter.
                }
            }
        }

        if (feesRange.HasValue)
        {
            var sourceTable = filteredDoctors.Rows.Count > 0 ? filteredDoctors : originalDoctors;

            var filteredRows = sourceTable.AsEnumerable()
                .Where(row => Convert.ToDouble(row["PricePA"]) <= feesRange.Value);

            if (filteredRows.Any())
            {
                filteredDoctors = filteredRows.CopyToDataTable();
            }
            else
            {
                filteredDoctors.Clear(); // No rows match feesRange filter.
            }
        }

        // After filtering, ensure filteredDoctors is not empty before further processing.
        // Update the UI with filteredDoctors or handle empty results accordingly.
        if (filteredDoctors.Rows.Count == 0)
        {
            // Handle empty results, e.g., show a message to the user.
            ViewData["Message"] = "No doctors found matching the selected criteria.";
        }
        else
        {
            // Proceed with displaying the filtered results.
            ViewData["Doctors"] = filteredDoctors;
        }
        
        // After applying filters, update the UI or return the filtered data.
        // Update remaining filters (allGovernorates and fields)
        if (filteredDoctors.Rows.Count > 0)
        {
            // Create a new DataTable for governorates
            DataTable governoratesTable = new DataTable();
            governoratesTable.Columns.Add("Governorate", typeof(string));

            // Populate the DataTable with unique governorates
            var uniqueGovernorates = filteredDoctors.AsEnumerable()
                .Select(row => row.Field<string>("Governorate"))
                .Distinct();

            foreach (var governorate1 in uniqueGovernorates)
            {
                governoratesTable.Rows.Add(governorate1);
            }

            allGovernorates = governoratesTable;
        }
        else
        {
            allGovernorates = new DataTable(); // Empty table
        }


        if (filteredDoctors.Rows.Count > 0)
        {
            // Create a new DataTable for fields
            DataTable fieldsTable = new DataTable();
            fieldsTable.Columns.Add("FieldCode", typeof(int));
            fieldsTable.Columns.Add("FieldName", typeof(string));

            // Populate the DataTable with unique fields
            var uniqueFields = filteredDoctors.AsEnumerable()
                .Select(row => new
                {
                    FieldCode = row.Field<int>("FieldCode"),
                    FieldName = row.Field<string>("FieldName")
                })
                .Distinct();

            foreach (var field in uniqueFields)
            {
                fieldsTable.Rows.Add(field.FieldCode, field.FieldName);
            }

            fields = fieldsTable;
        }
        else
        {
            fields = new DataTable(); // Empty table
        }

        // Map filtered doctors to therapists
        Therapists = MapDoctorsToTherapists(filteredDoctors);

        // Re-render the page with the filtered data
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
