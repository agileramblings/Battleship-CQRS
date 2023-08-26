using Battleship.Domain.Core.Messaging;
using MediatR;

namespace Battleship.Domain.Aggregates.Game.Events
{
    public record PlayerNameUpdated(string NewName, uint PlayerPosition, AggregateParams AggParms, EventParams EventParams) : EventBase(AggParms, EventParams), INotification;
}