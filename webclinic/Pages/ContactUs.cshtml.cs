using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace webclinic.Pages;
public class ContactUsModel : PageModel
{
    private readonly ILogger<ContactUsModel> _logger;

    public ContactUsModel(ILogger<ContactUsModel> logger)
    {
        _logger = logger;
    }

    [BindProperty]
    [Required]
    public string Name { get; set; }

    [BindProperty]
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [BindProperty]
    [Required]
    public string Message { get; set; }

    [BindProperty]
    [Required]
    public string Phone { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPostA()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        return RedirectToPage("/Index");
    }
}