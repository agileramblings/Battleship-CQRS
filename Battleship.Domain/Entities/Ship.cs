using System;
using Battleship.Domain.Core.DDD;
using Battleship.Domain.Enums;

namespace Battleship.Domain.Entities;

public class Ship : EntityBase
{
    
    public Heading Heading { get; set; } = Heading.None;
    public Location BowAt { get; set; } = new Location('A', 0);
    public uint ClassSize { get; set; } = 2;
    public string ClassName { get; set; } = "Destroyer";
    public uint TimesHit { get; private set; }
    public ShipStatus Status { get; set; } = ShipStatus.DryDock;

    public Ship() : base(Guid.NewGuid().ToString())
    {
    }

    public (string message, bool hit, bool sunkShip) Hit()
    {
        TimesHit++;
        if (TimesHit >= ClassSize)
        {
            Status = ShipStatus.Sunk;
            return ($"You sank the {ClassName}!", true, true);
        }

        return ("Hit!", true, false);
    }
    public override string ToString()
    {
        return $"{EntityId} - Class: {ClassName} Damage: {TimesHit}/{ClassSize}";
    }
}