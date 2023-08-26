using System.Threading.Tasks;
using Battleship.Domain.Core.DDD;

namespace Battleship.Domain.Core.Services.Persistence.CQRS;

public interface IReadModelPersistence
{
    Task PutAsync<T>(T t, string? partitionKeyValue = null) where T : TypeBasedProjectionBase;
    Task PutAggregateAsync<T>(T t, string? partitionKeyValue = null) where T : ProjectionBase;
    Task DeleteAsync<T>(T t, string? partitionKeyValue = null) where T : TypeBasedProjectionBase;
    Task DeleteAggregateAsync<T>(T t, string? partitionKeyValue = null) where T : ProjectionBase;
}