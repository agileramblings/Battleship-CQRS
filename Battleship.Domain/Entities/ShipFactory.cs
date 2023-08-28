using Battleship.Domain.Enums;

namespace Battleship.Domain.Entities;

public static class ShipFactory
{
    public static Ship BuildShip(ShipType desiredClassOfShip, Location placeBowAt, Heading heading)
    {
        return new Ship
        {
            ClassSize = desiredClassOfShip.Size,
            ClassName = desiredClassOfShip.Name,
            BowAt = placeBowAt,
            Heading = heading,
            EntityId = Guid.NewGuid().ToString(),
            Status = ShipStatus.DryDock
        };
    }
}