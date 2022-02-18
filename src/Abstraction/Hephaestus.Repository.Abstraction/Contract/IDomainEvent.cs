using System;
using MediatR;

namespace Hephaestus.Repository.Abstraction.Contract
{
    public interface IDomainEvent : INotification
    {
        DateTimeOffset OccurredOn { get; }
    }
}
