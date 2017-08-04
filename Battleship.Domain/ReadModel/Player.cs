namespace Battleship.Domain.ReadModel
{
    public class Player
    {
        public Board Board = new Board();
        public string Name;
        public int Position;

        public bool HasActiveShips => Board.HasActiveShips;
    }
}