using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Net;

namespace DotNet.Parsers
{
    public class GreenpeaceParser: IParser
    {
        private string url = "https://maps.greenpeace.org/airpollution/load_markers.php";

        public void Parse(MySqlConnection mysql_connection)
        {
            try
            {
                string response;
                using (WebClient client = new WebClient())
                    response = client.DownloadString(url);

                var featureCol = JsonConvert.DeserializeObject<FeatureCollection>(response);
                foreach (var feature in featureCol.Features)
                {
                    MySqlCommand mysql_query = mysql_connection.CreateCommand();
                    mysql_query.CommandText = "SELECT latitude, longitude, date FROM message " +
                        $"WHERE latitude = '{feature.Geometry.Coordinates[1].ToString().Replace(",", ".")}' AND " +
                        $"longitude = '{feature.Geometry.Coordinates[0].ToString().Replace(",", ".")}' AND " +
                        $"date = '{feature.Properties.Date_created.Split(' ')[0]}';";
                    var dataReader = mysql_query.ExecuteReader();
                    bool noContent = !dataReader.HasRows;
                    dataReader.Close();
                    if (noContent)
                    {
                        mysql_query.CommandText = "INSERT INTO message (latitude, longitude, date, region, city, problem, comments)" +
                            $"VALUES ('{feature.Geometry.Coordinates[1].ToString().Replace(",", ".")}', " +
                            $"'{feature.Geometry.Coordinates[0].ToString().Replace(",", ".")}', " +
                            $"'{feature.Properties.Date_created.Split(' ')[0]}'," +
                            $"'{feature.Properties.Region}', " +
                            $"'{feature.Properties.City}', " +
                            $"'{feature.Properties.Problem}'," +
                            $" '{feature.Properties.Comments}');";
                        mysql_query.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"DOTNET ERROR: {ex.Message}");
            }
        }
    }
}
