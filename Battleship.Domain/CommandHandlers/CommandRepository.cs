using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleship.Domain.CQRS;

namespace Battleship.Domain.CommandHandlers
{
    public interface ICommandRepository
    {
        Task Save<T>(T command) where T : BaseCommand;
        Task<IEnumerable<BaseCommand>> Get(Guid tenantId, int skip, int take);
    }

    public class InMemoryCommandRepository : ICommandRepository
    {
        private readonly List<BaseCommand> _commands = new List<BaseCommand>();

        public Task Save<T>(T command) where T : BaseCommand
        {
            _commands.Add(command);
            return Task.FromResult(0);
        }

        public Task<IEnumerable<BaseCommand>> Get(Guid tenantId, int skip, int take)
        {
            var retVal =  _commands.OrderBy(c => c.ReceivedOn).Skip(skip).Take(take);
            return Task.FromResult(retVal);
        }
    }
}