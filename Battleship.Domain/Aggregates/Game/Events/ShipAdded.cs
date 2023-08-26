using Battleship.Domain.Core.Messaging;
using Battleship.Domain.Entities;
using MediatR;

namespace Battleship.Domain.Aggregates.Game.Events;

public record ShipAdded(uint PlayerIndex, Ship Ship, AggregateParams AggParams, EventParams EventParams) : EventBase(AggParams, EventParams), INotification;