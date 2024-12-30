using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using webclinic.Models;

public class AddConditionModel : PageModel
{
    public List<string> ConditionList { get; set; }

    [BindProperty]
    public string ConditionName { get; set; }

    [BindProperty]
    public int Severity { get; set; }

    [BindProperty]
    public DateTime DateOfFirstInstance { get; set; }

    public DB db { get; set; }

    public void OnGet()
    {
        ConditionList = GetConditions();
    }


    public void AddCondationModel(DB db)
    {
        this.db = db;

    }

    public IActionResult OnPost()
    {

        AddCondition(ConditionName, Severity, DateOfFirstInstance);
        return RedirectToPage("/PatientProfile");
    }

    private List<string> GetConditions()
    {
        var dataTable = db.getCondations();
        var conditions = new List<string>();

        foreach (System.Data.DataRow row in dataTable.Rows)
        {
            conditions.Add(row["ConditionName"].ToString());
        }

        return conditions;
    }


    private void AddCondition(string conditionName, int severity, DateTime dateOfFirstInstance)
    {
        int id = HttpContext.Session.GetInt32("Patient_ID").Value;
        db.Addcondation(id,conditionName,severity,dateOfFirstInstance);
    }
}
