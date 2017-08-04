using System;

namespace Battleship.Domain.CQRS.Events.Storage
{
    public class EventDescriptorEntity
    {
        public EventDescriptorEntity()
        {
        }

        public EventDescriptorEntity(Guid aggregateId, int version)
        {
        }

        public string EventData { get; set; }
        public string EventType { get; set; }
    }
}