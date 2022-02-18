using Hephaestus.Repository.Abstraction.Base;
using System;

namespace Hephaestus.Repository.Abstraction.Contract
{
    public interface IExposedDomainEvent<TEntity> where TEntity : Entity
    {
        string Subject { get; }
        TEntity Payload { get; }
        DateTimeOffset OccurredOn { get; }
    }
}
