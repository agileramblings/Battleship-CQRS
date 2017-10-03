using System;
using System.Collections.Generic;
using Battleship.Domain.ReadModel;

namespace Battleship.Domain.CQRS.Persistence
{
    public class InMemoryReadModelStorage : IReadModelFacade, IReadModelPersistence
    {
        readonly Dictionary<Type, Dictionary<Guid, ReadModelBase>> _storage = new Dictionary<Type,Dictionary<Guid, ReadModelBase>>();

        public IEnumerable<T> GetAll<T>() where T : ReadModelBase, new()
        {
            var type = typeof(T);
            if (!_storage.ContainsKey(type))
            {
                return new List<T>();
            }
            return _storage[type] as ICollection<T>;
        }

        public T Get<T>(Guid id) where T : ReadModelBase, new()
        {
            var type = typeof(T);
            if (!_storage.ContainsKey(type))
            {
                return null;
            }
            return _storage[type][id] as T;
        }

        public void Put<T>(T t) where T : ReadModelBase
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
        }

        public void Delete<T>(Guid id) where T : ReadModelBase
        {
            var type = typeof(T);
            if (!_storage.ContainsKey(type))
            {
                return;
            }
            if (_storage[type].ContainsKey(id))
            {
                _storage[type].Remove(id);
            }
        }
    }
}