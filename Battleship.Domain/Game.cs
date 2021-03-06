﻿using System;
using Battleship.Domain.CQRS;
using Battleship.Domain.Events;
using Battleship.Domain.ReadModel;

namespace Battleship.Domain
{
    public class Game : AggregateBase
    {
        private GameDetails _innerDetails;
        public bool CanAddShip(ShipDetails ship, uint playerIndex) => _innerDetails?.Players[playerIndex].Board.ShipFitsOnBoard(ship) ?? false;
        public bool ValidLocation(Location location, uint playerIndex) => _innerDetails?.Players[playerIndex].Board.ValidLocations.Contains(location.GetHashCode()) ?? false;

        public Game()
        {
        }

        public Game(Guid newGameId, uint dimensionSize)
        {
            ApplyChange(new GameCreated(newGameId));
            ApplyChange(new BoardSizeSet(newGameId, dimensionSize));
        }

        public void UpdatePlayer(string name, uint position)
        {
            ApplyChange(new PlayerNameUpdated(Id, name, position));
        }

        public bool AddShip(ShipDetails ship, uint playerIndex)
        {
            // Ensure current ship fits within board dimensions
            if (_innerDetails.Players[playerIndex].Board.ShipFitsOnBoard(ship))
            {
                ApplyChange(new ShipAdded(Id, playerIndex, ship));
                return true;
            }
            return false;
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
            _innerDetails = new GameDetails
            {
                Id = e.Id,
                ActivatedOn = e.CreatedOn,
                Version = e.Version
            };
        }

        // ReSharper disable once UnusedMember.Local
        private void Apply(BoardSizeSet e)
        {
            _innerDetails.Dimensions = e.Size;
            foreach (var player in _innerDetails.Players)
            {
                player.Board.Dimensions = e.Size;
            }
            _innerDetails.Version = e.Version;
        }

        private void Apply(ShipAdded e)
        {
            _innerDetails.Players[e.PlayerIndex].Board.AddShip(e.ShipToAdd);
            _innerDetails.Version = e.Version;
        }

        #endregion
    }
}