using System;

namespace Battleship.Domain.CQRS
{
    public class Event : IMessage
    {
        public Guid Id;
        public int Version;
    }
}