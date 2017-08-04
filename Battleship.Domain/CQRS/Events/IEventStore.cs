using System;
using System.Collections.Generic;
using Battleship.Domain.CQRS;

namespace Battleship.Domain.Events
{
    public interface IEventStore
    {
        void Put(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
        List<Event> GetEventsForAggregate(Guid aggregateId);
    }
}