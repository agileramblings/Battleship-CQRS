using System.Collections.Generic;
using Autofac;
using Battleship.Domain.CQRS;

namespace Battleship.Domain.EventHandlers
{
    public interface IEventHandlerFactory
    {
        IEnumerable<IHandles<T>> GetHandlers<T>() where T : Event;
    }

    public class EventHandlerFactory : IEventHandlerFactory
    {
        private readonly IComponentContext _container;

        public EventHandlerFactory(IComponentContext container)
        {
            _container = container;
        }

        public IEnumerable<IHandles<T>> GetHandlers<T>() where T : Event
        {
            return _container.Resolve<IEnumerable<IHandles<T>>>();
        }
    }
}