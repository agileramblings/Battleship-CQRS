using System.Threading.Tasks;
using Battleship.Domain.Core.DDD;

namespace Battleship.Domain.Core.Services.Persistence.EventSource.Snapshot;

public interface ISnapshotStrategy<T> where T : AggregateBase, new()
{
    Task<SnapshotResponse> GetEventsForAggregateAsync(string aggregateId);
}