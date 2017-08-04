using System;
using System.Collections.Generic;

namespace Battleship.Domain.CQRS
{
    public abstract class AggregateBase
    {
        private readonly List<Event> _changes = new List<Event>();
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
        public int Version { get; internal set; }

        public IEnumerable<Event> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<Event> history)
        {
            int version = -1;
            foreach (var e in history)
            {
                ApplyChange(e, false);
                version++;
            }
            Version = version;
        }

        protected void ApplyChange(Event @event)
        {
            ApplyChange(@event, true);
        }

        // push atomic aggregate changes to local history for further processing (EventStore.SaveEvents)
        private void ApplyChange(Event @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);
            if (isNew)
                _changes.Add(@event);
        }
    }
}