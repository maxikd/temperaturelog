using System.Collections.Generic;
using WebAPI.Entities;

namespace WebAPI.Repositories
{
    interface IRepository
    {
        /// <summary>
        /// Adds a city.
        /// </summary>
        bool AddCity(City city);
        /// <summary>
        /// Gets a city by it's id.
        /// </summary>
        City RetrieveCity(string id);
        /// <summary>
        /// Gets all the cities.
        /// </summary>
        IEnumerable<City> RetrieveCities();
        /// <summary>
        /// Gets a collection of cities.
        /// </summary>
        /// <param name="page">Current page.</param>
        /// <param name="itemsPerPage">Max items per page.</param>
        /// <param name="pagesTotal">Total pages.</param>
        /// <param name="itemsTotal">Total number of items.</param>
        IEnumerable<City> RetrieveCities(int page, int itemsPerPage, out int pagesTotal, out int itemsTotal);
        /// <summary>
        /// Deletes a city by it's id..
        /// </summary>
        bool DeleteCity(string id);

        /// <summary>
        /// Add a temeprature.
        /// </summary>
        bool AddTemperature(Temperature temperature);
        /// <summary>
        /// Gets all the temperatures.
        /// </summary>
        IEnumerable<Temperature> RetrieveTemperature();
        /// <summary>
        /// Gets all the temperatures of a city.
        /// </summary>
        IEnumerable<Temperature> RetrieveTemperature(string cityId);
        /// <summary>
        /// Deletes the temperatures of a city.
        /// </summary>
        bool DeleteTemperature(string cityId);
    }
}
