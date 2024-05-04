using Npgsql;
using WeatherCheck.Data.Interfaces;
using WeatherCheck.Domain.Entities;

namespace WeatherCheck.Data.Repositories
{
    public class WeatherRepository: IWeatherRepository
    {
        private string ConnectionString;

        public WeatherRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task<List<Weather>> GetWeatherHistoryAsync(string cityName)
        {
            List<Weather> weatherDataList = new List<Weather>();

            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT id, temp_c, temp_f, wind_mph, wind_kph, humidity, cloud, name, region, country, local_time, date_created " +
                                          "FROM weather_data " +
                                          "WHERE name = @cityName " +
                                          "ORDER BY local_time DESC";

                        // Add parameters to the SQL command
                        cmd.Parameters.AddWithValue("cityName", cityName);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                // Read data from the reader and create WeatherData objects
                                Weather weatherData = new Weather
                                {
                                    Id = reader.GetGuid(0),
                                    Temp_c = reader.GetDouble(1),
                                    Temp_f = reader.GetDouble(2),
                                    Wind_mph = reader.GetDouble(3),
                                    Wind_kph = reader.GetDouble(4),
                                    Humidity = reader.GetDouble(5),
                                    Cloud = reader.GetDouble(6),
                                    Name = reader.GetString(7),
                                    Region = reader.GetString(8),
                                    Country = reader.GetString(9),
                                    LocalTime = reader.GetDateTime(10),
                                    DateCreated = reader.GetDateTime(11)
                                };

                                // Add the WeatherData object to the list
                                weatherDataList.Add(weatherData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving weather data: {ex.Message}");
            }

            return weatherDataList;
        }

        public async Task SaveWeatherDataAsync(Weather weatherData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = "INSERT INTO weather_data (id, temp_c, temp_f, wind_mph, wind_kph, humidity, cloud, name, region, country, local_time, date_created) " +
                                          "VALUES (@id, @temp_c, @temp_f, @wind_mph, @wind_kph, @humidity, @cloud, @name, @region, @country, @local_time, @date_created)";


                        // Add parameters to the SQL command
                        cmd.Parameters.AddWithValue("id", weatherData.Id);
                        cmd.Parameters.AddWithValue("temp_c", weatherData.Temp_c);
                        cmd.Parameters.AddWithValue("temp_f", weatherData.Temp_f);
                        cmd.Parameters.AddWithValue("wind_mph", weatherData.Wind_mph);
                        cmd.Parameters.AddWithValue("wind_kph", weatherData.Wind_kph);
                        cmd.Parameters.AddWithValue("humidity", weatherData.Humidity);
                        cmd.Parameters.AddWithValue("cloud", weatherData.Cloud);
                        cmd.Parameters.AddWithValue("name", weatherData.Name);
                        cmd.Parameters.AddWithValue("region", weatherData.Region);
                        cmd.Parameters.AddWithValue("country", weatherData.Country);
                        cmd.Parameters.AddWithValue("local_time", weatherData.LocalTime);
                        cmd.Parameters.AddWithValue("date_created", weatherData.DateCreated);

                        // Execute the INSERT statement
                        await cmd.ExecuteNonQueryAsync();

                        // Clear parameters for the next iteration
                        cmd.Parameters.Clear();

                    }
                }

                Console.WriteLine("Data inserted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving weather data: {ex.Message}");
            }
        }
    }
}