namespace Battleship.Domain.Core.DDD;

public abstract class EntityBase
{
    protected EntityBase(string entityId)
    {
        EntityId = entityId;
    }

    public string EntityId { get; set; }
}