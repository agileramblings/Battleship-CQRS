using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Battleship.Domain.ReadModel;

namespace Battleship.Domain.CQRS.Persistence
{
    public interface IReadModelFacade
    {
        Task<IEnumerable<T>> GetAll<T>() where T : ReadModelBase, new();
        Task<T> Get<T>(Guid id) where T : ReadModelBase, new();
    }
}