﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Battleship.Domain.Core.DDD;

namespace Battleship.Domain.Core.Services.Persistence.EventSource.Aggregates;

public interface IAggregateRepository<T> where T : AggregateBase, new()
{
    Task SaveAsync(T aggregate, int expectedVersion, bool failOnConcurrency = true, bool batchSave = false);
    Task<T> GetAsync(string aggregateId);
    Task<IEnumerable<string>> GetAllAggregateIdsAsync(int page, int count);
    Task RepublishEventsForAggregate(string aggregateId);
}