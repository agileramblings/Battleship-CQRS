using Battleship.Domain.Core.Messaging;
using MediatR;

namespace Battleship.Domain.Aggregates.Game.Events;

public record GameCreated(uint NumberOfPlayers, uint BoardDimensions, AggregateParams AggParams, EventParams EventParams) : EventBase(AggParams, EventParams), INotification;