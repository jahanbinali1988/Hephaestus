﻿using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using SampleWebApiApplicationWithElasticsearch.Models.DomainEvent;
using System;

namespace SampleWebApiApplicationWithElasticsearch.Models
{
    public class PersonEntity : Entity, IAggregateRoot
    {
        public PersonEntity(string firstName, string lastName)
        {
            Random random = new Random();

            base.Id = random.Next();
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
