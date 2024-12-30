using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webclinic.Models;

namespace webclinic.Pages;

[BindProperties]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public int happypatients = 4379;


    public string img { get; set; }
    public int id { get; set; }

    public int doctors = 557;
    public DB db { get; set; }
    public IndexModel(ILogger<IndexModel> logger, DB db)
    {
        _logger = logger;
        this.db = db;
    }

    public void OnGet()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("email")))
        {
            int? userId = HttpContext.Session.GetInt32("user_id");
            if (userId.HasValue)
            {
                id = userId.Value;

                img = db.getimage(id);
            }
        }
    }
}
