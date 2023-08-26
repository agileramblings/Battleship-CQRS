using System;

namespace Battleship.Domain.Core.Messaging;

public abstract record CommandBase(AggregateParams AggParams, EventParams EventParams) : Message(Guid.NewGuid(), EventParams.CorrelationId, EventParams.CausationId);
