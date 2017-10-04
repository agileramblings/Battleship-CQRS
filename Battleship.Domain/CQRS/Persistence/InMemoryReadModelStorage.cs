using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Battleship.Domain.ReadModel;

namespace Battleship.Domain.CQRS.Persistence
{
    public class InMemoryReadModelStorage : IReadModelFacade, IReadModelPersistence
    {
        private readonly Dictionary<Type, Dictionary<Guid, ReadModelBase>> _storage = new Dictionary<Type,Dictionary<Guid, ReadModelBase>>();

        public Task<IEnumerable<T>> GetAll<T>() where T : ReadModelBase, new()
        {
            var type = typeof(T);
            var retval = new List<T>() as IEnumerable<T>;
            if (!_storage.ContainsKey(type))
            {
                return Task.FromResult(retval);
            }
            return Task.FromResult(_storage[type] as IEnumerable<T>);
        }

        public Task<T> Get<T>(Guid id) where T : ReadModelBase, new()
        {
            var type = typeof(T);
            if (!_storage.ContainsKey(type))
            {
                return null;
            }
            return Task.FromResult(_storage[type][id] as T);
        }

        public Task Put<T>(T t) where T : ReadModelBase
        {
            var type = typeof(T);
            if (!_storage.ContainsKey(type))
            {
                _storage.Add(type, new Dictionary<Guid, ReadModelBase>());
            }
            if (_storage[type].ContainsKey(t.Id))
            {
                // update existing readmodel
                _storage[type][t.Id] = t;
            }
            else
            {
                // add new readmodel
                _storage[type].Add(t.Id, t);
            }
            return Task.FromResult(0);
        }

        public Task Delete<T>(Guid id) where T : ReadModelBase
        {
            var type = typeof(T);
            if (!_storage.ContainsKey(type))
            {
                return Task.FromResult(0);
            }
            if (_storage[type].ContainsKey(id))
            {
                _storage[type].Remove(id);
            }
            return Task.FromResult(0);
        }
    }
}