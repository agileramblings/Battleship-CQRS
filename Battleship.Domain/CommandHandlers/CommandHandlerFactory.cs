using System.Collections.Generic;
using Autofac;
using Battleship.Domain.CQRS;

namespace Battleship.Domain.CommandHandlers
{
    public interface ICommandHandler<T>
    {
        void Handle(T command);
    }

    public interface ICommandHandlerFactory
    {
        IEnumerable<ICommandHandler<T>> GetHandlers<T>() where T : Command;
    }

    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IComponentContext _container;

        public CommandHandlerFactory(IComponentContext container)
        {
            _container = container;
        }

        public IEnumerable<ICommandHandler<T>> GetHandlers<T>() where T : Command
        {
            return _container.Resolve<IEnumerable<ICommandHandler<T>>>();
        }
    }
}