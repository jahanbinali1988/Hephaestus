using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using SampleWebApiApplicationWithMongoDb.Models.DomainEvent;
using System;

namespace SampleWebApiApplicationWithMongoDb.Models
{
    public class PersonEntity : Entity, IAggregateRoot
    {
        public PersonEntity(string firstName, string lastName)
        {
            base.Id = Guid.NewGuid();
            UpdateFirstName(firstName);
            UpdateLastName(lastName);

            AddDomainEvent(new CreatePersonDomainEvent(base.Id));
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        private void UpdateFirstName(string firstName) { this.FirstName = firstName; }
        private void UpdateLastName(string lastName) { this.LastName = lastName; }

        public static PersonEntity Create(string firstName, string lastName)
        {
            return new PersonEntity(firstName, lastName);
        }
    }
}
