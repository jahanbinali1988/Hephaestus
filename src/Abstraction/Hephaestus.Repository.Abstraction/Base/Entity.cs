﻿using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.Abstraction.Exceptions;
using Hephaestus.Repository.Abstraction.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Abstraction.Base
{
    public abstract class Entity
    {
        private List<IDomainEvent> _domainEvents;
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= new List<IDomainEvent>();
            this._domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        protected static async Task CheckRule(IBusinessRule rule)
        {
            if (await rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule, rule.Properties, rule.ErrorType);
            }
        }

        protected void MarkAsUpdated()
        {
            ModifiedAt = SystemClock.Now;
        }

        protected void MarkAsDeleted()
        {
            ModifiedAt = SystemClock.Now;
            IsDeleted = true;
        }

        public Guid Id { get; protected set; }

        public DateTimeOffset? ModifiedAt { get; protected set; }

        public DateTimeOffset CreatedAt { get; protected set; }

        public bool IsDeleted { get; internal set; }

    }
}
