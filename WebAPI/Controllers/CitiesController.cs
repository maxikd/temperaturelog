using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Entities;
using WebAPI.Helpers;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private IRepository _repository;

        public CitiesController(IConfiguration configuration)
        {
            _repository = new Repository(configuration.GetConnectionString("TemperatureLog"));
        }

        /// <summary>
        /// Adds a new city.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AddCity([FromBody] string postalCode)
        {
            if (string.IsNullOrEmpty(postalCode)) return BadRequest();

            try
            {
                var response = await HttpHelper.Client.GetAsync(string.Format("https://viacep.com.br/ws/{0}/json/", postalCode));
                if (!response.IsSuccessStatusCode)
                    return BadRequest();

                var address = await SerializerHelper.Deserialize<Address>(response);

                var city = new City
                {
                    CreatedOn = DateTime.UtcNow,
                    FederativeUnity = address.State,
                    Name = address.City,
                    PostalCode = address.PostalCode
                };

                bool added = _repository.AddCity(city);

                var result = new Result
                {
                    Data = new
                    {
                        Key = city.Id,
                        city.Name,
                        city.PostalCode,
                        city.CreatedOn
                    },
                    Success = added
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Unknown error.");
            }
        }

        /// <summary>
        /// Deletes the specified city.
        /// </summary>
        [HttpDelete("{key}")]
        public ActionResult DeleteCity(string key)
        {
            if (string.IsNullOrEmpty(key)) return BadRequest();

            try
            {
                bool deleted = _repository.DeleteCity(key);

                var result = new Result { Success = deleted };
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Unknown error.");
            }
        }

        /// <summary>
        /// Retrieves the cities.
        /// </summary>
        /// <param name="page">The current page.</param>
        [HttpGet]
        public ActionResult RetrieveCity([FromBody] int page)
        {
            try
            {
                const int itemsPerPage = 100;
                int pagesTotal, itemsTotal;

                var cities = _repository.RetrieveCities(page, itemsPerPage, out pagesTotal, out itemsTotal);

                var result = new Result
                {
                    Data = cities.Select(c => new
                    {
                        Key = c.Id,
                        c.Name,
                        c.PostalCode,
                        c.CreatedOn
                    }),
                    Paging = new Paging
                    {
                        Page = page,
                        RecordsPerPage = cities.Count(),
                        TotalPages = pagesTotal,
                        TotalRecords = itemsTotal
                    }
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Unknown error.");
            }
        }

        /// <summary>
        /// Adds a new temperature to the specified city.
        /// </summary>
        [HttpPost("{key}/temperatures")]
        public async Task<ActionResult> AddTemperature(string key)
        {
            if (string.IsNullOrEmpty(key)) return BadRequest();

            try
            {
                var city = _repository.RetrieveCity(key);
                if (city == null) throw new ArgumentOutOfRangeException(nameof(key), key, "No city was found with the informed key.");

                var response = await HttpHelper.Client.GetAsync(
                    string.Format("https://api.hgbrasil.com/weather?key=f5280c5e&fields=only_results,temp&city_name={0},{1}",
                    city.Name,
                    city.FederativeUnity)
                    );

                var forecast = await SerializerHelper.Deserialize<Forecast>(response);

                var entity = new Temperature
                {
                    CityId = key,
                    CreatedOn = DateTime.UtcNow,
                    Value = forecast.Temperature
                };

                bool added = _repository.AddTemperature(entity);

                var result = new Result
                {
                    Data = new
                    {
                        City = city.Name,
                        forecast.Temperature,
                        entity.CreatedOn
                    },
                    Success = added
                };

                return Ok(result);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return StatusCode(500, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Unknown error.");
            }
        }

        /// <summary>
        /// Retrieves the temperatures for the specified city.
        /// </summary>
        [HttpGet("{key}/temperatures")]
        public ActionResult RetrieveTemperatures(string key)
        {
            if (string.IsNullOrEmpty(key)) return BadRequest();

            try
            {
                var city = _repository.RetrieveCity(key);
                var temperatures = _repository.RetrieveTemperature(key);

                var result = new Result
                {
                    Data = new
                    {
                        City = city.Name,
                        Temperatures = temperatures.Select(t => new { Temperature = t.Value, t.CreatedOn })
                    }
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Unknown error.");
            }
        }

        /// <summary>
        /// Retrieves the temperatures for all cities.
        /// </summary>
        [HttpGet("temperatures")]
        public ActionResult RetrieveTemperatures()
        {
            try
            {
                var cities = _repository.RetrieveCities().ToDictionary(k => k.Id, v => v.Name);
                var temperaturesByCity = _repository.RetrieveTemperature().ToLookup(k => k.CityId);

                var resultList = new List<CityTemperatureResultListItem>();
                foreach (var item in temperaturesByCity)
                {
                    var resultItem = new CityTemperatureResultListItem
                    {
                        City = cities[item.Key],
                        Temperatures = item.OrderBy(o => o.CreatedOn).Select(t => new { Temperature = t.Value, t.CreatedOn })
                    };
                    resultList.Add(resultItem);
                }

                var result = new Result
                {
                    Data = resultList.OrderBy(o => o.City)
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Unknown error.");
            }
        }

        /// <summary>
        /// Deletes the temperature history for the specified city.
        /// </summary>
        [HttpDelete("{key}/temperatures")]
        public ActionResult DeleteTemperatures(string key)
        {
            if (string.IsNullOrEmpty(key)) return BadRequest();

            try
            {
                bool deleted = _repository.DeleteTemperature(key);

                var result = new Result
                {
                    Success = deleted
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Unknown error.");
            }
        }
    }
}