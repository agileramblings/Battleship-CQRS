using System;
using System.Collections.Generic;

namespace Battleship.Domain.CQRS.Events.Storage
{
    public interface IEventDescriptorStorage
    {
        bool GetEventDescriptors(Guid aggregateId, out List<EventDescriptor> eventDescriptors);
        void AddDescriptor(EventDescriptor ed);
        void AddDescriptors(IEnumerable<EventDescriptor> eds);
    }
}