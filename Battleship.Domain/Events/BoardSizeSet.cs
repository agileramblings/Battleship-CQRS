using System;
using Battleship.Domain.CQRS;

namespace Battleship.Domain.Events
{
    public class BoardSizeSet : Event
    {
        public Guid GameId;
        public uint Size;

        public BoardSizeSet(Guid gameId, uint dimensionSize)
        {
            Id = Guid.NewGuid();
            GameId = gameId;
            Size = dimensionSize;
        }
    }
}