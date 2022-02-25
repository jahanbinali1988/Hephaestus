using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using SampleWebApiApplicationWithElasticsearch.Models.DomainEvent;
using System;

namespace SampleWebApiApplicationWithElasticsearch.Models
{
    public class PersonEntity : Entity, IAggregateRoot
    {
        public PersonEntity(Guid id, string firstName, string lastName)
        {
            base.Id = id;
            base.CreatedAt = DateTimeOffset.Now;
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
            var id = Guid.NewGuid();
            return new PersonEntity(id, firstName, lastName);
        }

        public void Update(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            base.MarkAsUpdated();
        }

        public void Delete()
        {
            MarkAsDeleted();
        }
    }
}
