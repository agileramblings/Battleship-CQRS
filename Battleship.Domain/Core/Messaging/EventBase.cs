using System;

namespace Battleship.Domain.Core.Messaging;

public abstract record EventBase(AggregateParams AggParams, EventParams EventParams) : Message(Guid.NewGuid(), EventParams.CorrelationId, EventParams.CausationId);
public record EventParams (string InvokedBy, NodaTime.Instant ReceivedOn, string CorrelationId, Guid CausationId);
public record AggregateParams(string AggregateId, int Version, bool IsSnapshot, Guid Owner);