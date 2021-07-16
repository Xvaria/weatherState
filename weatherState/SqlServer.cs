using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text;

namespace weatherState
{
    public class SqlServer
    {
        public static void Connection()
        {
            try
            {
                Console.WriteLine("Connect to SQL Server and demo Create, Read, Update and Delete operations.");

                // Build connection string
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "localhost";   // update me
                builder.UserID = "Xvaria";              // update me
                builder.Password = "Punkyfab231993";      // update me
                builder.InitialCatalog = "master";

                // Connect to SQL
                Console.Write("Connecting to SQL Server ... ");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("Done.");

                    // Create a sample database
                    Console.Write("Dropping and creating database 'SampleDB' ... ");
                    String sql = "CREATE DATABASE IF NOT EXISTS [WeatherState]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Done.");
                    }

                    // Create a Table and insert some sample data
                    Console.Write("Creating sample table with data, press any key to continue...");
                    // Console.ReadKey(true);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("USE WeatherState; ");
                    sb.Append("CREATE TABLE IF NOT EXISTS Cities ( ");
                    sb.Append(" Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, ");
                    sb.Append(" City NVARCHAR(50) NOT NULL, ");
                    sb.Append(" Country NVARCHAR(50) NOT NULL, ");
                    sb.Append(" Status NVARCHAR(50) NOT NULL, ");
                    sb.Append(" Description NVARCHAR(50) NOT NULL, ");
                    sb.Append(" Temp NVARCHAR(50) NOT NULL, ");
                    sb.Append(" FeelsLike NVARCHAR(50) NOT NULL, ");
                    sb.Append(" Pressure NVARCHAR(50) NOT NULL, ");
                    sb.Append(" Humidity NVARCHAR(50) NOT NULL ");
                    sb.Append("); ");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Done.");
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
