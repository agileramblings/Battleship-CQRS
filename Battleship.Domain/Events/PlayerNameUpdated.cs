using System;
using Battleship.Domain.CQRS;

namespace Battleship.Domain.Events
{
    public class PlayerNameUpdated : Event
    {
        public readonly Guid GameId;
        public readonly string Name;
        public readonly uint Position;

        public PlayerNameUpdated(Guid gameId, string name, uint position)
        {
            Id = Guid.NewGuid();
            GameId = gameId;
            Name = name;
            Position = position;
        }
    }
}