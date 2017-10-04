using System;
using Battleship.Domain.CQRS;
using Battleship.Domain.ReadModel;

namespace Battleship.Domain.Commands
{
    // Commands that allow you to alter the state of the system
    public class CreateGame : BaseCommand
    {
        public readonly uint BoardSize;
        public CreateGame(Guid id, uint boardSize)
            : base(id, -1)
        {
            BoardSize = boardSize;
        }
    }

    public class UpdatePlayerName : BaseCommand
    {
        public readonly Guid GameId;
        public readonly string Name;
        public readonly uint Position;

        public UpdatePlayerName(Guid commandId, int currentAggregateVersion, Guid gameId, string playerName, uint position)
            : base(commandId, currentAggregateVersion)
        {
            GameId = gameId;
            Name = playerName;
            Position = position;
        }
    }

    public class AddShip : BaseCommand
    {
        public readonly Guid GameId;
        public readonly uint PlayerIndex;
        public readonly ShipDetails ShipDetails;
        
        public AddShip(Guid commandId, int currentAggregateVersion, Guid gameId, uint playerIndex, ShipDetails shipDetails)
            : base(commandId, currentAggregateVersion)
        {
            GameId = gameId;
            PlayerIndex = playerIndex;
            ShipDetails = shipDetails;
        }
    }

    public class FireShot : BaseCommand
    {
        public readonly Guid GameId;
        public readonly Location Target;
        public readonly uint AttackingPlayerIndex;
        public readonly uint TargetPlayerIndex;

        public FireShot(Guid commandId, int currentAggregateVersion, Guid gameId, Location target, uint attackingPLayerIndex, uint targetPlayerIndex) : base(commandId, currentAggregateVersion)
        {
            GameId = gameId;
            Target = target;
            AttackingPlayerIndex = attackingPLayerIndex;
            TargetPlayerIndex = targetPlayerIndex;
        }
    }
}