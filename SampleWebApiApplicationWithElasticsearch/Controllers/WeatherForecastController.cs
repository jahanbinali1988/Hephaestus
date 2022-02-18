using Hephaestus.Repository.Abstraction.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleWebApiApplicationWithElasticsearch.Models;
using SampleWebApiApplicationWithElasticsearch.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithElasticsearch.Controllers
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
        private readonly SampleElasticDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public WeatherForecastController(SampleElasticDbContext context, IUnitOfWork unitOfWork, ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var person = PersonEntity.Create("Ali", "Jahanbin");
            _context.AddDocument<PersonEntity, long>(person);
            _unitOfWork.CommitAsync();

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
