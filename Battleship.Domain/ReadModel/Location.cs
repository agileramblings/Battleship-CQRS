namespace Battleship.Domain.ReadModel
{
    public class Location
    {
        public Location(char row, uint column)
        {
            Row = char.ToUpper(row);
            Column = column;
        }

        public char Row { get; }
        public uint Column { get; }

        public override string ToString()
        {
            return $"{Row}:{Column}";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Location) obj);
        }

        protected bool Equals(Location other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Row.GetHashCode() * 397) ^ (int) Column;
            }
        }
    }
}