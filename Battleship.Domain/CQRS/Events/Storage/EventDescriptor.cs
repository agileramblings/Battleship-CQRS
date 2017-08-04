using System;

namespace Battleship.Domain.CQRS.Events.Storage
{
    public struct EventDescriptor
    {
        public readonly Guid Id;
        public readonly int Version;
        public readonly Event EventData;

        public EventDescriptor(Guid id,  Event eventData, int version)
        {
            EventData = eventData;
            Version = version;
            Id = id;
        }
    }
}
