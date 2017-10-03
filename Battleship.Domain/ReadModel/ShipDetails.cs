using System.Collections.Generic;
using System.Linq;
using Battleship.Domain.ReadModel.Enums;

namespace Battleship.Domain.ReadModel
{
    public class ShipDetails
    {
        private Location _bowAt;
        private uint _classSize;
        private Direction _heading = Direction.None;
        private readonly List<Location> _locations = new List<Location>();

        public Direction Heading
        {
            get => _heading;
            set
            {
                _heading = value;
                _BuildLocations();
            }
        }

        public Location BowAt
        {
            get => _bowAt;
            set
            {
                _bowAt = value;
                _BuildLocations();
            }
        }

        public uint ClassSize
        {
            get => _classSize;
            set
            {
                _classSize = value;
                _BuildLocations();
            }
        }

        public string ClassName { get; set; }
        public IEnumerable<Location> Locations => _locations;
        public HashSet<int> LocationSet => new HashSet<int>(_locations.Select(s => s.GetHashCode()));
        public ShipStatus Status { get; set; }

        /// <summary>
        ///     Boats do not care if their location is incorrect from the boards perspective
        ///     so they will build locations as per the rules, even if that means they are in illegal
        ///     game positions
        /// </summary>
        private void _BuildLocations()
        {
            if (_heading != Direction.None && _bowAt != null && _classSize != 0)
            {
                _locations.Clear();
                switch (Heading)
                {
                    case Direction.N:
                        _BuildNorthLocations();
                        break;
                    case Direction.E:
                        _BuildEastLocations();
                        break;
                    case Direction.W:
                        _BuildWestLocations();
                        break;
                    case Direction.S:
                        _BuildSouthLocations();
                        break;
                }
            }
        }

        private void _BuildSouthLocations()
        {
            _locations.Add(_bowAt);
            // decrement row from bow
            for (var i = 1; i <= ClassSize - 1; i++)
            {
                var newRow = (char) (_bowAt.Row - i);
                _locations.Add(new Location(newRow, _bowAt.Column));
            }
        }

        private void _BuildWestLocations()
        {
            _locations.Add(_bowAt);
            // increment col from bow
            for (var i = 1; i <= ClassSize - 1; i++)
            {
                var newCol = (uint) (_bowAt.Column + i);
                _locations.Add(new Location(_bowAt.Row, newCol));
            }
        }

        private void _BuildEastLocations()
        {
            _locations.Add(_bowAt);
            // decrement col from bow
            for (var i = 1; i <= ClassSize - 1; i++)
            {
                var newCol = (uint) (_bowAt.Column - i);
                _locations.Add(new Location(_bowAt.Row, newCol));
            }
        }

        private void _BuildNorthLocations()
        {
            _locations.Add(_bowAt);
            // increment row from bow
            for (var i = 1; i <= ClassSize - 1; i++)
            {
                var newRow = (char) (_bowAt.Row + i);
                _locations.Add(new Location(newRow, _bowAt.Column));
            }
        }
    }
}