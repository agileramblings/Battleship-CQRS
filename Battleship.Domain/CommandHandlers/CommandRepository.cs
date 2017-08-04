using System;
using System.Collections.Generic;
using System.Linq;
using Battleship.Domain.CQRS;

namespace Battleship.Domain.CommandHandlers
{
    public interface ICommandRepository
    {
        void Save<T>(T command) where T : BaseCommand;
        IEnumerable<BaseCommand> Get(Guid tenantId, int skip, int take);
    }

    public class InMemoryCommandRepository : ICommandRepository
    {
        private readonly List<BaseCommand> _commands = new List<BaseCommand>();

        public void Save<T>(T command) where T : BaseCommand
        {
            _commands.Add(command);
        }

        public IEnumerable<BaseCommand> Get(Guid tenantId, int skip, int take)
        {
            return _commands.OrderBy(c => c.ReceivedOn).Skip(skip).Take(take);
        }
    }
}