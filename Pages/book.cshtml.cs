using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace db2.Pages;

public class bookModel : PageModel
{
    private readonly ILogger<bookModel> _logger;

    public bookModel(ILogger<bookModel> logger)
    {
        _logger = logger;
    }

    // Public property for therapists
    public List<Therapist> Therapists { get; private set; }

    public void OnGet()
    {
        Therapists = GetSampleTherapists();
    }

    // Method to return a list of sample therapists
    private List<Therapist> GetSampleTherapists()
    {
        return new List<Therapist>
        {
            new Therapist
            {
                Id = 1,
                Name = "Huda Radwan",
                Specialization = new Specialization { Id = 1, Name = "Psychiatrist" },
                SessionFees = 1500,
                Ratings = 4.85,
                ProfileImageUrl = "img/author1.jpg",
            },
            new Therapist
            {
                Id = 2,
                Name = "Bahaa Mahmoud",
                Specialization = new Specialization { Id = 2, Name = "Psychologist" },
                SessionFees = 620,
                Ratings = 4.89,
                ProfileImageUrl = "img/author1.jpg",

            },
            new Therapist
            {
                Id = 2,
                Name = "Bahaa Mahmoud",
                Specialization = new Specialization { Id = 2, Name = "Psychologist" },
                SessionFees = 620,
                Ratings = 4.89,
                ProfileImageUrl = "img/author1.jpg",

            },
            new Therapist
            {
                Id = 2,
                Name = "Bahaa Mahmoud",
                Specialization = new Specialization { Id = 2, Name = "Psychologist" },
                SessionFees = 620,
                Ratings = 4.89,
                ProfileImageUrl = "img/author1.jpg",

            }
        };
    }
}

// Therapist class
public class Therapist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SpecializationId { get; set; }
    public double SessionFees { get; set; }
    public double Ratings { get; set; }

    // Navigation property
    public Specialization Specialization { get; set; }

    // Default to an empty list if no interests are provided
    public List<string> Interests { get; set; } = new List<string>();

    // New properties
    public string ProfileImageUrl { get; set; } // Add the ProfileImageUrl property
    public DateTime NextAppointment { get; set; } // Add the NextAppointment property
}

// Specialization class
public class Specialization
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Therapist> Therapists { get; set; }
}
