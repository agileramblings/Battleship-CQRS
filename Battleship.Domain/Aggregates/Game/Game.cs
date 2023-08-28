using Battleship.Domain.Aggregates.Game.Events;
using Battleship.Domain.Core.DDD;
using Battleship.Domain.Core.Messaging;
using Battleship.Domain.Entities;
using Battleship.Domain.Enums;
using Battleship.Domain.Projections;
using NodaTime;

namespace Battleship.Domain.Aggregates.Game;

public class Game : OwnedAggregateBase
{
    public Instant ActivatedOn { get; private set; }
    public Player[] Players { get; private set; } = Array.Empty<Player>();
    public Player? Winner { get; private set; }
    public uint Turn { get; private set; }
    public uint Dimensions { get; private set; }
    public bool LastAttackResult { get; private set; }
    public string LastMessage { get; private set; } = string.Empty;
    public Player? LastPlayer { get; private set; }
    public Instant LastPlayedOn { get; private set; }
    public GameStatus GameStatus { get; private set; } = GameStatus.NotStarted;

    public Game() : base("tbd", Guid.Empty) { /* DO NOT USE - Serializers Only*/ }
    public Game(Guid newGameId, uint dimensionSize, EventParams eventParams) : base(newGameId.ToString(), Guid.Empty)
    {
        var aggParams = new AggregateParams(newGameId.ToString(), -1, false, Guid.Empty);
        ApplyChange(new GameCreated(2, dimensionSize, aggParams, eventParams));
    }

    public void UpdatePlayerName(string name, uint position, EventParams eventParams)
    {
        ApplyChange(new PlayerNameUpdated(name, position, GetAggregateParams(), eventParams));
    }

    public AddShipResult AddShip(Ship ship, uint playerIndex, EventParams eventParams)
    {
        // Ensure current ship fits within board dimensions
        var result = Players[playerIndex].Board.CanPlaceShip(ship, out _);
        if (result.Added)
        {
            ApplyChange(new ShipAdded(playerIndex, ship, GetAggregateParams(), eventParams));
        }

        return result;
    }

    public void FireShot(Location target, uint attackingPlayerIndex, uint targetPlayerIndex, EventParams eventParams)
    {
        ApplyChange(new ShotFired( attackingPlayerIndex, targetPlayerIndex, target, GetAggregateParams(), eventParams));

        // if any player does not have active ships, the other player has won
        if (!Players[targetPlayerIndex].Board.HasActiveShips)
        {
            ApplyChange(new GameWon(attackingPlayerIndex, GetAggregateParams(), eventParams));
        }
    }

    #region Private Event Handlers
    // Applied by Reflection when reading events (AggregateBase -> this.AsDynamic().Apply(@event);)
    // Ensures aggregates get their needed property values (e.g. Id) from events
    // ReSharper disable UnusedMember.Local
    private void Apply(GameCreated e)
    {
        GameStatus = GameStatus.Started;
        Dimensions = e.BoardDimensions;
        Players = new Player[e.NumberOfPlayers];
        for (uint i = 0; i < e.NumberOfPlayers; i++)
        {
            Players[i] = new Player("tbd", i, e.BoardDimensions);
        }

        AggregateId = e.AggParams.AggregateId;
        ActivatedOn = e.EventParams.ReceivedOn;
    }

    private void Apply(PlayerNameUpdated e)
    {
        Players[e.PlayerPosition].Name = e.NewName;
        Version = e.AggParams.Version;
    }

    private void Apply(ShipAdded e)
    {
        Players[e.PlayerIndex].Board.AddShip(e.Ship);
        Version = e.AggParams.Version;
    }

    private void Apply(ShotFired e)
    {
        Turn++;
        Players[e.Aggressor].Board.AddShotFired(e.Location);
        var (hit, sunkShip, lastMessage) = Players[e.Target].Board.AddShotReceived(e.Location);
        LastMessage = lastMessage;
        LastAttackResult = hit;
        LastPlayer = Players[e.Aggressor];
        LastPlayedOn = e.EventParams.ReceivedOn;
        Version = e.AggParams.Version;
    }

    private void Apply(GameWon e)
    {
        GameStatus = GameStatus.Over;
        Winner = Players[e.WinningPlayerPosition];
        Version = e.AggParams.Version;
    }

    #endregion

    public GameProjection ToProjection()
    {
        return new GameProjection
        {
            AggregateId = this.AggregateId,
            Players = this.Players,
            Version = this.Version,
            ActivatedOn = this.ActivatedOn,
            Dimensions = this.Dimensions,
            LastMessage = this.LastMessage,
            OwnerId = this.Owner,
            Turn = this.Turn,
            Winner = Winner?.Position
        };
    }
}