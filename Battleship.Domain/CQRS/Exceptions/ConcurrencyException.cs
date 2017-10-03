using System;

namespace Battleship.Domain.CQRS.Exceptions
{
    public class ConcurrencyException : Exception
    {
        public readonly Guid Id;

        public ConcurrencyException(Guid id) : base(
            $"There was an attempt to save multiple, concurrent changes to an aggregate that could not be resolved. ({id})")
        {
            Id = id;
        }
    }
}