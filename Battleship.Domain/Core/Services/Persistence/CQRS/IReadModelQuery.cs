using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Battleship.Domain.Core.DDD;

namespace Battleship.Domain.Core.Services.Persistence.CQRS;

public interface IReadModelQuery
{
    Task<IEnumerable<T>> GetItemsAsync<T>(Guid ownerId, string query) where T : ReadModelBase, new();
    Task<T?> GetTypedItemAsync<T>(Guid ownerId, string? partitionKeyValue = null) where T : TypeBasedProjectionBase, new();
    Task<T?> GetItemAsync<T>(Guid ownerId, string aggregateId, string? partitionKeyValue = null) where T : ProjectionBase, new();
    Task<int> GetItemCount<T>(Guid ownerId, string whereClause) where T : ProjectionBase, new();

    Task<string?> GetRawJsonAsync<T>(Guid ownerId) where T : TypeBasedProjectionBase, new();
    Task<string?> GetRawJsonAsync<T>(Guid ownerId, string aggregateId) where T : ProjectionBase, new();
    Task<IEnumerable<T>?> GetRawJsonAllAsync<T>(Guid ownerId, string query) where T : ReadModelBase, new();
}