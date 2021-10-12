using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroService.TaskCenter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            Console.WriteLine("Get------------------");
            Thread.Sleep(1000);
            _logger.LogInformation($"【TaskCenter】Get------------------【{DateTime.Now}】");
            return Ok();
        }


        [HttpGet("health")]
        public IActionResult Health() => Ok("ok");
    }
}
