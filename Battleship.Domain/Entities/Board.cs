using Battleship.Domain.Core.DDD;
using Battleship.Domain.Enums;

namespace Battleship.Domain.Entities;

public class Board : EntityBase
{
    public uint Dimension { get; init; }
    private readonly Dictionary<Location, Ship> _locationsWithShips = new();
    private readonly List<Ship> _ships = new();
    private readonly HashSet<Location> _shotsFired = new();
    private readonly HashSet<Location> _shotsReceived = new();
    private uint ActiveShips { get; set; }
    public bool HasActiveShips => ActiveShips > 0;

    public Board(uint dimensions) : base(string.Empty)
    {
        Dimension = dimensions;
    }

    public IEnumerable<Ship> Ships => _ships;
    public IEnumerable<Location> ShotsFired => _shotsFired;
    public IEnumerable<Location> ShotsReceived => _shotsReceived;

    public char[,] Representation
    {
        get
        {
            var representation = new char[Dimension + 1, Dimension + 1];
            // rows
            for (var i = 0; i <= Dimension; i++)
            {
                // cols
                for (uint j = 0; j <= Dimension; j++)
                {
                    representation[i, j] = '.';
                }
            }

            // TopLeft Corner
            representation[0, 0] = '@';

            // draw header row
            // cols
            for (uint j = 1; j <= Dimension; j++)
            {
                representation[0, j] = j.ToString().First();
            }

            // draw label column
            for (uint j = 0; j < Dimension; j++)
            {
                representation[j + 1, 0] = GameConstants.Alphabet[(int)j];
            }

            // add Ships
            foreach (var location in _locationsWithShips.Keys)
            {
                var row = GameConstants.Alphabet.IndexOf(location.Row) + 1;
                var col = location.Column;
                representation[row, col] = _locationsWithShips[location].ClassName.First();
            }

            foreach (var sr in ShotsReceived)
            {
                var row = GameConstants.Alphabet.IndexOf(sr.Row) + 1;
                var col = sr.Column;
                representation[row, col] = 'X';
            }
            return representation;
        }
    }


    public AddShipResult CanPlaceShip(Ship shipToAdd, out List<Location> desiredLocations)
    {
        // does the ship's size and orientation with the bow location fit on the board
        desiredLocations = BuildDesiredLocations(shipToAdd);
        if (!desiredLocations.All(dl => ValidRowSelection(dl.Row) && ValidColumnSelection(dl.Column)))
        {
            return new AddShipResult(false, "Ship would not be completely on the board.");
        }

        // does the board have any ships occupying any of those spots yet

        /* - this is an O(N) search + the memory allocation of the keys to a hashset
        var occupiedLocations = _locationsWithShips.Keys.ToHashSet();
        if (occupiedLocations.Intersect(desiredLocations).Any())
        {
            return (false, "Another ship already occupies some or all of these spaces.");
        }
        */

        // this is a N * O(1) search with N being the ship size with no memory allocation for the keys
        // I think this would be overall faster than the above search
        foreach (var loc in desiredLocations)
        {
            if (_locationsWithShips.ContainsKey(loc))
            {
                return new AddShipResult(false, "Another ship already occupies some or all of these spaces.");
            }
        }

        return new AddShipResult(true, "Can add ship");

    }
    public AddShipResult AddShip(Ship shipToAdd)
    {
        var result = CanPlaceShip(shipToAdd, out var desiredLocations);

        if (result.Added)
        {
            // add occupied locations with reference to ship
            foreach (var location in desiredLocations)
            {
                _locationsWithShips.Add(location, shipToAdd);
            }

            // increment the number of active ships in the theater
            _ships.Add(shipToAdd);
            ActiveShips++;
            shipToAdd.Status = ShipStatus.Active;

            return new AddShipResult(true, "Ship added.");
        }
        return  new AddShipResult(result.Added, result.Message);
    }

    public void AddShotFired(Location shotFired)
    {
        _shotsFired.Add(shotFired);
    }

    public AttackShipResult AddShotReceived(Location shotReceived)
    {
        _shotsReceived.Add(shotReceived);

        if(_locationsWithShips.TryGetValue(shotReceived, out var boatAtLocation))
        {
            var result = boatAtLocation.Hit(shotReceived);
            if (result.SunkShip)
            {
                ActiveShips--;
            }

            return result;
        }
        return new AttackShipResult(false, false, "Missed");
    }

    private bool ValidRowSelection(char row)
    {
        return GameConstants.Alphabet[..(int)Dimension].Contains(char.ToUpper(row).ToString());
    }

    private bool ValidColumnSelection(uint col)
    {
        return col > 0 && col <= Dimension;
    }

    private List<Location> BuildDesiredLocations(Ship shipToAdd)
    {
        var locations = new List<Location>();
        if (shipToAdd.Heading == Heading.None || shipToAdd.BowAt == null || shipToAdd.ClassSize == 0)
            return locations;
        return shipToAdd.Heading switch
        {
            Heading.N => BuildNorthLocations(shipToAdd),
            Heading.E => BuildEastLocations(shipToAdd),
            Heading.W => BuildWestLocations(shipToAdd),
            Heading.S => BuildSouthLocations(shipToAdd),
            _ => throw new ArgumentOutOfRangeException(nameof(shipToAdd.Heading)),
        };
        return locations;
    }

    private List<Location> BuildSouthLocations(Ship shipToAdd)
    {
        var locations = new List<Location>();
        var bowAt = shipToAdd.BowAt;
        var classSize = shipToAdd.ClassSize;
        locations.Add(bowAt);
        // decrement row from bow
        for (var i = 1; i <= classSize - 1; i++)
        {
            var newRow = (char)(bowAt.Row - i);
            locations.Add(new Location(newRow, bowAt.Column));
        }
        return locations;
    }

    private List<Location> BuildWestLocations(Ship shipToAdd)
    {
        var locations = new List<Location>();
        var bowAt = shipToAdd.BowAt;
        var classSize = shipToAdd.ClassSize;
        locations.Add(bowAt);
        // increment col from bow
        for (var i = 1; i <= classSize - 1; i++)
        {
            var newCol = (uint)(bowAt.Column + i);
            locations.Add(new Location(bowAt.Row, newCol));
        }
        return locations;
    }

    private List<Location> BuildEastLocations(Ship shipToAdd)
    {
        var locations = new List<Location>();
        var bowAt = shipToAdd.BowAt;
        var classSize = shipToAdd.ClassSize;
        locations.Add(bowAt);
        // decrement col from bow
        for (var i = 1; i <= classSize - 1; i++)
        {
            var newCol = (uint)(bowAt.Column - i);
            locations.Add(new Location(bowAt.Row, newCol));
        }
        return locations;
    }

    private List<Location> BuildNorthLocations(Ship shipToAdd)
    {
        var locations = new List<Location>();
        var bowAt = shipToAdd.BowAt;
        var classSize = shipToAdd.ClassSize;
        locations.Add(bowAt);
        // increment row from bow
        for (var i = 1; i <= classSize - 1; i++)
        {
            var newRow = (char)(bowAt.Row + i);
            locations.Add(new Location(newRow, bowAt.Column));
        }
        return locations;
    }
}