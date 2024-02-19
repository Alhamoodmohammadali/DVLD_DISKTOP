using System;
using System.Data.SqlClient;
using System.Data;
using static System.Math;
using System.Windows.Forms;
using System.Diagnostics;
using static DVLD_DataAccess.clsDataAccessSettings;
namespace DVLD_DataAccess
{
    public class clsApplicationTypeData
    {
        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID, ref string ApplicationTypeTitle, ref float ApplicationFees)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;
                                ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                                ApplicationFees = Convert.ToSingle(reader["ApplicationFees"]);
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
                    }
                }
            }
            return isFound;
        }

        public static DataTable GetAllApplicationTypes()
        {

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM ApplicationTypes order by ApplicationTypeTitle";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
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

        public static int AddNewApplicationType(string Title, float Fees)
        {
            int ApplicationTypeID = -1;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = @"Insert Into ApplicationTypes (ApplicationTypeTitle,ApplicationFees)
                            Values (@Title,@Fees)
                            
                            SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeTitle", Title);
                    command.Parameters.AddWithValue("@ApplicationFees", Fees);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            ApplicationTypeID = insertedID;
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
            return ApplicationTypeID;
        }
        public static bool UpdateApplicationType(int ApplicationTypeID, string Title, float Fees)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = @"Update  ApplicationTypes  
                            set ApplicationTypeTitle = @Title,
                                ApplicationFees = @Fees
                                where ApplicationTypeID = @ApplicationTypeID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@Title", Title);
                    command.Parameters.AddWithValue("@Fees", Fees);
                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
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
