using System;
using System.Linq;
using System.Threading.Tasks;
using Battleship.Domain.CommandHandlers;
using Battleship.Domain.EventHandlers;

namespace Battleship.Domain.CQRS
{
    public class MessageBus : ICommandSender, IEventPublisher
    {
        private readonly ICommandHandlerFactory _commandHandlers;
        private readonly IEventHandlerFactory _eventHandlers;

        public MessageBus(ICommandHandlerFactory chf, IEventHandlerFactory ehf)
        {
            _commandHandlers = chf;
            _eventHandlers = ehf;
        }

        public Task Send<T>(T command) where T : Command
        {
            var handlers = _commandHandlers.GetHandlers<T>().ToList();

            if (!handlers.Any()) throw new InvalidOperationException($"no command handler registered for {typeof(T)}");
            foreach (var h in handlers)
            {
                h.Handle(command);
            }
            return Task.FromResult(0);
        }

        public Task Publish<T>(T @event) where T : Event
        {
            var t = @event.GetType();
            var handlers = _eventHandlers.GetHandlers<T>().ToList();

            if (!handlers.Any()) throw new InvalidOperationException($"no event handler registered for {typeof(T)}");

            foreach (var handler in handlers)
            {
                handler.Handle(@event);
            }
            return Task.FromResult(0);
        }
    }

    public interface IHandles<T>
    {
        Task Handle(T message);
    }

    public interface ICommandSender
    {
        Task Send<T>(T command) where T : Command;
    }

    public interface IEventPublisher
    {
        Task Publish<T>(T @event) where T : Event;
    }
}