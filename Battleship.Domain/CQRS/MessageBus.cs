using System;
using System.Linq;
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

        public void Send<T>(T command) where T : Command
        {
            var handlers = _commandHandlers.GetHandlers<T>().ToList();

            if (!handlers.Any()) throw new InvalidOperationException($"no command handler registered for {typeof(T)}");
            foreach (var h in handlers)
            {
                h.Handle(command);
            }
        }

        public void Publish<T>(T @event) where T : Event
        {
            var t = @event.GetType();
            var handlers = _eventHandlers.GetHandlers<T>().ToList();

            if (!handlers.Any()) throw new InvalidOperationException($"no event handler registered for {typeof(T)}");

            foreach (var handler in handlers)
            {
                handler.Handle(@event);
            }
        }
    }

    public interface IHandles<T>
    {
        void Handle(T message);
    }

    public interface ICommandSender
    {
        void Send<T>(T command) where T : Command;
    }

    public interface IEventPublisher
    {
        void Publish<T>(T @event) where T : Event;
    }
}