using Battleship.Domain.Core.Messaging;
using NodaTime;

namespace Battleship.Tests;

public static class Utilities
{
    public static EventParams ToEventParams(this IClock clock)
    {
        return new EventParams("TestFramework", clock.GetCurrentInstant(), "TestCorrelation", Guid.Empty);
    }
}