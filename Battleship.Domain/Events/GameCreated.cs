using System;
using Battleship.Domain.CQRS;

namespace Battleship.Domain.Events
{
    public class GameCreated : Event
    {
        public DateTime CreatedOn;

        public GameCreated(Guid id)
        {
            Id = id;
            CreatedOn = DateTime.UtcNow;
        }
    }
}