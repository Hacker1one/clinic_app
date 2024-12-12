using Microsoft.Data.SqlClient;
using System.Data;

namespace webclinic.Models
{
	public class DB
	{
		private string connectionString;
		private SqlConnection con = new SqlConnection();

		public DB()
		{
			// change the constring before running locally here
			connectionString = "Data Source=AMNESIA\\SQLEXPRESS; Initial Catalog=clinicdb; Integrated Security=True; Trust Server Certificate = True;";
			con.ConnectionString = connectionString;
		}

		public DataTable getFields()
		{
			string queryString = "Select FieldName from FieldOfMedicine";
			DataTable dt = new DataTable();
			SqlCommand cmd = new SqlCommand(queryString, con);
			try
			{
				con.Open();
                dt.Load(cmd.ExecuteReader());
            }
			catch (Exception ex)
			{
				Console.Write(ex.ToString());
			}
			finally
			{
				con.Close();
			}

			return dt;
        }

		public Dictionary<string, int> getNumUsersJan()
		{
			string queryString = "select Day(RegistrationDate) as [Day], count(RegistrationDate) as NumUsersInJan from [user] Where RegistrationDate between '2023-01-01' and '2023-01-30' group by RegistrationDate";
			SqlCommand cmd = new SqlCommand(queryString, con);
			Dictionary<string, int> dayAndNum = new Dictionary<string, int>();
			try
			{
				con.Open();
				SqlDataReader rdr  = cmd.ExecuteReader();
				while (rdr.Read())
				{
					dayAndNum.Add(rdr["day"].ToString(), (int)rdr["NumUsersInJan"]);

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				con.Close();
			}
			return dayAndNum;
		}
	}
}
