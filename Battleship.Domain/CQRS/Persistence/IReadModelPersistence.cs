using System;
using System.Threading.Tasks;
using Battleship.Domain.ReadModel;

namespace Battleship.Domain.CQRS.Persistence
{
    public interface IReadModelPersistence
    {
        Task Put<T>(T t) where T : ReadModelBase;
        Task Delete<T>(Guid id) where T : ReadModelBase;
    }
}