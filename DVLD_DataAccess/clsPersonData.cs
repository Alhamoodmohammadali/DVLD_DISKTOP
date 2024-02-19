using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using static DVLD_DataAccess.clsDataAccessSettings;
using System.Windows.Forms;

namespace DVLD_DataAccess
{
    public class clsPersonData
    {
        public static bool GetPersonInfoByID(int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref string NationalNo, ref DateTime? DateOfBirth, ref short? Gendor, ref string Address, ref string Phone, ref string Email, ref int? NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM People WHERE PersonID = @PersonID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;
                                FirstName = (string)reader["FirstName"];
                                SecondName = (string)reader["SecondName"];
                                //ThirdName: allows null in database so we should handle null
                                LastName = (string)reader["LastName"];
                                NationalNo = (string)reader["NationalNo"];
                                DateOfBirth = (DateTime)reader["DateOfBirth"];
                                Gendor = (byte)reader["Gendor"];
                                Address = (string)reader["Address"];
                                Phone = (string)reader["Phone"];
                                //Email: allows null in database so we should handle null
                                ThirdName = (string)reader["ThirdName"] ?? null;
                                Email = (string)reader["Email"] ?? null;
                                NationalityCountryID = (int)reader["NationalityCountryID"];
                                //ImagePath: allows null in database so we should handle null
                                if (reader["ImagePath"] != DBNull.Value)
                                {
                                    ImagePath = (string)reader["ImagePath"];
                                }
                            }
                            else
                            {
                                // The record was not found
                                isFound = false;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (!EventLog.SourceExists(sourceName))
                        {
                            EventLog.CreateEventSource(sourceName, "Application");
                        }
                        EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                        // Display error message using MessageBox
                        MessageBox.Show($"An error occurred while interacting with the database. Please check the event log for more details.\n\nError: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isFound = false;
                    }
                }
            }
            return isFound;
        }







        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int? PersonID, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime? DateOfBirth, ref short? Gendor, ref string Address, ref string Phone, ref string Email, ref int? NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM People WHERE NationalNo = @NationalNo";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                PersonID = (int)reader["PersonID"];
                                FirstName = (string)reader["FirstName"];
                                SecondName = (string)reader["SecondName"];
                                ThirdName = (string)reader["ThirdName"] ?? null;
                                LastName = (string)reader["LastName"];
                                DateOfBirth = (DateTime)reader["DateOfBirth"];
                                Gendor = (byte)reader["Gendor"];
                                Address = (string)reader["Address"];
                                Phone = (string)reader["Phone"];
                                Email = (string)reader["Email"] ?? null;
                                NationalityCountryID = (int)reader["NationalityCountryID"];
                                if (reader["ImagePath"] != DBNull.Value)
                                {
                                    ImagePath = (string)reader["ImagePath"];
                                }
                            }
                            else
                            {
                                isFound = false;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (!EventLog.SourceExists(sourceName))
                        {
                            EventLog.CreateEventSource(sourceName, "Application");
                        }
                        EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                        // Display error message using MessageBox
                        MessageBox.Show($"An error occurred while interacting with the database. Please check the event log for more details.\n\nError: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isFound = false;
                    }
                }
            }
            return isFound;
        }
       
        
        
        
        public static int AddNewPerson(string FirstName, string SecondName, string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth, short Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            //this function will return the new person id if succeeded and -1 if not.
            int PersonID = -1;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = @"INSERT INTO People (FirstName, SecondName, ThirdName,LastName,NationalNo,
                                                   DateOfBirth,Gendor,Address,Phone, Email, NationalityCountryID,ImagePath)
                             VALUES (@FirstName, @SecondName,@ThirdName, @LastName, @NationalNo,
                                     @DateOfBirth,@Gendor,@Address,@Phone, @Email,@NationalityCountryID,@ImagePath);
                             SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);
                    command.Parameters.AddWithValue("@ThirdName", (object)ThirdName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@Gendor", Gendor);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Email", (object)Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NationalityCountryID",NationalityCountryID);
                    command.Parameters.AddWithValue("@ImagePath", (object)ImagePath ?? DBNull.Value);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            PersonID = insertedID;
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (!EventLog.SourceExists(sourceName))
                        {
                            EventLog.CreateEventSource(sourceName, "Application");
                        }
                        EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                        // Display error message using MessageBox
                        MessageBox.Show($"An error occurred while interacting with the database. Please check the event log for more details.\n\nError: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return PersonID;
        }
      
     
        public static bool UpdatePerson(int PersonID, string FirstName, string SecondName, string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth, short Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {

            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = @"Update  People  
                            set FirstName = @FirstName,
                                SecondName = @SecondName,
                                ThirdName = @ThirdName,
                                LastName = @LastName, 
                                NationalNo = @NationalNo,
                                DateOfBirth = @DateOfBirth,
                                Gendor=@Gendor,
                                Address = @Address,  
                                Phone = @Phone,
                                Email = @Email, 
                                NationalityCountryID = @NationalityCountryID,
                                ImagePath =@ImagePath
                                where PersonID = @PersonID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);
                    command.Parameters.AddWithValue("@ThirdName", (object)ThirdName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@Gendor", Gendor);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Email", (object)Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NationalityCountryID",NationalityCountryID );
                    command.Parameters.AddWithValue("@ImagePath", (object)ImagePath ?? DBNull.Value);
                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (!EventLog.SourceExists(sourceName))
                        {
                            EventLog.CreateEventSource(sourceName, "Application");
                        }
                        EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                        // Display error message using MessageBox
                        MessageBox.Show($"An error occurred while interacting with the database. Please check the event log for more details.\n\nError: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return false;
                    }
                }
            }


            return (rowsAffected > 0);
        }
      
        
        
        public static DataTable _GetAllPeople()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query =
              @"SELECT People.PersonID, People.NationalNo,
              People.FirstName, People.SecondName, People.ThirdName, People.LastName,
			  People.DateOfBirth, People.Gendor,  
				  CASE
                  WHEN People.Gendor = 0 THEN 'Male'

                  ELSE 'Female'

                  END as GendorCaption ,
			  People.Address, People.Phone, People.Email, 
              People.NationalityCountryID, Countries.CountryName, People.ImagePath
              FROM            People INNER JOIN
                         Countries ON People.NationalityCountryID = Countries.CountryID
                ORDER BY People.FirstName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (!EventLog.SourceExists(sourceName))
                        {
                            EventLog.CreateEventSource(sourceName, "Application");
                        }
                        EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                        // Display error message using MessageBox
                        MessageBox.Show($"An error occurred while interacting with the database. Please check the event log for more details.\n\nError: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return dt;
        }
        
        



        public static bool _DeletePerson(int PersonID)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = @"Delete People 
                                where PersonID = @PersonID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (!EventLog.SourceExists(sourceName))
                        {
                            EventLog.CreateEventSource(sourceName, "Application");
                        }
                        EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                        // Display error message using MessageBox
                        MessageBox.Show($"An error occurred while interacting with the database. Please check the event log for more details.\n\nError: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return (rowsAffected > 0);
        }
        
        
        
        
        public static bool IsPersonExist(int PersonID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT Found=1 FROM People WHERE PersonID = @PersonID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                            isFound = reader.HasRows;
                    }
                    catch (SqlException ex)
                    {
                        if (!EventLog.SourceExists(sourceName))
                        {
                            EventLog.CreateEventSource(sourceName, "Application");
                        }
                        EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                        // Display error message using MessageBox
                        MessageBox.Show($"An error occurred while interacting with the database. Please check the event log for more details.\n\nError: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isFound = false;
                    }
                }
            }
            return isFound;
        }
        
        
        
        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT Found=1 FROM People WHERE NationalNo = @NationalNo";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            isFound = reader.HasRows;
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (!EventLog.SourceExists(sourceName))
                        {
                            EventLog.CreateEventSource(sourceName, "Application");
                        }
                        EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                        // Display error message using MessageBox
                        MessageBox.Show($"An error occurred while interacting with the database. Please check the event log for more details.\n\nError: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isFound = false;
                    }
                }
            }
            return isFound;
        }
    
    
    
    }
}
