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

namespace DVLD_DataAccess
{
    public class clsTestTypeData
    {

        public static bool GetTestTypeInfoByID(int TestTypeID, ref string TestTypeTitle, ref string TestDescription, ref float TestFees)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                TestTypeTitle = (string)reader["TestTypeTitle"];
                                TestDescription = (string)reader["TestTypeDescription"];
                                TestFees = Convert.ToSingle(reader["TestTypeFees"]);
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
                        Console.WriteLine("Error: " + ex.Message);
                        isFound = false;
                    }
                }
            }
            return isFound;
        }

        public static DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM TestTypes order by TestTypeID";
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
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
            return dt;
        }

        public static int AddNewTestType(string Title, string Description, float Fees)
        {
            int TestTypeID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"Insert Into TestTypes (TestTypeTitle,TestTypeTitle,TestTypeFees)
                            Values (@TestTypeTitle,@TestTypeDescription,@ApplicationFees)
                            where TestTypeID = @TestTypeID;
                            SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestTypeTitle", Title);
                    command.Parameters.AddWithValue("@TestTypeDescription", Description);
                    command.Parameters.AddWithValue("@ApplicationFees", Fees);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            TestTypeID = insertedID;
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
            return TestTypeID;
        }

        public static bool UpdateTestType(int TestTypeID, string Title, string Description, float Fees)
        {

            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"Update  TestTypes  
                            set TestTypeTitle = @TestTypeTitle,
                                TestTypeDescription=@TestTypeDescription,
                                TestTypeFees = @TestTypeFees
                                where TestTypeID = @TestTypeID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@TestTypeTitle", Title);
                    command.Parameters.AddWithValue("@TestTypeDescription", Description);
                    command.Parameters.AddWithValue("@TestTypeFees", Fees);
                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
            return (rowsAffected > 0);
        }


    }
}
