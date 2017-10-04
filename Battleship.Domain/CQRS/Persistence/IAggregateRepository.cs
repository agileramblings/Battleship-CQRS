using System;
using System.Threading.Tasks;

namespace Battleship.Domain.CQRS.Persistence
{
    public interface IAggregateRepository
    {
        Task Save(AggregateBase aggregate, int expectedVersion);
        Task<T> GetById<T>(Guid id) where T : AggregateBase, new();
    }
}