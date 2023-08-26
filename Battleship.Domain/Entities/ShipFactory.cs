using Battleship.Domain.Enums;

namespace Battleship.Domain.Entities;

public static class ShipFactory
{
    public static Ship BuildShip(ShipClasses desiredClassOfShip, Location placeBowAt, Heading heading)
    {
        return new Ship
        {
            ClassSize = GetClassSize(desiredClassOfShip),
            ClassName = GetClassName(desiredClassOfShip),
            BowAt = placeBowAt,
            Heading = heading,
            EntityId = Guid.NewGuid().ToString(),
            Status = ShipStatus.DryDock
        };
    }

    private static string GetClassName(ShipClasses desiredClassOfShip)
    {
        return desiredClassOfShip switch
        {
            ShipClasses.Destroyer => nameof(ShipClasses.Destroyer),
            ShipClasses.Submarine => nameof(ShipClasses.Submarine),
            ShipClasses.Cruiser => nameof(ShipClasses.Cruiser),
            ShipClasses.Battleship => nameof(ShipClasses.Battleship),
            ShipClasses.AircraftCarrier => nameof(ShipClasses.AircraftCarrier),
            _ => string.Empty
        };
    }

    private static uint GetClassSize(ShipClasses desiredClassOfShip)
    {
        return desiredClassOfShip switch
        {
            ShipClasses.Destroyer => 2,
            ShipClasses.Submarine => 3,
            ShipClasses.Cruiser => 3,
            ShipClasses.Battleship => 4,
            ShipClasses.AircraftCarrier => 5,
            _ => 0
        };
    }
}