using System;

namespace Battleship.Domain.Core.Messaging;

public abstract record Message(Guid MessageId, string CorrelationId, Guid CausationId);