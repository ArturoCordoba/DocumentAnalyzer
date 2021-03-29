using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using DataHandlerSQL;
using DataHandlerSQL.Factory;
using DataHandlerSQL.Repository;

namespace AuthAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _logger = logger;
            
            _unitOfWork = unitOfWorkFactory.Create();
        }

        [HttpGet]
        [AllowAnonymous]
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

        [Route("claim")]
        [HttpGet]
        public IActionResult GetX()
        {
            ClaimsPrincipal claims = HttpContext.User;

            return Ok(claims.FindFirst(ClaimTypes.Email).Value);

        }

        [Route("data")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetData()
        {
            IRepository<Employee> employeeRep = _unitOfWork.GetRepository<Employee>();
            Employee employee = employeeRep.GetById(2);

            return Ok(employee.FullName);
        }
    }
}
