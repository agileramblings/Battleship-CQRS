namespace Battleship.Domain;

public class Location
{
    private readonly char _row;
    public char Row { get => _row; init => _row = char.ToUpper(value); }
    public uint Column { get; init; }

    public Location(char row, uint column)
    {
        _row = char.ToUpper(row);
        Column = column;
    }

    protected bool Equals(Location other)
    {
        return _row == other._row && Column == other.Column;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Location)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_row, Column);
    }

    public override string ToString()
    {
        return $"{char.ToUpper(Row)}:{Column}";
    }
}
