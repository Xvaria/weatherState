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
        public static void Connection(ref WeatherStatus json)
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
                    String sql = "DROP DATABASE IF EXISTS [WeatherStatus]; IF NOT EXISTS (SELECT * FROM sys.databases WHERE name='WeatherStatus') CREATE DATABASE [WeatherStatus]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Done.");
                    }

                    // Create a Table and insert some sample data
                    Console.Write("Creating sample table with data, press any key to continue...");
                    // Console.ReadKey(true);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("USE WeatherStatus; ");
                    sb.Append("IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Weather' and xtype='U') CREATE TABLE Weather ( ");
                    sb.Append(" IdCity NVARCHAR(250) NOT NULL PRIMARY KEY, ");
                    sb.Append(" City NVARCHAR(250) NOT NULL, ");
                    sb.Append(" Country NVARCHAR(250) NOT NULL, ");
                    sb.Append(" Status NVARCHAR(250) NOT NULL, ");
                    sb.Append(" Description NVARCHAR(250) NOT NULL, ");
                    sb.Append(" Temp DECIMAL(4,2) NOT NULL, ");
                    sb.Append(" FeelsLike DECIMAL(4,2) NOT NULL, ");
                    sb.Append(" Pressure INT NOT NULL, ");
                    sb.Append(" Humidity INT NOT NULL ");
                    sb.Append("); ");
                    sb.Append("IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='News' and xtype='U') CREATE TABLE News ( ");
                    sb.Append(" IdNews NVARCHAR(250) NOT NULL PRIMARY KEY, ");
                    sb.Append(" Name NVARCHAR(250) NOT NULL, ");
                    sb.Append(" Author NVARCHAR(250) NOT NULL, ");
                    sb.Append(" Title NVARCHAR(250) NOT NULL, ");
                    sb.Append(" Published DATETIME NOT NULL, ");
                    sb.Append(" Description NVARCHAR(4000) NOT NULL, ");
                    sb.Append(" Url NVARCHAR(4000) NOT NULL, ");
                    sb.Append(" UrlImage NVARCHAR(4000) NOT NULL, ");
                    sb.Append(" IdCity NVARCHAR(250) FOREIGN KEY REFERENCES Weather(IdCity) ");
                    sb.Append("); ");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Done.");
                    }

                    // INSERT Weather
                    Console.Write("Inserting a new row into table, press any key to continue...");
                    // Console.ReadKey(true);
                    sb.Clear();
                    Guid cityId = Guid.NewGuid();
                    sb.Append("INSERT Weather (IdCity, City, Country, Status, Description, Temp, FeelsLike, Pressure, Humidity) ");
                    sb.Append("VALUES (@idCity, @city, @country, @status, @description, @temp, @feelsLike, @pressure, @humidity);");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@idCity", $"{cityId}");
                        command.Parameters.AddWithValue("@city", $"{json.name}");
                        command.Parameters.AddWithValue("@country", $"{json.country}");
                        command.Parameters.AddWithValue("@status", $"{json.status}");
                        command.Parameters.AddWithValue("@description", $"{json.description}");
                        command.Parameters.AddWithValue("@temp", json.temp);
                        command.Parameters.AddWithValue("@feelsLike", json.feels_like);
                        command.Parameters.AddWithValue("@pressure", json.pressure);
                        command.Parameters.AddWithValue("@humidity", json.humidity);
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " row(s) inserted");
                    }

                    // INSERT News
                    for (int i = 0; i < json.citynews.Count; i++)
                    {
                        Console.Write("Inserting a new row into table, press any key to continue...");
                        // Console.ReadKey(true);
                        sb.Clear();
                        Guid newsId = Guid.NewGuid();
                        sb.Append("INSERT News (IdNews, Name, Author, Title, Published, Description, Url, UrlImage, IdCity) ");
                        sb.Append("VALUES (@idNews, @name, @author, @title, @published, @description, @url, @urlImage, @idCity);");
                        sql = sb.ToString();
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@idNews", $"{newsId}");
                            command.Parameters.AddWithValue("@name", $"{json.citynews[i].name}");
                            command.Parameters.AddWithValue("@author", $"{json.citynews[i].author}");
                            command.Parameters.AddWithValue("@title", $"{json.citynews[i].title}");
                            command.Parameters.AddWithValue("@published", json.citynews[i].publishedAt);
                            command.Parameters.AddWithValue("@description", $"{json.citynews[i].description}");
                            command.Parameters.AddWithValue("@url", $"{json.citynews[i].url}");
                            command.Parameters.AddWithValue("@urlImage", $"{json.citynews[i].urlToImage}");
                            command.Parameters.AddWithValue("@idCity", $"{cityId}");
                            int rowsAffected = command.ExecuteNonQuery();
                            Console.WriteLine(rowsAffected + " row(s) inserted");
                        }
                    }

                    // READ
                    Console.WriteLine("Reading data from table, press any key to continue...");
                    // Console.ReadKey(true);
                    sql = "SELECT Name, IdCity FROM News;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                            }
                        }
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
