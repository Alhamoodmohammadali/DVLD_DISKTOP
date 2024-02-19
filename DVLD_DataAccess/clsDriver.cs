using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_DataAccess.clsCountryData;
using System.Net;
using System.Security.Policy;
using System.Windows.Forms;
using System.Diagnostics;
using static DVLD_DataAccess.clsDataAccessSettings;
namespace DVLD_DataAccess
{
    public class clsDriverData
    {

        public static bool GetDriverInfoByDriverID(int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                PersonID = (int)reader["PersonID"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];
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
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return isFound;
        }
        public static bool GetDriverInfoByPersonID(int PersonID, ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";

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

                                DriverID = (int)reader["DriverID"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];
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
        public static DataTable GetAllDrivers()
        {

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Drivers_View order by FullName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
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
                    }
                }
            }
            return dt;
        }
        public static int AddNewDriver(int PersonID, int CreatedByUserID)
        {
            int DriverID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"Insert Into Drivers (PersonID,CreatedByUserID,CreatedDate)
                            Values (@PersonID,@CreatedByUserID,@CreatedDate);
                          
                            SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            DriverID = insertedID;
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
            return DriverID;

        }
        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID)
        {

            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                //we dont update the createddate for the driver.
                string query = @"Update  Drivers  
                            set PersonID = @PersonID,
                                CreatedByUserID = @CreatedByUserID
                                where DriverID = @DriverID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
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
                        return false;
                    }
                }
            }
            return (rowsAffected > 0);
        }

    }
}
