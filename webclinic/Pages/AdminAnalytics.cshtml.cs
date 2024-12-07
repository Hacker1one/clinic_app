using ChartExample.Models.Chart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using webclinic.Models;

namespace webclinic.Pages
{
	public class AdminAnalyticsModel : PageModel
	{
		public ChartJs BarChart { get; set; }
		public string ChartJson { get; set; }
		public DB db { get; set; }
		public AdminAnalyticsModel(DB db)
		{
			BarChart = new ChartJs();
			this.db = db;
		}
		public void OnGet()
		{
			Dictionary<string, int> dayAndNum = db.getNumUsersJan();

			setUpBarChart(dayAndNum);
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
					labelsArray.Add(data.Key);
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
