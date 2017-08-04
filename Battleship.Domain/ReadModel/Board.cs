using System;
using System.Collections.Generic;
using System.Linq;
using Battleship.Domain.ReadModel.Enums;

namespace Battleship.Domain.ReadModel
{
    public class Board
    {
        private uint _dimension;
        private readonly HashSet<int> _validLocations = new HashSet<int>();
        private readonly HashSet<int> _shotsReceivedSet = new HashSet<int>();
        private readonly List<ShipDetails> _ships = new List<ShipDetails>();
        private readonly List<Location> _shotsFired = new List<Location>();
        private readonly List<Location> _shotsReceived = new List<Location>();

        public uint Dimensions {
            get => _dimension;
            set
            {
                _dimension = value;
                _BuildValidLocationSet();
            }
        }
        public string LastAttackMessage { get; set; }

        public HashSet<int> ValidLocations => _validLocations;
        public IReadOnlyCollection<ShipDetails> Ships => _ships;
        public IReadOnlyCollection<Location> ShotsFired => _shotsFired;
        public IReadOnlyCollection<Location> ShotsReceived => _shotsReceived;

        public bool ShipFitsOnBoard(ShipDetails shipToPlace)
        {
            // check against existing ship locations
            var existingShipLocations = new HashSet<int>();
            foreach (var ship in Ships)
            {
                foreach (var loc in ship.LocationSet)
                {
                    existingShipLocations.Add(loc);
                }
            }

            if (existingShipLocations.Overlaps(shipToPlace.LocationSet))
            {
                return false;
            }

            // Check that is a valid board location
            var shipLocations = shipToPlace.LocationSet.Count;
            var intersection = ValidLocations.Intersect(shipToPlace.LocationSet);
            return intersection.Count() == shipLocations;
        }

        public char[,] Representation
        {
            get
            {
                var representation = new char[_dimension+1, _dimension+1];
                // rows
                for (var i = 0; i <= _dimension; i++)
                {
                    // cols
                    for (uint j = 0; j <= _dimension; j++)
                    {
                        representation[i, j] = '.';
                    }
                }

                // TopLeft Corner
                representation[0, 0] = '@';

                // draw header row
                // cols
                for (uint j = 1; j <= _dimension; j++)
                {
                    representation[0, j] = j.ToString().First();
                }

                // draw label column
                for (uint j = 0; j < _dimension; j++)
                {
                    representation[j+1, 0] = GameConsts.Alphabet[(int)j];
                }

                // add Ships
                foreach (var ship in Ships)
                {
                    foreach (var location in ship.Locations)
                    {
                        var row = GameConsts.Alphabet.IndexOf(location.Row)+1;
                        var col = location.Column;
                        representation[row, col] = ship.ClassName.First();
                    }
                }

                foreach (var sr in ShotsReceived)
                {
                    var row = GameConsts.Alphabet.IndexOf(sr.Row) + 1;
                    var col = sr.Column;
                    representation[row, col] = 'X';
                }
                return representation;
            }
        }

        public void AddShip(ShipDetails shipToAdd)
        {
            if (ShipFitsOnBoard(shipToAdd))
            {
                _ships.Add(shipToAdd);
            }
        }

        public void AddShotFired(Location shotFired)
        {
            _shotsFired.Add(shotFired);
        }

        public void AddShotReceived(Location shotReceived)
        {
            _shotsReceived.Add(shotReceived);
            _shotsReceivedSet.Add(shotReceived.GetHashCode());

            foreach (var ship in Ships)
            {
                // check ships for damage
                if (ship.LocationSet.Contains(shotReceived.GetHashCode()))
                {
                    // Hit
                    LastAttackMessage = "The last attack hit one of our ships.";
                }
                else
                {
                    LastAttackMessage = "Missed!!";
                }

                // check for sunken ship
                var shotsThatHitThisBoat = _shotsReceivedSet.Intersect(ship.LocationSet).Count();
                if (shotsThatHitThisBoat == ship.ClassSize)
                {
                    ship.Status = ShipStatus.Sunk;
                    LastAttackMessage = $"You sank my {ship.ClassName}!!";
                    if (_ships.All(s => s.Status == ShipStatus.Sunk))
                    {
                        LastAttackMessage +=
                            $"{Environment.NewLine}The game is over. All of your ships have sunk!";
                    }
                }
            }
        }

        public bool HasActiveShips => Ships.Any(s => s.Status == ShipStatus.Active);

        private void _BuildValidLocationSet()
        {
            _validLocations.Clear();
            for (var i = 0; i < _dimension; i++)
            {
                for (uint j = 1; j <= _dimension; j++)
                {
                    var location = new Location(GameConsts.Alphabet[i], j);
                    _validLocations.Add(location.GetHashCode());
                }
            }
        }

    }
}