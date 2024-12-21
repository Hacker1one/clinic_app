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
		public ChartJs BarChart1 { get; set; }
		public ChartJs BarChart2 { get; set; }
		public ChartJs PieChart { get; set; }
		public string userChartJson { get; set; }
		public string appChartJson { get; set; }
		public string docChartJson { get; set; }
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
			BarChart1 = new ChartJs();
			BarChart2 = new ChartJs();
			PieChart = new ChartJs();
			this.db = db;
		}
		public void OnGet()
		{
			string tab = HttpContext.Session.GetString("tab")!;
            string fromS = HttpContext.Session.GetString("fromS")!;
            string toS = HttpContext.Session.GetString("toS")!;

			if (!string.IsNullOrEmpty(HttpContext.Session.GetString("fromS")))
			{
                if (tab == "users")
                {
                    utype = HttpContext.Session.GetString("utype")!;

                    Dictionary<string, int> dayAndNum = db.getRegisteredUsers(fromS, toS, utype);
                    setUpBarChart(dayAndNum, tab);
                }
                else if(tab == "app")
                {

                    Dictionary<string, int> dayAndNum = db.getAppAnalytics(fromS, toS);
                    setUpBarChart(dayAndNum, tab);

                }
                else if(tab == "doc")
                {
                    Dictionary<string, int> fieldAndNum = db.getFieldAnalytics(fromS, toS);
                    setUpPieChart(fieldAndNum);

                }
				return;
			}

            string curYear = DateTime.Today.Date.Year.ToString();
            string curMonth = DateTime.Today.Date.Month.ToString();

            string tomorrow = DateTime.Today.AddDays(1).Date.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(tab) || tab == "users")
			{
				HttpContext.Session.SetString("tab", "users");

                Dictionary<string, int> dayAndNum = db.getRegisteredUsers($"{curYear}-{curMonth}-01", tomorrow, "dp");

                setUpBarChart(dayAndNum, "users");
			}
			else if (tab == "app")
			{
                Dictionary<string, int> dayAndNum = db.getAppAnalytics($"{curYear}-{curMonth}-01", tomorrow);

                setUpBarChart(dayAndNum, tab);
			}
            else if(tab == "doc")
            {
                Dictionary<string, int> fieldAndNum = db.getFieldAnalytics($"{curYear}-{curMonth}-01", tomorrow);

                setUpBarChart(fieldAndNum, tab);
            }

		}

		public IActionResult OnPostRegisteredUsers()
		{
			if (string.IsNullOrEmpty(utype) || from.Date.Year < 1990 || to.Date.Year < 1990 || from.Date.Year > to.Date.Year)
			{
				Console.WriteLine("bad");
				return RedirectToPage();
			}
			string fromS = from.Date.ToString("yyyy-MM-dd");
			string toS = to.Date.ToString("yyyy-MM-dd");
			HttpContext.Session.SetString("fromS", fromS);
			HttpContext.Session.SetString("toS", toS);
			HttpContext.Session.SetString("utype", utype);
			HttpContext.Session.SetString("tab", "users");
			return RedirectToPage();
		}
		public IActionResult OnPostAppRange()
		{
			if (from.Date.Year < 1990 || to.Date.Year < 1990 || from.Date.Year > to.Date.Year)
			{
				Console.WriteLine("bad");
				return RedirectToPage();
			}
			string fromS = from.Date.ToString("yyyy-MM-dd");
			string toS = to.Date.ToString("yyyy-MM-dd");
			HttpContext.Session.SetString("fromS", fromS);
			HttpContext.Session.SetString("toS", toS);
			HttpContext.Session.SetString("tab", "app");
			return RedirectToPage();
		}
        public IActionResult OnGetSetTab(string tab)
        {
            // Set the session variable
            HttpContext.Session.SetString("tab", tab);

            // Redirect to the AdminAnalytics page
            return RedirectToPage();
        }

        private void setUpPieChart(Dictionary<string, int> dataToDisplay)
		{
			try
			{
				// 1. set up chart options
                PieChart.type = "pie";
                PieChart.options.responsive = true;

                // 2. separate the received Dictionary data into labels and data arrays
                var labelsArray = new List<string>();
                var dataArray = new List<double>();

                foreach (var data in dataToDisplay)
                {
                    labelsArray.Add(data.Key.Split(' ')[0]);
                    dataArray.Add(data.Value);
                }

                PieChart.data.labels = labelsArray;

                // 3. set up a dataset
                var firsDataset = new Dataset();
                firsDataset.label = "Number of Users registered per day";
                firsDataset.data = dataArray.ToArray();

                PieChart.data.datasets.Add(firsDataset);

                // 4. finally, convert the object to json to be able to inject in HTML code
                docChartJson = JsonConvert.SerializeObject(PieChart, new JsonSerializerSettings
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

        private void setUpBarChart(Dictionary<string, int> dataToDisplay, string tab)
		{
			try
			{
				// 1. set up chart options
				if (tab == "users")
				{
                    BarChart1.type = "bar";
                    BarChart1.options.responsive = true;

                    // 2. separate the received Dictionary data into labels and data arrays
                    var labelsArray = new List<string>();
                    var dataArray = new List<double>();

                    foreach (var data in dataToDisplay)
                    {
                        labelsArray.Add(data.Key.Split(' ')[0]);
                        dataArray.Add(data.Value);
                    }

                    BarChart1.data.labels = labelsArray;

                    // 3. set up a dataset
                    var firsDataset = new Dataset();
                    firsDataset.label = "Number of Users registered per day";
                    firsDataset.data = dataArray.ToArray();

                    BarChart1.data.datasets.Add(firsDataset);

                    // 4. finally, convert the object to json to be able to inject in HTML code
                    userChartJson = JsonConvert.SerializeObject(BarChart1, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    });

				}
				else if (tab == "app")
				{
                    BarChart2.type = "bar";
                    BarChart2.options.responsive = true;

                    // 2. separate the received Dictionary data into labels and data arrays
                    var labelsArray = new List<string>();
                    var dataArray = new List<double>();

                    foreach (var data in dataToDisplay)
                    {
                        labelsArray.Add(data.Key.Split(' ')[0]);
                        dataArray.Add(data.Value);
                    }

                    BarChart2.data.labels = labelsArray;

                    // 3. set up a dataset
                    var firsDataset = new Dataset();
                    firsDataset.label = "Number of Appointments per day";
                    firsDataset.data = dataArray.ToArray();

                    BarChart2.data.datasets.Add(firsDataset);

                    // 4. finally, convert the object to json to be able to inject in HTML code
                    appChartJson = JsonConvert.SerializeObject(BarChart2, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    });
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Error initialising the bar chart inside Index.cshtml.cs");
				throw e;
			}
		}
	}
}
