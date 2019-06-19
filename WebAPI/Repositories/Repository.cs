using DB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WebAPI.Entities;

namespace WebAPI.Repositories
{
    public class Repository : IRepository
    {
        private Connection _connection;

        public Repository(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString), "The connection string cannot be null or empty.");

            _connection = new Connection(connectionString);
        }

        public bool AddCity(City city)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT COUNT(*) FROM Cities WHERE [PostalCode] = @PostalCode;");
            int affectedRows = Convert.ToInt32(_connection.ExecuteScalar(query, "@PostalCode", city.PostalCode));
            if (affectedRows > 0) return false;

            query.Clear();
            query.Append("INSERT INTO Cities([Id], [Name], [FederativeUnity], [PostalCode], [CreatedOn], [Deleted]) VALUES (@Id, @Name, @FederativeUnity, @PostalCode, @CreatedOn, @Deleted);");
            affectedRows = _connection.ExecuteNonQuery(query,
                "@Id", city.Id,
                "@Name", city.Name,
                "@FederativeUnity", city.FederativeUnity,
                "@PostalCode", city.PostalCode,
                "@CreatedOn", city.CreatedOn,
                "@Deleted", city.Deleted
                );

            return affectedRows > 0;
        }

        public bool AddTemperature(Temperature temperature)
        {
            StringBuilder query = new StringBuilder();
            query.Append("INSERT INTO Temperatures ([Id], [CityId], [Value], [CreatedOn], [Deleted]) VALUES (@Id, @CityId, @Value, @CreatedOn, @Deleted);");
            int affectedRows = _connection.ExecuteNonQuery(query,
                 "@Id", temperature.Id,
                 "@CityId", temperature.CityId,
                 "@Value", temperature.Value,
                 "@CreatedOn", temperature.CreatedOn,
                 "@Deleted", temperature.Deleted
                 );

            return affectedRows > 0;
        }

        public bool DeleteCity(string id)
        {
            StringBuilder query = new StringBuilder();
            query.Append("UPDATE Cities SET [Deleted] = 1 WHERE [Id] = @Id;");
            int affectedRows = _connection.ExecuteNonQuery(query, "@Id", id);
            return affectedRows > 0;
        }

        public bool DeleteTemperature(string cityId)
        {
            StringBuilder query = new StringBuilder();
            query.Append("UPDATE Temperatures SET [Deleted] = 1 WHERE [CityId] = @CityId;");
            int affectedRows = _connection.ExecuteNonQuery(query, "@CityId", cityId);
            return affectedRows > 0;
        }

        public IEnumerable<City> RetrieveCities()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [Id], [Name], [FederativeUnity], [PostalCode], [CreatedOn], [Deleted] FROM Cities WHERE [Deleted] = 0;");
            return _connection.ExecuteReader(query, ReadCities);
        }

        public IEnumerable<City> RetrieveCities(int page, int itemsPerPage, out int pagesTotal, out int itemsTotal)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT COUNT(*) FROM Cities WHERE [Deleted] = 0;");
            int items = Convert.ToInt32(_connection.ExecuteScalar(query));

            pagesTotal = items % itemsPerPage == 0 ? items / itemsPerPage : items / itemsPerPage + 1;
            itemsTotal = items;

            query.Clear();
            query.Append("SELECT [Id], [Name], [FederativeUnity], [PostalCode], [CreatedOn], [Deleted] FROM Cities WHERE [Deleted] = 0 ORDER BY [CreatedOn] DESC OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;");
            return _connection.ExecuteReader(query, ReadCities,
                "@CreatedOn", DateTime.UtcNow.AddDays(-1),
                "@Skip", (page - 1) * itemsPerPage,
                "@Take", itemsPerPage
                );
        }

        public City RetrieveCity(string id)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [Id], [Name], [FederativeUnity], [PostalCode], [CreatedOn], [Deleted] FROM Cities WHERE [Id] = @Id;");
            return _connection.ExecuteReader(query, ReadCity, "@Id", id);
        }

        public IEnumerable<Temperature> RetrieveTemperature()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [Id], [CityId], [Value], [CreatedOn], [Deleted] FROM Temperatures WHERE [Deleted] = 0 AND [CreatedOn] > @CreatedOn;");
            return _connection.ExecuteReader(query, ReadTemperatures,
                "@CreatedOn", DateTime.UtcNow.AddDays(-1)
                );
        }

        public IEnumerable<Temperature> RetrieveTemperature(string cityId)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [Id], [CityId], [Value], [CreatedOn], [Deleted] FROM Temperatures WHERE [Deleted] = 0 AND [CreatedOn] > @CreatedOn AND [CityId] = @CityId ORDER BY [CreatedOn] ASC;");
            return _connection.ExecuteReader(query, ReadTemperatures,
                "@CreatedOn", DateTime.UtcNow.AddDays(-1),
                "@CityId", cityId
                );
        }

        #region Readers
        /// <summary>
        /// Reads cities from the reader.
        /// </summary>
        private IEnumerable<City> ReadCities(SqlDataReader reader)
        {
            var cities = new List<City>();

            while (reader.Read())
                cities.Add(FillCityData(reader));

            return cities;
        }
        /// <summary>
        /// Reads temperatures from the reader.
        /// </summary>
        private IEnumerable<Temperature> ReadTemperatures(SqlDataReader reader)
        {
            var temperatures = new List<Temperature>();

            while (reader.Read())
                temperatures.Add(FillTemperatureData(reader));

            return temperatures;
        }
        /// <summary>
        /// Reads a city from the reader.
        /// </summary>
        private City ReadCity(SqlDataReader reader)
        {
            if (!reader.Read())
                return null;

            return FillCityData(reader);
        }
        /// <summary>
        /// Reads a temperature from the reader.
        /// </summary>
        private Temperature ReadTemperature(SqlDataReader reader)
        {
            if (!reader.Read())
                return null;

            return FillTemperatureData(reader);
        }
        /// <summary>
        /// Fills a city from the database.
        /// </summary>
        private City FillCityData(SqlDataReader reader)
        {
            City city = new City(Convert.ToString(reader["Id"]))
            {
                CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                Deleted = Convert.ToBoolean(reader["Deleted"]),
                FederativeUnity = Convert.ToString(reader["FederativeUnity"]),
                Name = Convert.ToString(reader["Name"]),
                PostalCode = Convert.ToString(reader["PostalCode"])
            };

            return city;
        }
        /// <summary>
        /// Fills a temperature from the database.
        /// </summary>
        private Temperature FillTemperatureData(SqlDataReader reader)
        {
            Temperature temperature = new Temperature(Convert.ToString(reader["Id"]))
            {
                CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                Deleted = Convert.ToBoolean(reader["Deleted"]),
                Value = (float)Convert.ToDouble(reader["Value"]),
                CityId = Convert.ToString(reader["CityId"])
            };

            return temperature;
        }
        #endregion
    }
}
