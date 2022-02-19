using Hephaestus.Repository.Abstraction.Contract;
using System;

namespace SampleWebApiApplicationWithMongoDb.Models.DomainEvent
{
    public class CreatePersonDomainEvent : IDomainEvent
    {
        public CreatePersonDomainEvent(Guid uniqueId)
        {
            this.UniqueId = uniqueId;
        }
        public DateTimeOffset OccurredOn => DateTimeOffset.Now;
        public Guid UniqueId { get; set; }
    }
}
