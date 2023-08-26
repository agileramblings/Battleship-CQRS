using System;

namespace Battleship.Domain.Core.Messaging;

public class Correlation
{
    public Correlation(string? correlationId = null)
    {
        if (!string.IsNullOrEmpty(correlationId)) 
            Id = correlationId;
    }

    public string Id { get; } = Guid.NewGuid().ToString();
}

public interface ICorrelationIdResolver
{
    string Resolve();
}

public class DefaultCorrelationIdResolver : ICorrelationIdResolver
{
    public string Resolve()
    {
        return Guid.NewGuid().ToString();
    }
}