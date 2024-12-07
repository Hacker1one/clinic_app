using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webclinic.Models;

namespace webclinic.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public int happypatients = 4379;
    
    public int doctors = 557;
    public DB db { get; set; }
    public IndexModel(ILogger<IndexModel> logger, DB db)
    {
        _logger = logger;
        this.db = db;
    }

    public void OnGet()
    {

    }
}
