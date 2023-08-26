using Battleship.Domain.Core.Messaging;
using Battleship.Domain.Entities;
using MediatR;

namespace Battleship.Domain.Commands;

// Commands that allow you to alter the state of the system
public record CreateGame(uint BoardSize, AggregateParams AggParams, EventParams EventParams) : CommandBase(AggParams, EventParams), IRequest;
public record UpdatePlayerName(string NewName, uint Position, AggregateParams AggParams, EventParams EventParams) : CommandBase(AggParams, EventParams), IRequest;
public record AddShip(uint PlayerIndex, Ship ShipDetails, AggregateParams AggParams, EventParams EventParams) : CommandBase(AggParams, EventParams), IRequest;
public record FireShot(uint AttackingPlayerIndex, uint TargetPlayerIndex, Location Target, AggregateParams AggParams, EventParams EventParams) : CommandBase(AggParams, EventParams), IRequest;