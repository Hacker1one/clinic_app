using ChartExample.Models.Chart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using webclinic.Models;

namespace webclinic.Pages
{
	[BindProperties]
	public class AdminAnalyticsModel : PageModel
	{
		public ChartJs BarChart { get; set; }
		public string ChartJson { get; set; }
		public DB db { get; set; }
		public string utype { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime from { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]

        public DateTime to { get; set; }
		public AdminAnalyticsModel(DB db)
		{
			BarChart = new ChartJs();
			this.db = db;
		}
		public void OnGet()
		{
			if (!string.IsNullOrEmpty(HttpContext.Session.GetString("fromS")))
			{

                string fromS = HttpContext.Session.GetString("fromS")!;
                string toS = HttpContext.Session.GetString("toS")!;
                utype = HttpContext.Session.GetString("utype")!;

                Dictionary<string, int> dayAndNum1 = db.getRegisteredUsers(fromS, toS, utype);
                setUpBarChart(dayAndNum1);
				return;
			}

			string curYear = DateTime.Today.Date.Year.ToString();
			string curMonth = DateTime.Today.Date.Month.ToString();

			string today = DateTime.Today.Date.ToString("yyyy-MM-dd");

			Dictionary<string, int> dayAndNum = db.getRegisteredUsers($"{curYear}-{curMonth}-01", today, "dp");

			setUpBarChart(dayAndNum);
		}

		public IActionResult OnPostRegisteredUsers()
		{
			if (string.IsNullOrEmpty(utype) || from.Date.Year < 1990 || to.Date.Year < 1990 || from.Date.Year > to.Date.Year)
			{
				Console.WriteLine("bad");
				return RedirectToPage("AdminAnalytics");
			}
			string fromS = from.Date.ToString("yyyy-MM-dd");
			string toS = to.Date.ToString("yyyy-MM-dd");
			HttpContext.Session.SetString("fromS", fromS);
			HttpContext.Session.SetString("toS", toS);
			HttpContext.Session.SetString("utype", utype);
			return RedirectToPage();
		}
		private void setUpBarChart(Dictionary<string, int> dataToDisplay)
		{
			try
			{
				// 1. set up chart options
				BarChart.type = "bar";
				BarChart.options.responsive = true;

				// 2. separate the received Dictionary data into labels and data arrays
				var labelsArray = new List<string>();
				var dataArray = new List<double>();

				foreach (var data in dataToDisplay)
				{
					labelsArray.Add(data.Key.Split(' ')[0]);
					dataArray.Add(data.Value);
				}

				BarChart.data.labels = labelsArray;

				// 3. set up a dataset
				var firsDataset = new Dataset();
				firsDataset.label = "Number of users registered per day";
				firsDataset.data = dataArray.ToArray();

				BarChart.data.datasets.Add(firsDataset);

				// 4. finally, convert the object to json to be able to inject in HTML code
				ChartJson = JsonConvert.SerializeObject(BarChart, new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore,
				});
			}
			catch (Exception e)
			{
				Console.WriteLine("Error initialising the bar chart inside Index.cshtml.cs");
				throw e;
			}
		}
	}
}
