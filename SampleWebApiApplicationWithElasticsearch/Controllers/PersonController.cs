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
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly SampleElasticDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public PersonController(SampleElasticDbContext context, IUnitOfWork unitOfWork, ILogger<PersonController> logger)
        {
            _logger = logger;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreatePersonRequest request)
        {
            var person = PersonEntity.Create(request.FirstName, request.LastName);
            _context.AddDocument<PersonEntity, long>(person);
            await _unitOfWork.CommitAsync();

            return Created($"/Person/{person.Id}", person);
        }
    }
}
