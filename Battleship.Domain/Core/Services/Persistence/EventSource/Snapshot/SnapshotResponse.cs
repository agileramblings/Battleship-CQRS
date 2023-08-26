using System.Collections.Generic;
using Battleship.Domain.Core.Messaging;

namespace Battleship.Domain.Core.Services.Persistence.EventSource.Snapshot;

public class SnapshotResponse
{
    public bool ShouldSnapshot { get; set; }
    public IEnumerable<EventBase> Events { get; set; } = new List<EventBase>();
}