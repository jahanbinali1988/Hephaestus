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

namespace SampleWebApiApplicationWithElasticsearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PersonController(IUnitOfWork unitOfWork, ILogger<PersonController> logger, IPersonRepository personRepository)
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
            await _unitOfWork.CommitAsync();

            return Created($"/Person/{person.Id}", person.Adapt<PersonViewModel>());
        }

        [HttpPut]
        public async Task<ActionResult<PersonViewModel>> Update([FromQuery] Guid id, [FromBody] CreatePersonRequest request)
        {
            var person = await _personRepository.GetAsync(id, CancellationToken.None);
            person.Update(request.FirstName, request.LastName);
            await _personRepository.UpdateAsync(person, CancellationToken.None);
            await _unitOfWork.CommitAsync();

            return person.Adapt<PersonViewModel>();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromQuery] Guid id)
        {
            var person = await _personRepository.GetAsync(id, CancellationToken.None);
            await _personRepository.DeleteAsync(person, CancellationToken.None);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonViewModel>> Get([FromQuery] Guid id)
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
