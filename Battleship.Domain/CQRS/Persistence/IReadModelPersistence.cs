using System;
using Battleship.Domain.ReadModel;

namespace Battleship.Domain.CQRS.Persistence
{
    public interface IReadModelPersistence
    {
        void Put<T>(T t) where T : ReadModelBase;
        void Delete<T>(Guid id) where T : ReadModelBase;
    }
}