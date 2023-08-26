using Battleship.Domain.Core.DDD;

namespace Battleship.Domain.Core.Services.Persistence.CQRS
{
    public class InMemoryReadModelStorage : IReadModelQuery, IReadModelPersistence
    {
        private readonly Dictionary<Type, Dictionary<string, ReadModelBase>> _storage = new();

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

        public Task<T> Get<T>(string id) where T : ReadModelBase, new()
        {
            var type = typeof(T);
            if (!_storage.ContainsKey(type))
            {
                return null;
            }
            return Task.FromResult(_storage[type][id] as T);
        }

        public Task Put<T>(T t) where T : ProjectionBase
        {
            var type = typeof(T);
            if (!_storage.ContainsKey(type))
            {
                _storage.Add(type, new Dictionary<string, ReadModelBase>());
            }
            if (_storage[type].ContainsKey(t.AggregateId))
            {
                // update existing readmodel
                _storage[type][t.AggregateId] = t;
            }
            else
            {
                // add new readmodel
                _storage[type].Add(t.AggregateId, t);
            }
            return Task.FromResult(0);
        }

        public Task Delete<T>(string id) where T : ReadModelBase
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

        public Task<IEnumerable<T>> GetItemsAsync<T>(Guid ownerId, string query) where T : ReadModelBase, new()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetTypedItemAsync<T>(Guid ownerId, string partitionKeyValue = null) where T : TypeBasedProjectionBase, new()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetItemAsync<T>(Guid ownerId, string aggregateId, string partitionKeyValue = null) where T : ProjectionBase, new()
        {
            return Get<T>(aggregateId);
        }

        public Task<int> GetItemCount<T>(Guid ownerId, string whereClause) where T : ProjectionBase, new()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRawJsonAsync<T>(Guid ownerId) where T : TypeBasedProjectionBase, new()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRawJsonAsync<T>(Guid ownerId, string aggregateId) where T : ProjectionBase, new()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetRawJsonAllAsync<T>(Guid ownerId, string query) where T : ReadModelBase, new()
        {
            throw new NotImplementedException();
        }

        public Task PutAsync<T>(T t, string partitionKeyValue = null) where T : TypeBasedProjectionBase
        {
            var type = typeof(T);
            if (!_storage.ContainsKey(type))
            {
                _storage.Add(type, new Dictionary<string, ReadModelBase>());
            }
            if (_storage[type].ContainsKey(type.Name))
            {
                // update existing readmodel
                _storage[type][type.Name] = t;
            }
            else
            {
                // add new readmodel
                _storage[type].Add(type.Name, t);
            }
            return Task.FromResult(0);
        }

        public Task PutAggregateAsync<T>(T t, string partitionKeyValue = null) where T : ProjectionBase
        {
            var type = typeof(T);
            if (!_storage.ContainsKey(type))
            {
                _storage.Add(type, new Dictionary<string, ReadModelBase>());
            }
            if (_storage[type].ContainsKey(t.AggregateId))
            {
                // update existing readmodel
                _storage[type][t.AggregateId] = t;
            }
            else
            {
                // add new readmodel
                _storage[type].Add(t.AggregateId, t);
            }
            return Task.FromResult(0);
        }

        public Task DeleteAsync<T>(T t, string partitionKeyValue = null) where T : TypeBasedProjectionBase
        {
            throw new NotImplementedException();
        }

        public Task DeleteAggregateAsync<T>(T t, string partitionKeyValue = null) where T : ProjectionBase
        {
            throw new NotImplementedException();
        }
    }
}
