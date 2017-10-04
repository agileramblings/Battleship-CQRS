using System;
using System.Threading.Tasks;
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

        public Task Save(AggregateBase aggregate, int expectedVersion)
        {
            _store.Put(aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);
            return Task.FromResult(0);
        }

        public async Task<T> GetById<T>(Guid id) where T : AggregateBase, new()
        {
            var obj = new T();
            var e = await _store.GetEventsForAggregate(id);
            obj.LoadsFromHistory(e);
            return obj;
        }
    }
}