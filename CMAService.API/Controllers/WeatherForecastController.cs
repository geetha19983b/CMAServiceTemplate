﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
//#if(AddRedis)
using Microsoft.Extensions.Caching.Distributed;
//#endif
namespace CMAService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        static int _counter = 0;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
         #if (AddRedis)
        private readonly IDistributedCache _distributedCache;
            #endif
        public WeatherForecastController(ILogger<WeatherForecastController> logger
        #if (AddRedis)
            , IDistributedCache distributedCache 
        #endif
            )
        {
            _logger = logger;
#if (AddRedis)
            _distributedCache = distributedCache;
#endif
        }
        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        [Route("GetValues")]
        public ActionResult<IEnumerable<string>> GetValues()
        {
            return new string[] { "value1", "value2" };
        }
        /// <summary>
        /// Get weather forecast details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
#if (AddRedis)
        /// <summary>
        /// Redis Cache Testing
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTime")]
        public string GetTimeInfo()
        {
            var cacheKey = "TheTime";
            var existingTime = _distributedCache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(existingTime))
            {
                return "Fetched from cache : " + existingTime;
            }
            else
            {
                existingTime = DateTime.UtcNow.ToString();
                _distributedCache.SetString(cacheKey, existingTime);
                return "Added to cache : " + existingTime;
            }
        }
#endif
        /// <summary>
        /// Polly retry test end point
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetErrorPollyTest")]
        public ActionResult Get(int id)
        {
            var rng = new Random();
            _counter++;

            if (_counter % 4 == 0) // only one out of four requests will succeed
            {
                var res = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                });
                return Ok(res);

            }
            return StatusCode((int)HttpStatusCode.BadRequest, "Something went wrong when getting the temperature.");

        }
    }
}
