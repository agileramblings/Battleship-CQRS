using System;
using Battleship.Domain.CQRS;

namespace Battleship.Domain.Events
{
    public class PlayerNameUpdated : Event
    {
        public readonly Guid GameId;
        public readonly string Name;
        public readonly int Position;

        public PlayerNameUpdated(Guid gameId, string name, int position)
        {
            Id = Guid.NewGuid();
            GameId = gameId;
            Name = name;
            Position = position;
        }
    }
}