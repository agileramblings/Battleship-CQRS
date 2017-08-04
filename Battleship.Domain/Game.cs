using System;
using Battleship.Domain.CQRS;
using Battleship.Domain.Events;
using Battleship.Domain.ReadModel;

namespace Battleship.Domain
{
    public class Game : AggregateBase
    {
        private uint _boardSize;
        public uint BoardSize => _boardSize;

        public Game()
        {
        }

        public Game(Guid newGameId, uint dimensionSize)
        {
            ApplyChange(new GameCreated(newGameId));
            ApplyChange(new BoardSizeSet(newGameId, dimensionSize));
        }

        public void UpdatePlayer(string name, int position)
        {
            ApplyChange(new PlayerNameUpdated(Id, name, position));
        }

        public void AddShip(ShipDetails ship, uint playerIndex)
        {
            // Ensure current ship fits within board dimensions
            ApplyChange(new ShipAdded(Id, playerIndex, ship));
        }

        public void FireShot(Location target, uint attackingPlayerIndex, uint targetPlayerIndex)
        {
            ApplyChange(new ShotFired(Id, attackingPlayerIndex, targetPlayerIndex, target));
        }

        #region Private Setters
        // Applied by Reflection when reading events (AggregateBase -> this.AsDynamic().Apply(@event);)
        // Ensures aggregates get their needed property values (e.g. Id) from events

        // ReSharper disable once UnusedMember.Local
        private void Apply(GameCreated e)
        {
            Id = e.Id;
        }

        // ReSharper disable once UnusedMember.Local
        private void Apply(BoardSizeSet e)
        {
            _boardSize = e.Size;
        }
        #endregion
    }
}