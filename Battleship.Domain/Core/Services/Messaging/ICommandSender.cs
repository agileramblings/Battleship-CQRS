using Battleship.Domain.Core.Messaging;

namespace Battleship.Domain.Core.Services.Messaging;

public interface ICommandSender
{
    Task SendAsync<T>(T command) where T : CommandBase;
}