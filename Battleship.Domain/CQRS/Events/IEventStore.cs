using System;
using System.Collections.Generic;

namespace Battleship.Domain.CQRS.Events
{
    public interface IEventStore
    {
        void Put(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
        IEnumerable<Event> GetEventsForAggregate(Guid aggregateId);
    }
}