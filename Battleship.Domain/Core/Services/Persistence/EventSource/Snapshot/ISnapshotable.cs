namespace Battleship.Domain.Core.Services.Persistence.EventSource.Snapshot;

public interface ISnapshotable
{
    void TakeSnapshot(string? correlationId);
}