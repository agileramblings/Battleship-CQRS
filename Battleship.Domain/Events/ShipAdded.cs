using System;
using Battleship.Domain.CQRS;
using Battleship.Domain.ReadModel;

namespace Battleship.Domain.Events
{
    public class ShipAdded : Event
    {
        public readonly Guid GameId;
        public readonly uint PlayerIndex;
        public readonly ShipDetails ShipToAdd;

        public ShipAdded(Guid gameId, uint playerIndex, ShipDetails ship)
        {
            Id = Guid.NewGuid();
            GameId = gameId;
            PlayerIndex = playerIndex;
            ShipToAdd = ship;
        }
    }
}