using System.Threading.Tasks;
using Battleship.Domain.Core.DDD;

namespace Battleship.Domain.Core.Services.Persistence.EventSource.Snapshot;

public class DefaultSnapshotStrategy<T> : ISnapshotStrategy<T> where T : AggregateBase, new()
{
    protected IEventStore<T> _store;

    public DefaultSnapshotStrategy(IEventStore<T> store)
    {
        _store = store;
    }

    public virtual async Task<SnapshotResponse> GetEventsForAggregateAsync(string aggregateId)
    {
        return new SnapshotResponse
        {
            Events = await _store.GetEventsForAggregateAsync(aggregateId),
            ShouldSnapshot = false
        };
    }
}