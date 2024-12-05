using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webclinic.Pages
{
    [BindProperties]
    public class OTPModel : PageModel
    {
        public string otp { get; set; }
        public void OnGet()
        {
            Random generator = new Random();
            string n = generator.Next(0, 1000000).ToString("D6");
            Console.WriteLine(n);
            HttpContext.Session.SetString("otp", n);
        }
        public IActionResult OnPost()
        {
            if (otp == HttpContext.Session.GetString("otp"))
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
