using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using SampleWebApiApplicationWithMongoDb.Models.DomainEvent;
using System;
using System.Collections.Generic;

namespace SampleWebApiApplicationWithMongoDb.Models
{
    public class PersonEntity : Entity, IAggregateRoot
    {
        public PersonEntity(string firstName, string lastName)
        {
            base.Id = Guid.NewGuid();
            base.CreatedAt = DateTimeOffset.Now;
            UpdateFirstName(firstName);
            UpdateLastName(lastName);

            AddDomainEvent(new CreatePersonDomainEvent(base.Id));
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }


        private List<OrderEntity> _orders = new ();
        public IReadOnlyCollection<OrderEntity> Orders => _orders.AsReadOnly();

        private void UpdateFirstName(string firstName) { this.FirstName = firstName; }
        private void UpdateLastName(string lastName) { this.LastName = lastName; }

        public static PersonEntity Create(string firstName, string lastName)
        {
            return new PersonEntity(firstName, lastName);
        }

        public void Update(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            base.MarkAsUpdated();
        }

        public void Delete()
        {
            base.MarkAsDeleted();
        }

        public void AddOrder(Guid orderId)
        {
            var order = OrderEntity.Create(orderId);
            _orders.Add(order);
        }
    }
}
