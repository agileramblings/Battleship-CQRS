namespace Battleship.Domain;

public record ShipType
{
    public static readonly ShipType Destroy = new("Destroy", 2);
    public static readonly ShipType Submarine = new("Submarine", 3);
    public static readonly ShipType Cruiser = new("Cruiser", 3);
    public static readonly ShipType Battleship = new("Battleship", 4);
    public static readonly ShipType AircraftCarrier = new("AircraftCarrier", 5);

    public static readonly ShipType[] AvailableTypes = { Destroy, Submarine, Cruiser, Battleship, AircraftCarrier };
    public string Name { get; init; }
    public uint Size { get; init; }
    private ShipType(string name, uint size)
    {
        Name = name;
        Size = size;
    }
}