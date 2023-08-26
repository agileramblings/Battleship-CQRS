using Battleship.Domain.Core.Messaging;
using MediatR;

namespace Battleship.Domain.Aggregates.Game.Events;

public record ShotFired(uint Aggressor, uint Target, Location Location, AggregateParams AggParams, EventParams EventParams) : EventBase(AggParams, EventParams), INotification;