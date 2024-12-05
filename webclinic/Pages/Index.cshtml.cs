using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webclinic.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public int happypatients = 4379;
    
    public int doctors = 557;
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
