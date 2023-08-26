using System.Threading.Tasks;
using Battleship.Domain.Core.Messaging;

namespace Battleship.Domain.Core.Services.Messaging.EventSource;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event) where T : EventBase;
}