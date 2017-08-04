using System;
using System.Collections.Generic;
using Battleship.Domain.ReadModel;

namespace Battleship.Domain.CQRS.Persistence
{
    public interface IReadModelFacade
    {
        ICollection<T> GetAll<T>() where T : ReadModelBase, new();
        T Get<T>(Guid id) where T : ReadModelBase, new();
    }
}