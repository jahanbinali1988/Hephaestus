using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.Abstraction.Exceptions;
using Hephaestus.Repository.Abstraction.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Abstraction.Base
{
    /// <summary>
    /// Base class for entities.
    /// </summary>
    [Serializable]
    public abstract class Entity<TKey>
    {
        private List<IDomainEvent> _domainEvents;
        private List<IExposedDomainEvent<Entity>> _exposedDomainEvents;

        /// <summary>
        /// Domain events occurred.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        /// <summary>
        /// Exposed events occurred.
        /// </summary>
        public IReadOnlyCollection<IExposedDomainEvent<Entity>> ExposedDomainEvents => _exposedDomainEvents?.AsReadOnly();

        /// <summary>
        /// Add domain event.
        /// </summary>
        /// <param name="domainEvent"></param>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= new List<IDomainEvent>();
            this._domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Add exposed event.
        /// </summary>
        /// <param name="exposedEvent"></param>
        protected void AddExposedEvent(IExposedDomainEvent<Entity> exposedEvent)
        {
            _exposedDomainEvents ??= new List<IExposedDomainEvent<Entity>>();
            this._exposedDomainEvents.Add(exposedEvent);
        }

        /// <summary>
        /// Clear domain events.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        /// <summary>
        /// Clear domain events.
        /// </summary>
        public void ClearExposedEvents()
        {
            _exposedDomainEvents?.Clear();
        }

        protected static async Task CheckRule(IBusinessRule rule)
        {
            if (await rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule, rule.Properties, rule.ErrorType);
            }
        }

        internal void MarkAsUpdated()
        {
            ModifiedAt = SystemClock.Now;
        }

        public Guid UniqId { get; protected set; }




        public TKey Id { get; protected set; }


        /// <summary>
        /// Row version
        /// </summary>
        public byte[] Version { get; protected set; }

        /// <summary>
        /// Modification date and time of this entity
        /// </summary>
        public DateTimeOffset? ModifiedAt { get; protected set; }


        public DateTimeOffset CreatedAt { get; protected set; }

    }

    public abstract class Entity : Entity<long>
    {

    }
}
