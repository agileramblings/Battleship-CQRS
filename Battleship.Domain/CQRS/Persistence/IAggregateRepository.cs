using System;

namespace Battleship.Domain.CQRS.Persistence
{
    public interface IAggregateRepository
    {
        void Save(AggregateBase aggregate, int expectedVersion);
        T GetById<T>(Guid id) where T : AggregateBase, new();
    }
}