using Battleship.Domain.Core.DDD;

namespace Battleship.Domain.Entities;
public class Player : EntityBase
{
    public Board Board { get; init; }

    public Player(string name, uint position, uint dimensions) : base(position.ToString())
    {
        Name = name;
        Position = position;
        Board = new Board(dimensions);
    }

    public string Name { get; set; }
    public uint Position { get; init; }

    public void Deconstruct(out string name, out uint position)
    {
        name = Name;
        position = Position;
    }
}
