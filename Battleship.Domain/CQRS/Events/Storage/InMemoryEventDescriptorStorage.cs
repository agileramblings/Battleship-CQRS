using System;
using System.Collections.Generic;

namespace Battleship.Domain.CQRS.Events.Storage
{
    public class InMemoryEventDescriptorStorage : IEventDescriptorStorage
    {
        readonly Dictionary<Guid, List<EventDescriptor>> _descriptors = new Dictionary<Guid, List<EventDescriptor>>();

        public bool GetEventDescriptors(Guid aggregateId, out List<EventDescriptor> eventDescriptors)
        {
            eventDescriptors = new List<EventDescriptor>();
            if (!_descriptors.ContainsKey(aggregateId))
            {
                return false;
            }
            eventDescriptors = _descriptors[aggregateId];
            return true;
        }

        public void AddDescriptor(EventDescriptor ed)
        {
            if (!_descriptors.ContainsKey(ed.Id))
            {
                _descriptors.Add(ed.Id, new List<EventDescriptor>());
            }
            _descriptors[ed.Id].Add(new EventDescriptor(ed.Id, ed.EventData, ed.Version));

        }

        public void AddDescriptors(IEnumerable<EventDescriptor> eds)
        {
            throw new NotImplementedException();
        }
    }
}