using System;
using System.Collections.Generic;
using Battleship.Domain.Core.DDD;

namespace Battleship.Domain.Projections;

public class GameListItem : ProjectionBase
{
    public string GameName { get; set; }
    public string GameCode { get; set; }
    public List<string> PlayerNames { get; set; }

    public GameListItem(Guid ownerId) : base("", ownerId)
    {
    }
}