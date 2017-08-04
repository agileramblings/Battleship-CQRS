using System;

namespace Battleship.Domain.CQRS
{
    public abstract class BaseCommand : Command
    {
        public readonly Guid CommandId;
        public readonly int CurrentAggregateVersion;

        protected BaseCommand(Guid commandId, int currentAggregateVersion)
        {
            CommandId = commandId;
            CurrentAggregateVersion = currentAggregateVersion;
        }
    }
}