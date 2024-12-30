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
using System.Reflection.Metadata;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlDataReader = Microsoft.Data.SqlClient.SqlDataReader;
using SqlParameter = Microsoft.Data.SqlClient.SqlParameter;
using SqlException = Microsoft.Data.SqlClient.SqlException;
using Microsoft.Extensions.Primitives;

namespace webclinic.Models
{
	public class DB
	{
		private string connectionString;
		private SqlConnection con = new SqlConnection();

		public DB()
		{
            // change the constring before running locally here

            // adel's connection string:
            connectionString = "Data Source=DESKTOP-NQ0JKHE; Initial Catalog=clinicdb; Integrated Security=True; Trust Server Certificate = True;";

            // yassin's connection string:
            connectionString = "Data Source=AMNESIA\\SQLEXPRESS; Initial Catalog=clinicdb; Integrated Security=True; Trust Server Certificate = True;";

            con.ConnectionString = connectionString;
		}

		public DataTable getFields()
		{
			string queryString = "Select FieldName from FieldOfMedicine ORDER BY FIELDNAME ASC";
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

        public bool changePassword(string newP, string email, string type)
        {
            string queryS = "";
            if (type != "a")
            {
                queryS = $"UPDATE [user] SET [password] = '{newP}' WHERE Email = '{email}'";
            }
            else
            {
                queryS = $"UPDATE [admin] SET [password] = '{newP}' WHERE Email = '{email}'";
            }
            SqlCommand cmd = new SqlCommand(queryS, con);
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
        public bool createAdmin(string email, string password)
        {
            string today = DateTime.Today.Date.ToString("yyyy-MM-dd");
            string queryString = "INSERT INTO [admin] (Email, [Password]) " +
            $"VALUES ('{email}', '{password}')";
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

        public string getNationalIDPic(int id)
        {
            string queryS = $"select SSNPicture from [user] where id = {id}";
            SqlCommand cmd = new SqlCommand(queryS, con);
            string r = "";
            try
            {
                con.Open();
                r = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return r;
        }
        public string getDocCertPic(int id)
        {
            string queryS = $"select CertPic from DoctorCertificate where [Description] = 'MainMedicalCert' and DoctorID = {id}";
            SqlCommand cmd = new SqlCommand(queryS, con);
            string r = "";
            try
            {
                con.Open();
                r = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return r;
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
        public DataTable getdrgovernments()
        {
            string queryString = $"SELECT DISTINCT \r\n    Governorate\r\nFROM \r\n    [user]\r\nWHERE \r\n    [user].type = 'd';";
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
        public DataTable getdrspecialities()
        {
            string queryString = $"SELECT DISTINCT \r\n    FieldName\r\nFROM \r\n    Doctor join FieldOfMedicine on (Doctor.FieldCode = FieldOfMedicine.FieldCode) ORDER BY FIELDNAME ASC";
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
        public DataTable GetDoctorData()
        {
            string queryString = @"
    SELECT 
        FName, 
        LName, 
        Governorate,
        City,
        Doctor.ID, 
        PricePA,
        Doctor.FieldCode,
        FieldName, 
        ProfileImageUrl,
        COALESCE(AVG(NStars), 5) AS AverageRating
        FROM 
        [user]
        JOIN 
        Doctor ON [user].ID = Doctor.ID
    JOIN 
        FieldOfMedicine ON Doctor.FieldCode = FieldOfMedicine.FieldCode
    LEFT JOIN 
        Reviews ON Doctor.ID = Reviews.DoctorID
    WHERE 
        [user].type = 'd' and Banned = 0 and SSNValidation = 1
    GROUP BY 
        FName, 
        LName, 
        Doctor.ID, 
        PricePA, 
        Doctor.FieldCode,
        FieldName,
        Governorate,
        City,
        ProfileImageUrl";

            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(queryString, con);

            try
            {
                con.Open();
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());  // Make sure to use Console.WriteLine for better logging in C#
            }
            finally
            {
                con.Close();
            }

            return dt;
        }

        public DataTable getupcomingappointment(int id)
        {
            string queryString = $"SELECT AppointmentID, PatientID, DoctorID, u.FName, u.LName, Appointment.DatenTime, ProfileImageUrl\r\n\r\nfrom [user] as u join Patient on (u.ID = Patient.ID ) join Appointment on (Patient.ID = PatientID)\r\nwhere IsConfirmed = 1 and IsFinished = 0 and DoctorID = {id}";
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
        public DataTable getpendingappointment(int id)
        {
            string queryString = $"SELECT AppointmentID, PatientID, DoctorID, u.FName, u.LName, Appointment.DatenTime, ProfileImageUrl\r\n\r\nfrom [user] as u join Patient on (u.ID = Patient.ID ) join Appointment on (Patient.ID = PatientID)\r\nwhere IsConfirmed = 0 and IsFinished = 0 and DoctorID = {id}";
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
        public DataTable getcompletedappointment(int id)
        {
            string queryString = $"SELECT AppointmentID, PatientID, DoctorID, u.FName, u.LName, Appointment.DatenTime, ProfileImageUrl\r\n\r\nfrom [user] as u join Patient on (u.ID = Patient.ID ) join Appointment on (Patient.ID = PatientID)\r\nwhere IsConfirmed = 1 and IsFinished = 1 and DoctorID = {id}";
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
        public DataTable getcanceledappointment(int id)
        {
            string queryString = $"SELECT AppointmentID, PatientID, DoctorID, u.FName, u.LName, Appointment.DatenTime, ProfileImageUrl\r\n\r\nfrom [user] as u join Patient on (u.ID = Patient.ID ) join Appointment on (Patient.ID = PatientID)\r\nwhere IsConfirmed = 0 and IsFinished = 1 and DoctorID = {id}";
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
        public string GetAppointmentStatus(int appointmentId)
        {
            string query = @"SELECT 
        CASE 
            WHEN IsConfirmed = 0 AND IsFinished = 0 THEN 'Pending'
            WHEN IsConfirmed = 1 AND IsFinished = 0 THEN 'Confirmed'
            WHEN IsConfirmed = 1 AND IsFinished = 1 THEN 'Completed'
            WHEN IsConfirmed = 0 AND IsFinished = 1 THEN 'Cancelled'
            ELSE 'Unknown Status'
        END AS Status
    FROM Appointment
    WHERE AppointmentID = @AppointmentID";

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@AppointmentID", appointmentId);

                string status = cmd.ExecuteScalar()?.ToString() ?? "Unknown Status";
                return status;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        public void CancelAppointment(int aid)
        {
            string queryString = "UPDATE Appointment SET IsConfirmed = 0, IsFinished = 1 WHERE AppointmentID = @aid";

            try
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    // Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@aid", aid);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Appointment canceled successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No appointment found to cancel.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider logging it to a file or a logging system)
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Ensure that the connection is closed, even in case of an exception
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        public void confirmappointment(int aid)
        {
            string queryString = "UPDATE Appointment SET IsConfirmed = 1, IsFinished = 0 WHERE AppointmentID = @aid";

            try
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    // Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@aid", aid);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Appointment Confirmed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No appointment found to cancel.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider logging it to a file or a logging system)
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Ensure that the connection is closed, even in case of an exception
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        public void completeappointment(int aid)
        {
            string queryString = "UPDATE Appointment SET IsConfirmed = 1, IsFinished = 1 WHERE AppointmentID = @aid";

            try
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    // Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@aid", aid);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Appointment completed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No appointment found to cancel.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider logging it to a file or a logging system)
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Ensure that the connection is closed, even in case of an exception
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        // Patient appointment Queries
        public DataTable getpaupcomingappointment(int id)
        {
            string queryString = $"SELECT AppointmentID, PatientID, DoctorID, u.FName, u.LName, Appointment.DatenTime, ProfileImageUrl\r\n\r\nfrom [user] as u join Doctor on (u.ID = Doctor.ID ) join Appointment on (Doctor.ID = DoctorID)\r\nwhere IsConfirmed = 1 and IsFinished = 0 and PatientID = {id}";
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
        public DataTable getpapendingappointment(int id)
        {
            string queryString = $"SELECT AppointmentID, PatientID, DoctorID, u.FName, u.LName, Appointment.DatenTime, ProfileImageUrl\r\nfrom [user] as u join Doctor on (u.ID = Doctor.ID ) join Appointment on (Doctor.ID = DoctorID)\r\nwhere IsConfirmed = 0 and IsFinished = 0 and PatientID = {id}";
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
        public DataTable getpacompletedappointment(int id)
        {
            string queryString = $"SELECT AppointmentID, PatientID, DoctorID, u.FName, u.LName, Appointment.DatenTime, ProfileImageUrl\r\nfrom [user] as u join Doctor on (u.ID = Doctor.ID ) join Appointment on (Doctor.ID = DoctorID)\r\nwhere IsConfirmed = 1 and IsFinished = 1 and PatientID = {id}";
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
        public DataTable getpacanceledappointment(int id)
        {
            string queryString = $"SELECT AppointmentID, PatientID, DoctorID, u.FName, u.LName, Appointment.DatenTime, ProfileImageUrl\r\nfrom [user] as u join Doctor on (u.ID = Doctor.ID ) join Appointment on (Doctor.ID = DoctorID)\r\nwhere IsConfirmed = 0 and IsFinished = 1 and PatientID = {id}";
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
        //End of patient appointment queries

        //Add Diagnosis
        public void AddDiagnosis(int aid, int did, int pid, string cond, string des, string pre)
        {
            string query = "INSERT INTO Diagnosis (AppointmentID, DoctorID, PatientID, condition, [description], prescription) " +
                           "VALUES (@AppointmentID, @DoctorID, @PatientID, @Condition, @Description, @Prescription)";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add(new SqlParameter("@AppointmentID", SqlDbType.Int) { Value = aid });
                    cmd.Parameters.Add(new SqlParameter("@DoctorID", SqlDbType.Int) { Value = did });
                    cmd.Parameters.Add(new SqlParameter("@PatientID", SqlDbType.Int) { Value = pid });
                    cmd.Parameters.Add(new SqlParameter("@Condition", SqlDbType.NVarChar) { Value = (object)cond ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar) { Value = (object)des ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@Prescription", SqlDbType.NVarChar) { Value = (object)pre ?? DBNull.Value });

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                // Log exception
                throw new Exception("Error while inserting diagnosis data.", ex);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        // End of Add Diagnosis


        // View Diagnosis
        public DataTable getdiagnosis(int id)
        {
            string queryString = $"SELECT condition, [description], prescription, FName, LName from Diagnosis join [user] on (PatientID = ID)  where AppointmentID = {id}";
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
        // End of view diagnosis

        // Add Review
        public void AddReview(int did, int pid, int ns, string com)
        {
            string query = "INSERT INTO Reviews (DoctorID, PatientID, NStars, Comment) " +
                           "VALUES (@DoctorID, @PatientID, @NStars, @Comment)";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add(new SqlParameter("@DoctorID", SqlDbType.Int) { Value = did });
                    cmd.Parameters.Add(new SqlParameter("@PatientID", SqlDbType.Int) { Value = pid });
                    cmd.Parameters.Add(new SqlParameter("@NStars", SqlDbType.Int) { Value = ns });
                    cmd.Parameters.Add(new SqlParameter("@Comment", SqlDbType.NVarChar) { Value = (object)com ?? DBNull.Value });
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                // Log exception
                throw new Exception("Error while inserting review data.", ex);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        // End of Add Review

        // Check Dr if reviewed
        public bool CheckReviewExists(int doctorId, int patientId)
        {
            string queryString = "SELECT COUNT(1) FROM Reviews WHERE DoctorID = @DoctorID AND PatientID = @PatientID";
            bool exists = false;

            using (SqlCommand cmd = new SqlCommand(queryString, con))
            {
                cmd.Parameters.AddWithValue("@DoctorID", doctorId);
                cmd.Parameters.AddWithValue("@PatientID", patientId);

                try
                {
                    con.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int count))
                    {
                        exists = count > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }

            return exists;
        }
        // End of Check Dr if reviewed
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
            else if (checkAdmin(email, password))
            {
                return "a";
            }

			return "";
		}

        public bool checkAdmin(string email, string password)
        {
            string queryS = $"select Email from [admin] where Email = '{email}' and [Password] = '{password}'";
			SqlCommand cmd = new SqlCommand(queryS, con);
            string retEmail = "";
			try
			{
				con.Open();
                retEmail = (string)cmd.ExecuteScalar();
			}
			catch (Exception ex)
			{
				Console.Write(ex.ToString());
			}
			finally
			{
				con.Close();
			}

            return (retEmail == email);
        }

        public async Task<bool> addUser(string fname, string lname, string ssn, string password, string governorate, string city, string email, string gender, DateTime birthdate, string user_type, int field_code, IFormFile nationalIDPic, IFormFile docCertPic)
        {
            string today = DateTime.Today.Date.ToString("yyyy-MM-dd");
            string bd = birthdate.Date.ToString("yyyy-MM-dd");

            string queryString;
            if (user_type == "p")
            {
                queryString = "BEGIN TRANSACTION \n" +
                "INSERT INTO[user] \n" +
                "(FName, LName, SSN, RegistrationDate, Gender, [Password], BirthDate, City, Governorate, Email, [type], SSNValidation) \n" +
                "VALUES " +
                $"('{fname}', '{lname}', {ssn}, '{today}', '{gender}', '{password}', '{bd}', '{city}', '{governorate}', '{email}', '{user_type}', 0)\n" +
                "DECLARE @NewUserID INT = SCOPE_IDENTITY(); \n" +
                "INSERT INTO Patient(ID, PenaltyFees)\n" +
                "VALUES(@NewUserID, 0);\n" +
                "COMMIT TRANSACTION;";
            }
            else if (user_type == "d")
            {
                queryString = "BEGIN TRANSACTION \n" +
                "INSERT INTO[user] \n" +
                "(FName, LName, SSN, RegistrationDate, Gender, [Password], BirthDate, City, Governorate, Email, [type]) \n" +
                "VALUES " +
                $"('{fname}', '{lname}', {ssn}, '{today}', '{gender}', '{password}', '{bd}', '{city}', '{governorate}', '{email}', '{user_type}')\n" +
                "DECLARE @NewUserID INT = SCOPE_IDENTITY(); \n" +
                "INSERT INTO Doctor(ID, PricePA, Banned, FieldCode)\n" +
                $"VALUES(@NewUserID, 0, 0, {field_code});\n" +
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

                // recieve images from post request
                if (nationalIDPic == null || nationalIDPic.Length == 0)
                {
                    throw new InvalidOperationException("Please upload a valid file.");
                }

                // Save the Picture temporarily in the backend
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string natIDPath = Path.Combine(uploadsFolder, "NationalID_" + email + Path.GetExtension(nationalIDPic.FileName));
                using (var fileStream = new FileStream(natIDPath, FileMode.Create))
                {
                    await nationalIDPic.CopyToAsync(fileStream);
                }

                string docCertPath = "";

                if (user_type == "d")
                {
                    if (docCertPic == null || docCertPic.Length == 0)
                    {
                        throw new InvalidOperationException("Please upload a valid file.");
                    }
                    docCertPath = Path.Combine(uploadsFolder, "DocCert_" + email + Path.GetExtension(docCertPic.FileName));
                    using (var fileStream = new FileStream(docCertPath, FileMode.Create))
                    {
                        await docCertPic.CopyToAsync(fileStream);
                    }
                }

                List<string> filePaths = new List<string> { natIDPath };
                if (!string.IsNullOrEmpty(docCertPath))
                {
                    filePaths.Add(docCertPath);
                }


                // upload National ID to google drive
                string credentialsPath = ".\\bin\\Debug\\credentials.json";
                string folderID = "10_kLHobgMJhTHluPik28qiK9k3q4T0B4";
                GoogleCredential credential;

                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream).CreateScoped(new[]
                    {
                        DriveService.ScopeConstants.DriveFile
                    });
                }

                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Google drive SSN upload"
                });

                foreach (var p in filePaths)
                {
                    var fileMetaData = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = Path.GetFileName(p),
                        Parents = new List<string> { folderID }
                    };

                    FilesResource.CreateMediaUpload request;
                    using (var stream = new FileStream(p, FileMode.Open))
                    {

                        request = service.Files.Create(fileMetaData, stream, "");
                        request.Fields = "id";
                        request.Upload();
                    }
                    var uploadedFile = request.ResponseBody;
                    string link = $"https://drive.google.com/file/d/{uploadedFile.Id}/view";
                    Console.WriteLine($"File '{fileMetaData.Name}' uploaded with ID: {link}");

                    if (Path.GetFileName(p).StartsWith("National"))
                    {
                        queryString = $"UPDATE [user] SET SSNPicture = '{link}' WHERE email = '{email}';";
                        con.Close();
                        con.Open();
                        SqlCommand cmd2 = new SqlCommand(queryString, con);
                        cmd2.ExecuteReader();
                    }
                    else
                    {
                        con.Close();
                        int docid = getID(email);
                        queryString = $"Insert INTO DoctorCertificate(CertPic, DoctorID, cert_validation, [Description]) values('{link}', {docid}, 0, 'MainMedicalCert')";
                        con.Open();
                        SqlCommand cmd2 = new SqlCommand(queryString, con);
                        cmd2.ExecuteReader();
                    }

                }

                if (File.Exists(natIDPath))
                {
                    File.Delete(natIDPath);
                }
                if (File.Exists(docCertPath))
                {
                    File.Delete(docCertPath);
                }
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



        public Dictionary<string, int> getRegisteredUsers(string from, string to, string type)
		{
			string queryString;
			if (type == "dp")
			{
				queryString = $"WITH DateRange AS (SELECT CAST('{from}' AS DATE) AS [Date] UNION ALL SELECT DATEADD(DAY, 1, [Date]) FROM DateRange WHERE [Date] < '{to}' ) SELECT d.[Date], COUNT(u.RegistrationDate) AS NumUsers FROM DateRange d LEFT JOIN [user] u ON u.RegistrationDate = d.[Date] AND u.type != 'a' GROUP BY d.[Date] ORDER BY d.[Date] OPTION (MAXRECURSION 0);";
			}
			else
			{
				queryString = $"WITH DateRange AS (SELECT CAST('{from}' AS DATE) AS [Date] UNION ALL SELECT DATEADD(DAY, 1, [Date]) FROM DateRange WHERE [Date] < '{to}' ) SELECT d.[Date], COUNT(u.RegistrationDate) AS NumUsers FROM DateRange d LEFT JOIN [user] u ON u.RegistrationDate = d.[Date] AND u.type = '{type}' GROUP BY d.[Date] ORDER BY d.[Date] OPTION (MAXRECURSION 0);";
			}

			SqlCommand cmd = new SqlCommand(queryString, con);
			Dictionary<string, int> dayAndNum = new Dictionary<string, int>();
			try
			{
				con.Open();
				SqlDataReader rdr  = cmd.ExecuteReader();
				while (rdr.Read())
				{
					dayAndNum.Add(rdr["Date"].ToString()!, (int)rdr["NumUsers"]);
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


		public Dictionary<string, int> getAppAnalytics(string from, string to)
		{
			string queryString = $"WITH DateRange AS ( SELECT CAST('{from}' AS DATE) AS DatenTime UNION ALL SELECT DATEADD(DAY, 1, DatenTime) FROM DateRange WHERE DatenTime < '{to}' ) SELECT d.DatenTime AS [Date], COUNT(a.DatenTime) AS AppCount FROM DateRange d LEFT JOIN Appointment a ON CAST(a.DatenTime AS DATE) = d.DatenTime GROUP BY d.DatenTime ORDER BY d.DatenTime;";

			SqlCommand cmd = new SqlCommand(queryString, con);
			Dictionary<string, int> dayAndNum = new Dictionary<string, int>();
			try
			{
				con.Open();
				SqlDataReader rdr  = cmd.ExecuteReader();
				while (rdr.Read())
				{
					dayAndNum.Add(rdr["Date"].ToString()!, (int)rdr["AppCount"]);
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

		public Dictionary<string, int> getFieldAnalytics(string from, string to)
		{
			string queryString = $"select FieldName, Count(*) as DocN from Doctor join [user] on Doctor.ID = [user].ID join FieldOfMedicine on Doctor.FieldCode = FieldOfMedicine.FieldCode Where RegistrationDate > '{from}' and RegistrationDate < '{to}' Group By FieldName";


            SqlCommand cmd = new SqlCommand(queryString, con);
			Dictionary<string, int> fieldAndNum = new Dictionary<string, int>();
			try
			{
				con.Open();
				SqlDataReader rdr  = cmd.ExecuteReader();
				while (rdr.Read())
				{
					fieldAndNum.Add(rdr["FieldName"].ToString()!, (int)rdr["DocN"]);
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
			return fieldAndNum;
		}


        public DataTable getSymptoms(int id)
        {

            string queryString = $"(SELECT SymptomName,Severity, DateOfFirstInstance\r\nFROM Symptom, SymptomTypes\r\nWHERE symptom.SymptomID = SymptomTypes.SymptomID AND PatientID = '{id}')\r\nUNION\r\n(SELECT ConditionName, Severity, DateOfFirstInstance\r\nFROM LongTermConditions, LTCTypes\r\nWHERE LongTermConditions.ConditionID = LTCTypes.ConditionID AND PatientID = '{id}')";
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



        public DataTable getHistory(int id)
        {

            string queryString = $"SELECT Fname, Lname, condition, [description]\r\nFROM Diagnosis, [user]\r\nWHERE [user].ID = DoctorID  AND  PatientID = '{id}'";
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

        public int getAge(int id)
        {
            int age = 0;
            string today = DateTime.Today.Date.ToString("yyyy-MM-dd");
            string queryString = $"SELECT FLOOR(DATEDIFF(year, birthdate,'{today}')) AS age\r\nFROM [user]\r\nWHERE ID = '{id}'";
            SqlCommand cmd = new SqlCommand(queryString, con);
            try
            {
                con.Open();
                age = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally
            {
                con.Close();
            }

            return age;
        }

        public string getName(int id)
        {
            string name = "";
            string queryString = $"SELECT Fname + ' '  +Lname as [name]\r\nFROM [user]\r\nWHERE ID = '{id}'";

            SqlCommand cmd = new SqlCommand(queryString, con);
            try
            {
                con.Open();
                name = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally
            {
                con.Close();
            }

            return name;
        }

        public int getRating(int id)
        {
            int rating = 0;
            string queryString = $"SELECT AVG(NStars) as rating\r\nFROM Reviews\r\nWHERE DoctorID = '{id}'\r\ngroup by DoctorID";
            SqlCommand cmd = new SqlCommand(queryString, con);
            try
            {
                con.Open();
                rating = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally
            {
                con.Close();
            }

            return rating;
        }


        public int getPatientsTreated(int id)
        {
            int rating = 0;
            string queryString = $"SELECT COUNT(PatientID) as Patients_Treated\r\nFROM Appointment\r\nWHERE DoctorID = '{id}' and PatientID is not null\r\ngroup by DoctorID";
            SqlCommand cmd = new SqlCommand(queryString, con);
            try
            {
                con.Open();
                rating = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally
            {
                con.Close();
            }

            return rating;
        }

        public string getMedicalField(int id)
        {
            string field = "";
            string queryString = $"SELECT FieldName\r\nFROM Doctor, FieldOfMedicine\r\nWHERE Doctor.ID = '{id}' and FieldOfMedicine.FieldCode = Doctor.FieldCode";
            SqlCommand cmd = new SqlCommand(queryString, con);
            
            try
            {
                con.Open();
                field = (string) cmd.ExecuteScalar(); ;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally
            {
                con.Close();
            }

            return field;
        }

        public int getID(string email)
        {
            int id = 0;
            string queryString = $"Select id\r\nfrom [user]\r\nwhere email = '{email}'";
            SqlCommand cmd = new SqlCommand(queryString, con);
            try
            {
                con.Open();
                id = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return id;
        }

        public int getFee(int id)
        {
            int fee = 0;
            string queryString = $"SELECT PricePA\r\nFROM Doctor\r\nWHERE Doctor.ID = '{id}'";
            SqlCommand cmd = new SqlCommand(queryString, con);
            try
            {
                con.Open();
                fee = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return fee;
        }
        public DataTable getClinic(int id)
        {

            string queryString = $"SELECT city + ', ' +Governorate As address, Institution, JobPosition FROM DoctorCurWorkplace WHERE DoctorID = '{id}'";
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

        public DataTable getExperiance(int id)
        {

            string queryString = $"SELECT Institution, JobPosition\r\nFROM DoctorExperience\r\nWHERE DoctorID = '{id}'";
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

        public DataTable getEducation(int id)
        {

            string queryString = $"SELECT Institute, [Description]\r\nFROM DoctorCertificate\r\nWHERE DoctorID = '{id}' And cert_validation = 1";
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


        public DataTable getAllDoctors()
        {

            string queryString = $"select Fname + ' ' + Lname as [name], Banned, doctor.id, RegistrationDate, SSN, SSNValidation \r\nfrom doctor, [user]\r\nwhere doctor.id = [user].id";
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


        public DataTable getAllPatients()
        {

            string queryString = $"select Fname + ' ' + Lname as [name], SSNvalidation, patient.id, RegistrationDate, SSN \r\nfrom patient, [user]\r\nwhere patient.id = [user].id";
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
        public bool getDrStatus(int id)
        {
            bool status = false;
            string queryString = $"SELECT Banned FROM Doctor WHERE Doctor.ID = '{id}'";
            SqlCommand cmd = new SqlCommand(queryString, con);


            try
            {
                con.Open();
                status = Convert.ToBoolean(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return status;
        }


        public bool ToggleDoctorStatus(int id)
        {

            string queryString;
            queryString = $"update Doctor\r\nSET Banned = CASE \r\nWHEN Banned = 1 THEN 0\r\nELSE 1\r\nEND\r\nwhere ID = '{id}'";
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


        public bool getSSN(int id)
        {
            bool status = false;
            string queryString = $"SELECT SSNValidation FROM [user] WHERE ID = '{id}'";
            SqlCommand cmd = new SqlCommand(queryString, con);


            try
            {
                con.Open();
                status = Convert.ToBoolean(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return status;
        }



        public bool ToggleSSN(int id)
        {

            string queryString;
            queryString = $"update [user]\r\nSET SSNValidation = CASE \r\nWHEN SSNValidation = 1 THEN 0\r\nELSE 1\r\nEND\r\nwhere ID = '{id}'";
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




        public DataTable getTime(DateTime d, int id)
        {
            string queryString = $"select  AppointmentID, convert(time,(DatenTime)) as [time]\r\nfrom appointment\r\nwhere FORMAT(DatenTime, 'yyyy-MM-dd') = '{d.ToString("yyyy-MM-dd")}' and  DoctorID = '{id}'";
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


        public DataTable getAvailableTime(DateTime d, int id)
        {
            string queryString = $"select  AppointmentID, convert(time,(DatenTime)) as [time]\r\nfrom appointment\r\nwhere FORMAT(DatenTime, 'yyyy-MM-dd') = '{d.ToString("yyyy-MM-dd")}' and  DoctorID = '{id}' and PatientID is null";
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


        public DataTable getDates(int id)
        {
            string queryString = $"select distinct convert(date,(DatenTime)) as [date]\r\nfrom appointment\r\nwhere DoctorID = '{id}'";
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




        public DataTable getAvailableDates(int id)
        {
            string queryString = $"select distinct convert(date,(DatenTime)) as [date]\r\nfrom appointment\r\nwhere DoctorID = '{id}' and PatientID is null";
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


        public void GenerateAppointments(DateTime startDate, DateTime endDate, TimeSpan startHour, TimeSpan endHour, int appointmentDuration, int doctorId)
        {
            try
            {

                con.Open();

                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    DateTime currentTime = date.Date + startHour;
                    DateTime endTime = date.Date + endHour;

                    while (currentTime < endTime)
                    {
                        string checkQuery = "SELECT COUNT(*) FROM Appointment WHERE DoctorID = @DoctorID AND DatenTime = @DatenTime";
                        SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                        checkCmd.Parameters.AddWithValue("@DoctorID", doctorId);
                        checkCmd.Parameters.AddWithValue("@DatenTime", currentTime);

                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            currentTime = currentTime.AddMinutes(appointmentDuration);
                            continue;
                        }

                        string insertQuery = "INSERT INTO Appointment (DoctorID, IsFinished, IsConfirmed, DatenTime) VALUES (@DoctorID, @IsFinished, @IsConfirmed, @DatenTime)";
                        SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                        insertCmd.Parameters.AddWithValue("@DoctorID", doctorId);
                        insertCmd.Parameters.AddWithValue("@IsFinished", false);
                        insertCmd.Parameters.AddWithValue("@IsConfirmed", false);
                        insertCmd.Parameters.AddWithValue("@DatenTime", currentTime);

                        insertCmd.ExecuteNonQuery();

                        currentTime = currentTime.AddMinutes(appointmentDuration);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }


        public void bookAppointment(int PID, int DrID, DateTime date)
        {
            Console.WriteLine(date.ToString("yyyy-MM-dd HH:mm:ss"));
            string queryString = $"update Appointment set PatientID = {PID} where DoctorID = {DrID} and DatenTime = '{date.ToString("yyyy-MM-dd HH:mm:ss")}'";
            SqlCommand cmd = new SqlCommand(queryString, con);
            try
            {
                con.Open();
                cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally
            {
                con.Close();
            }

        }



        public string getimage(int id)
        {
            string x = "";
            string queryString = $"select ProfileImageUrl\r\nfrom [user]\r\nwhere ID = {id}";
            SqlCommand cmd = new SqlCommand(queryString, con);
            try
            {
                con.Open();
                x = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return x;

        }

        public bool changePrice(int id, int price)
        {

            string queryString;
            queryString = $"update Doctor\r\nset PricePA = '{price}'\r\nwhere ID = '{id}'";
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





    }
}
