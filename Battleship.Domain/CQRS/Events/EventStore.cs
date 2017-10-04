using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleship.Domain.CQRS.Events.Storage;
using Battleship.Domain.CQRS.Exceptions;

namespace Battleship.Domain.CQRS.Events
{
    public class EventStore : IEventStore
    {
        private readonly IEventDescriptorStorage _descriptorStorage;
        private readonly IEventPublisher _publisher;

        public EventStore(IEventPublisher publisher, IEventDescriptorStorage descriptorStorage)
        {
            _publisher = publisher;
            _descriptorStorage = descriptorStorage;
        }

        public Task Put(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            List<EventDescriptor> eventDescriptors;

            // try to get event descriptors list for given aggregate id
            // otherwise -> create empty dictionary
            if (_descriptorStorage.GetEventDescriptors(aggregateId, out eventDescriptors))
                if (eventDescriptors[eventDescriptors.Count - 1].Version != expectedVersion && expectedVersion != -1)
                    throw new ConcurrencyException(aggregateId);
            var i = expectedVersion;

            // iterate through current aggregate events increasing version with each processed event
            foreach (var @event in events)
            {
                i++;
                @event.Version = i;

                // push event to the event descriptors list for current aggregate
                _descriptorStorage.AddDescriptor(new EventDescriptor(aggregateId, @event, i));

                // publish current event to the bus for further processing by subscribers
                PublishEvent(@event);
            }
            return Task.FromResult(0);
        }


        // collect all processed events for given aggregate and return them as a list
        // used to build up an aggregate from its history (Domain.LoadsFromHistory)
        public Task<IEnumerable<Event>> GetEventsForAggregate(Guid aggregateId)
        {
            List<EventDescriptor> eventDescriptors;

            if (!_descriptorStorage.GetEventDescriptors(aggregateId, out eventDescriptors))
                throw new AggregateNotFoundException(aggregateId);

            var events = eventDescriptors
                .OrderBy(desc => desc.Version)
                .Select(desc => desc.EventData)
                .ToList();
            return Task.FromResult<IEnumerable<Event>>(events);
        }

        private void PublishEvent(Event @event)
        {
            var publishMethods = from m in typeof(IEventPublisher).GetMethods()
                where
                m.Name == "Publish" && m.ContainsGenericParameters && m.IsGenericMethod &&
                m.IsGenericMethodDefinition
                select m;
            var publishMethod = publishMethods.First();
            var genericMethod = publishMethod.MakeGenericMethod(@event.GetType());
            genericMethod.Invoke(_publisher, new object[] {@event});
        }
    }
}