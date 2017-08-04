using System;

namespace Battleship.Domain.ReadModel
{
    public abstract class ReadModelBase
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}