using Battleship.Domain.Core.Messaging;
using MediatR;

namespace Battleship.Domain.Aggregates.Game.Events;

public record GameWon(uint WinningPlayerPosition, AggregateParams AggParams, EventParams EventParams) : EventBase(AggParams, EventParams), INotification;