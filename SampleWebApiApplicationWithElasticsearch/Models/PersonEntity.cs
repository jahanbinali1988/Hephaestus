using Hephaestus.Repository.Abstraction.Base;
using SampleWebApiApplicationWithElasticsearch.Models.DomainEvent;
using System;

namespace SampleWebApiApplicationWithElasticsearch.Models
{
    public class PersonEntity : Entity
    {
        public PersonEntity(string firstName, string lastName)
        {
            base.Id = 1;
            base.UniqId = Guid.NewGuid();
            UpdateFirstName(firstName);
            UpdateLastName(lastName);

            AddDomainEvent(new CreatePersonDomainEvent(base.UniqId));
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
