using System;
using System.Configuration;


namespace DVLD_DataAccess
{
    public static class clsDataAccessSettings
    {
       public static string ConnectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;
       public static string sourceName = "DVLD_App";
    }
}
