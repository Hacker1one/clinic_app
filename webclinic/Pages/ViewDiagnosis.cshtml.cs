using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace webclinic.Pages
{
    
    public class ViewDiagnosisModel : PageModel
    {

        public string PatientName { get; set; } = "John Doe";
        public string Conditions { get; set; } = "Hypertension, Diabetes";
        public string Description { get; set; } = "The patient is experiencing high blood pressure and elevated blood sugar levels.";
        public string Prescription { get; set; } = "Take prescribed medications daily and follow up in two weeks.";
    
        private readonly ILogger<ViewDiagnosisModel> _logger;

        public ViewDiagnosisModel(ILogger<ViewDiagnosisModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // Code to handle GET requests
        }

        public void OnPost()
        {
            // Code to handle POST requests
        }
    }
}