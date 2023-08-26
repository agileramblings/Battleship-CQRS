using System;
using System.Collections.Generic;
using Battleship.Domain.Core.Messaging;
using ReflectionMagic;

namespace Battleship.Domain.Core.DDD;

public record AggregateId(string Id);

public abstract class AggregateBase
{
    private readonly List<EventBase> _changes = new();

    protected AggregateBase(string aggregateId)
    {
        AggregateId = aggregateId;
    }

    public string AggregateId { get; set; }
    public NodaTime.Instant Created { get; set; }
    public string? CreatedBy { get; set; }
    public NodaTime.Instant Modified { get; set; }
    public string? ModifiedBy { get; set; }
    public int Version { get; protected set; } = -1;

    public int EventCount => _changes.Count;

    public IEnumerable<EventBase> GetUncommittedChanges()
    {
        return _changes;
    }

    public void MarkChangesAsCommitted()
    {
        _changes.Clear();
    }

    public void LoadsFromHistory(IEnumerable<EventBase> history)
    {
        var version = -1;
        foreach (var e in history)
        {
            ApplyChange(e, false);
            version++;
        }

        Version = version;
    }

    protected void ApplyChange(EventBase @event)
    {
        ApplyChange(@event, true);
    }

    protected void PopulateCreatedBy(EventParams @params)
    {
        Created = @params.ReceivedOn;
        CreatedBy = @params.InvokedBy;
    }

    protected void PopulateModifiedBy(EventParams @params)
    {
        Modified = @params.ReceivedOn;
        ModifiedBy = @params.InvokedBy;
    }

    // push atomic aggregate changes to local history for further processing (EventStore.SaveEvents)
    private void ApplyChange(EventBase @event, bool isNew)
    {
        this.AsDynamic().Apply(@event);
        if (isNew) _changes.Add(@event);
    }
}

public abstract class OwnedAggregateBase : AggregateBase
{
    public Guid Owner { get; protected set; }
    protected OwnedAggregateBase(string aggregateId, Guid ownerId) : base(aggregateId)
    {
        Owner = ownerId;
    }

    protected virtual AggregateParams GetAggregateParams()
    {
        return new AggregateParams(AggregateId, Version, false, Owner);
    }
}