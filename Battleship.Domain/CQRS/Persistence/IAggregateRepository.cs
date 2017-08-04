using System;
using Battleship.Domain.CQRS;

namespace Battleship.Domain.Persistence
{
    public interface IAggregateRepository
    {
        void Save(AggregateBase aggregate, int expectedVersion);
        T GetById<T>(Guid id) where T : AggregateBase, new();
    }
}