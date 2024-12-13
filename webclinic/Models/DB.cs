using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using static Azure.Core.HttpHeader;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Security.Principal;
using webclinic.Pages;
using System.Runtime.Versioning;
using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;

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
		public DataTable getGovernorates()
		{
			string queryString = "select * from AllowedGovernorates";
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

		public DataTable getCities(string governorate)
		{
			string queryString = $"Select * from AllowedCities Where Governorate = '{governorate}'";
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
		public bool addUser(string fname, string lname, string ssn, string password, string governorate, string city, string email, string gender, DateTime birthdate, string user_type, int field_code)
		{
			string today = DateTime.Today.Date.ToString("yyyy-MM-dd");
			string bd = birthdate.Date.ToString("yyyy-MM-dd");

			string queryString;
			if (user_type == "p")
			{
				queryString = "BEGIN TRANSACTION \n" +
				"INSERT INTO[user] \n" +
				"(FName, LName, SSN, RegistrationDate, Gender, [Password], BirthDate, City, Governorate, Email, [type]) \n" +
				"VALUES " +
				$"('{fname}', '{lname}', {ssn}, '{today}', '{gender}', '{password}', '{bd}', 'Giza', 'Giza', '{email}', '{user_type}')\n" +
				"DECLARE @NewUserID INT = SCOPE_IDENTITY(); \n" +
				"INSERT INTO Patient(ID, SSNValidation, PenaltyFees)\n" +
				"VALUES(@NewUserID, 0, 0);\n" +
				"COMMIT TRANSACTION;";
			}
			else if (user_type == "d")
			{
				queryString = "BEGIN TRANSACTION \n" +
				"INSERT INTO[user] \n" +
				"(FName, LName, SSN, RegistrationDate, Gender, [Password], BirthDate, City, Governorate, Email, [type]) \n" +
				"VALUES " +
				$"('{fname}', '{lname}', {ssn}, '{today}', '{gender}', '{password}', '{bd}', 'Giza', 'Giza', '{email}', '{user_type}')\n" +
				"DECLARE @NewUserID INT = SCOPE_IDENTITY(); \n" +
				"INSERT INTO Doctor(ID, PricePA, SSNValidation, Banned, FieldCode)\n" +
				$"VALUES(@NewUserID, 0, 0, 0, {field_code});\n" +
				"COMMIT TRANSACTION;";
			}
			else
			{
				return false;
			}
            SqlCommand cmd = new SqlCommand(queryString, con);

			try
			{
				con.Open();
				cmd.ExecuteReader();
			}
			catch (Exception ex)
			{
				Console.Write(ex.ToString());
				return false;
			}
			finally
			{
				con.Close();
			}

			return true;
		}
		
		public string isValidLogin(string email, string password)
		{
			string queryString = $"Select * from [user] Where Email = '{email}' and [password] = '{password}'";
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
			if (dt.Rows.Count == 1 && dt.Rows[0]["Email"].ToString() == email)
			{
                return dt.Rows[0]["type"].ToString()!;
			}

			return "";
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
