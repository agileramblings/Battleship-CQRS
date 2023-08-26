using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleship.Domain.Core.Messaging;

namespace Battleship.Domain.Core.Services.Persistence.Commands;

public class InMemoryCommandRepository : ICommandRepository
{
    private readonly List<CommandBase> _commands = new();

    public Task SaveAsync<T>(T command) where T : CommandBase
    {
        _commands.Add(command);
        return Task.CompletedTask;
    }

    public Task<CommandBase?> GetAsync(string commandId)
    {
        return Task.FromResult(_commands.FirstOrDefault(c => c.EventParams.CorrelationId == commandId));
    }

    public Task<IEnumerable<CommandBase>> GetAllAsync(string aggregateId, int skip = 0, int take = int.MaxValue)
    {
        var results = _commands
            .Where(c => c.AggParams.AggregateId == aggregateId)
            .Skip(skip)
            .Take(take);

        return Task.FromResult(results);
    }
}