using System;
using Battleship.Domain.Core.DDD;
using Battleship.Domain.Entities;
using NodaTime;

namespace Battleship.Domain.Projections;

public class GameProjection : ProjectionBase
{
    public GameProjection() : base("tbd", Guid.Empty)
    {
    }

    public GameProjection(string aggregateId, Guid owner) : base(aggregateId, owner)
    {
    }

    public uint Turn { get; set; }
    public Player[] Players { get; set; }
    public uint? Winner { get; set; }
    public uint Dimensions { get; set; }
    public Instant ActivatedOn { get; set; }
    public string LastMessage { get; set; }

    public bool ValidRowSelection(char row)
    {
        return GameConstants.Alphabet.Substring(0, (int)Dimensions).Contains(char.ToUpper(row).ToString());
    }

    public bool ValidColumnSelection(uint col)
    {
        return col > 0 && col <= Dimensions;
    }
}