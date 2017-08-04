using System;
using Battleship.Domain.CQRS;
using Battleship.Domain.ReadModel;

namespace Battleship.Domain.Events
{
    public class ShotFired : Event
    {
        public readonly Guid GameId;
        public readonly Location Target;
        public readonly uint AggressorPlayer;
        public readonly uint TargetPlayer;

        public ShotFired(Guid gameId, uint aggressorPlayerIndex, uint targetPlayerIndex, Location target)
        {
            Id = Guid.NewGuid();
            GameId = gameId;
            Target = target;
            AggressorPlayer = aggressorPlayerIndex;
            TargetPlayer = targetPlayerIndex;
        }
    }
}