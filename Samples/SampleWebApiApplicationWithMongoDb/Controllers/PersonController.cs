using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.Abstraction.Exceptions;
using Hephaestus.Repository.MongoDB.Contract;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleWebApiApplicationWithMongoDb.Models;
using SampleWebApiApplicationWithMongoDb.Persistence;
using SampleWebApiApplicationWithMongoDb.ViewModels;
using System;
using System.Collections.Generic;
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
        private readonly IMongoDbUnitOfWork _unitOfWork;
        public PersonController(IPersonRepository personRepository, IMongoDbUnitOfWork unitOfWork, ILogger<PersonController> logger)
        {
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
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

            return Created($"/Person/{person.Id}", person.Adapt<PersonViewModel>());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromQuery] Guid id)
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
        public async Task<PersonViewModel> Get([FromQuery] Guid id)
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

        [HttpPost("{id}/order/{orderId}")]
        public async Task Post([FromRoute] Guid id, [FromRoute] Guid orderId, [FromBody] CreatePersonRequest request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetAsync(id, CancellationToken.None);
            if (person == null)
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                try
                {
                    person = PersonEntity.Create(request.FirstName, request.LastName);
                    await _personRepository.AddAsync(person, cancellationToken);

                    await _unitOfWork.CommitTransactionAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    _logger.Log(LogLevel.Information, $"Could not find person with the given id {id}");
                }
            }

            person.AddOrder(orderId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
