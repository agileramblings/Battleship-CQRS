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
    private readonly HashSet<Location> _hitAtLocations = new();

    public Ship() : base(Guid.NewGuid().ToString())
    {
    }

    public AttackShipResult Hit(Location hitAtLocation)
    {
        // Ship has already been hit here
        if (!_hitAtLocations.Add(hitAtLocation)) return new AttackShipResult(true, false, "Hit!");

        // new hit - check if ship is sunk
        if(_hitAtLocations.Count >= ClassSize)
        {
            Status = ShipStatus.Sunk;
            return new AttackShipResult(true, true, $"You sank the {ClassName}!");
        }
        return new AttackShipResult(true, false, "Hit!");
    }
    public override string ToString()
    {
        return $"{EntityId} - Class: {ClassName} Damage: {TimesHit}/{ClassSize}";
    }
}