using Hephaestus.Repository.Abstraction.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleWebApiApplicationWithMongoDb.Models;
using SampleWebApiApplicationWithMongoDb.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithMongoDb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PersonController(IPersonRepository personRepository, IUnitOfWork unitOfWork, ILogger<PersonController> logger)
        {
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreatePersonRequest request)
        {
            var person = PersonEntity.Create(request.FirstName, request.LastName);
            await _personRepository.AddAsync(person, CancellationToken.None);
            await _unitOfWork.CommitAsync();

            return Created($"/Person/{person.Id}", person);
        }
    }
}
