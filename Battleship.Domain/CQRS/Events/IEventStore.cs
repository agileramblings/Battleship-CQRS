using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Battleship.Domain.CQRS.Events
{
    public interface IEventStore
    {
        Task Put(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
        Task<IEnumerable<Event>> GetEventsForAggregate(Guid aggregateId);
    }
}