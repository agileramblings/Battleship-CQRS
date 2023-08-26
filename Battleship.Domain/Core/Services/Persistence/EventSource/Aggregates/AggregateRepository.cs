using System.Collections.Generic;
using System.Threading.Tasks;
using Battleship.Domain.Core.DDD;
using Battleship.Domain.Core.DDD.Exceptions;
using Battleship.Domain.Core.Messaging;
using Battleship.Domain.Core.Services.Persistence.EventSource.Snapshot;
using Microsoft.Extensions.Logging;

namespace Battleship.Domain.Core.Services.Persistence.EventSource.Aggregates;

public class AggregateRepository<T> : IAggregateRepository<T> where T : AggregateBase, new()
{
    private readonly Correlation _correlation;
    private readonly ILogger _logger;
    private readonly ISnapshotStrategy<T> _snapshotStrategy;
    private readonly IEventStore<T> _store;
    private readonly IOwnershipContext _ownerContext;

    public AggregateRepository(IEventStore<T> store, ISnapshotStrategy<T> snapshotStrategy, Correlation correlation, IOwnershipContext ownerContext,
        ILogger<AggregateRepository<T>> logger)
    {
        _store = store;
        _snapshotStrategy = snapshotStrategy;
        _correlation = correlation;
        _logger = logger;
        _ownerContext = ownerContext;
    }

    public async Task SaveAsync(T aggregate, int expectedVersion, bool failOnConcurrency = true, bool batchSave = false)
    {
        LogAction(nameof(AggregateRepository<T>), nameof(SaveAsync));
        await _store.PutAsync(aggregate.AggregateId, aggregate, aggregate.GetUncommittedChanges(), expectedVersion,
            failOnConcurrency, batchSave);
    }

    public async Task<T> GetAsync(string aggregateId)
    {
        LogAction(nameof(AggregateRepository<T>), nameof(GetAsync));
        var obj = new T();
        SnapshotResponse ssr;
        try
        {
            ssr = await _snapshotStrategy.GetEventsForAggregateAsync(aggregateId);
        }
        catch (AggregateNotFoundException)
        {
            _logger.LogDebug("Attempt to acquire aggregate failed. {aggregateId} was not found", aggregateId);
            throw;
        }

        obj.LoadsFromHistory(ssr.Events);

        if (ssr.ShouldSnapshot && obj is ISnapshotable snapshotable)
            snapshotable.TakeSnapshot(_correlation.Id);

        return obj;
    }

    public async Task<IEnumerable<string>> GetAllAggregateIdsAsync(int page = 1, int count = int.MaxValue)
    {
        LogAction(nameof(AggregateRepository<T>), nameof(GetAllAggregateIdsAsync));
        return await _store.GetAggregateIdsAsync(page, count);
    }

    public async Task RepublishEventsForAggregate(string aggregateId)
    {
        LogAction(nameof(AggregateRepository<T>), nameof(RepublishEventsForAggregate));
        await _store.RepublishEventsForAggregate(aggregateId);
    }

    private void LogAction(string component, string operation)
    {
        _logger.LogDebug("{Component} {Operation} executed", component, operation);
    }
}