using System;

namespace Battleship.Domain.Core.Services.Persistence;

public interface IOwnershipContext
{
    Guid OwnerId { get; set; }
}

public class OwnershipContext : IOwnershipContext
{
    public OwnershipContext(Guid? ownerId = null)
    {
        OwnerId = ownerId ?? Guid.Empty;
    }

    public Guid OwnerId { get; set; }
}

public interface IOwnershipContextResolver
{
    Guid Resolve();
}

public class DefaultOwnershipContextResolver : IOwnershipContextResolver
{
    private readonly Guid? _owner;

    public DefaultOwnershipContextResolver(Guid? defaultOwner = null)
    {
        _owner = defaultOwner;
    }

    public Guid Resolve()
    {
        return _owner ?? Guid.Empty;
    }
}