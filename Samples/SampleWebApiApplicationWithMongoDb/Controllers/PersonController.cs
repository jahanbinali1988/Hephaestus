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

        [HttpPut]
        public async Task<ActionResult> Update([FromQuery] Guid id, [FromBody] CreatePersonRequest request)
        {
            var person = await _personRepository.GetAsync(id, CancellationToken.None);
            person.Update(request.FirstName, request.LastName);
            await _personRepository.UpdateAsync(person, CancellationToken.None);
            await _unitOfWork.CommitAsync();

            return Created($"/Person/{person.Id}", person);
        }

        [HttpDelete("/id")]
        public async Task<ActionResult> Delete([FromQuery] Guid id)
        {
            var person = await _personRepository.GetAsync(id, CancellationToken.None);
            await _personRepository.DeleteAsync(person, CancellationToken.None);
            await _unitOfWork.CommitAsync();

            return Created($"/Person/{person.Id}", person);
        }

        [HttpGet("/id")]
        public async Task<PersonEntity> Get([FromQuery] Guid id)
        {
            var person = await _personRepository.GetAsync(id, CancellationToken.None);

            return person;
        }

        [HttpGet()]
        public async Task<IEnumerable<PersonEntity>> GetList()
        {
            var persons = await _personRepository.GetListAsync(CancellationToken.None);

            return persons;
        }
    }
}
