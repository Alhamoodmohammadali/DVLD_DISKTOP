using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_DataAccess.clsDataAccessSettings;
using System.Windows.Forms;
using System.Diagnostics;
namespace DVLD_DataAccess
{
    public class clsCountryData
    {
        public enum enGendor { Male = 0, Female = 1 };
        public static bool GetCountryInfoByID(int ID, ref string CountryName)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Countries WHERE CountryID = @CountryID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryID", ID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                CountryName = (string)reader["CountryName"];
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
        public static bool GetCountryInfoByName(string CountryName, ref int ID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Countries WHERE CountryName = @CountryName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryName", CountryName);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;
                                ID = (int)reader["CountryID"];
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
        public static DataTable _GetAllCountries()
        {

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Countries order by CountryName";

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

    }
}
