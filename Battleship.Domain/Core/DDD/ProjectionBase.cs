using System;

namespace Battleship.Domain.Core.DDD;

public abstract class ReadModelBase
{
    public Guid OwnerId { get; set; }

    protected ReadModelBase(Guid ownerId)
    {
        OwnerId = ownerId;
    }
}

public abstract class ProjectionBase : ReadModelBase
{
    /// <summary>
    ///     Aggregate-based Identity
    /// </summary>
    /// <param name="aggregateId"></param>
    protected ProjectionBase(string aggregateId, Guid owner) : base (owner)
    {
        AggregateId = aggregateId;
    }

    //[JsonPropertyName("id")]
    public string id
    {
        get => AggregateId;
        private set => AggregateId = value;
    }

    public string AggregateId { get; set; }
    public int Version { get; set; }
}

public abstract class TypeBasedProjectionBase : ReadModelBase
{
    /// <summary>
    ///     Type-based identity
    /// </summary>

    //[JsonPropertyName("id")] // This must stay lower-case for the CosmosDb ReadModel to work
    public string id // This must stay lower-case for the CosmosDb ReadModel to work
    {
        get => GetType().Name;
        set => _ = 0; //noop
    }

    public string AggregateId => GetType().Name;

    protected TypeBasedProjectionBase(Guid owner) : base(owner)
    {
    }
}