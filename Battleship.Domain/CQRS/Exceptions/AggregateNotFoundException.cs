using System;

namespace Battleship.Domain.CQRS.Exceptions
{
    public class AggregateNotFoundException : Exception
    {
        public readonly Guid Id;

        public AggregateNotFoundException(Guid id) : base(
            $"There were no events discovered for the requested aggregate ({id})")
        {
            Id = id;
        }
    }
}