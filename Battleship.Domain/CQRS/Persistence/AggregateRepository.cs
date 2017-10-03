using System;
using Battleship.Domain.CQRS.Events;

namespace Battleship.Domain.CQRS.Persistence
{
    public class AggregateRepository : IAggregateRepository
    {
        private readonly IEventStore _store;

        public AggregateRepository(IEventStore store)
        {
            _store = store;
        }

        public void Save(AggregateBase aggregate, int expectedVersion)
        {
            _store.Put(aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);
        }

        public T GetById<T>(Guid id) where T : AggregateBase, new()
        {
            var obj = new T();
            var e = _store.GetEventsForAggregate(id);
            obj.LoadsFromHistory(e);
            return obj;
        }
    }
}