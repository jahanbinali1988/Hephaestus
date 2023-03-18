using Hephaestus.Repository.Abstraction.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleWebApiApplicationWithElasticsearch.ViewModels;
using SampleWebApiApplicationWithElasticsearch.Persistence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SampleWebApiApplicationWithElasticsearch.Models;
using Mapster;
using Hephaestus.Repository.Abstraction.Exceptions;
using Hephaestus.Repository.Elasticsearch.Contract;

namespace SampleWebApiApplicationWithElasticsearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonRepository _personRepository;
        private readonly IElasticsearchUnitOfWork _unitOfWork;
        public PersonController(IElasticsearchUnitOfWork unitOfWork, ILogger<PersonController> logger, IPersonRepository personRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _personRepository = personRepository;
        }

        [HttpPost]
        public async Task<ActionResult<PersonViewModel>> Create([FromBody] CreatePersonRequest request)
        {
            var person = PersonEntity.Create(request.FirstName, request.LastName);
            await _personRepository.AddAsync(person, CancellationToken.None);
            await _unitOfWork.SaveChangesAsync();

            return Created($"/Person/{person.Id}", person.Adapt<PersonViewModel>());
        }

        [HttpPut]
        public async Task<ActionResult<PersonViewModel>> Update([FromQuery] Guid id, [FromBody] CreatePersonRequest request)
        {
            var person = await _personRepository.GetAsync(id, CancellationToken.None);
            if (person == null)
                throw new EntityNotFoundException($"Unable to find Person with ID '{id}'");

            person.Update(request.FirstName, request.LastName);
            await _personRepository.UpdateAsync(person, CancellationToken.None);
            await _unitOfWork.SaveChangesAsync();

            return person.Adapt<PersonViewModel>();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var person = await _personRepository.GetAsync(id, CancellationToken.None);
            if (person == null)
                throw new EntityNotFoundException($"Unable to find Person with ID '{id}'");

            person.Delete();
            await _personRepository.DeleteAsync(person, CancellationToken.None);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonViewModel>> Get(Guid id)
        {
            var person = await _personRepository.GetAsync(id, CancellationToken.None);

            return person.Adapt<PersonViewModel>();
        }

        [HttpGet()]
        public async Task<IEnumerable<PersonViewModel>> GetList()
        {
            var persons = await _personRepository.GetListAsync(CancellationToken.None);

            return persons.Adapt<IEnumerable<PersonViewModel>>();
        }
    }
}
