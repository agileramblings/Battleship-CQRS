using System.Threading.Tasks;
using Battleship.Domain.Commands;

namespace Battleship.Domain.CommandHandlers
{
    // This GenericCommandHandler is used only to persist commands
    // Commands could be used in the future to "replay" interactions with the user
    public class GenericCommandHandler :
        ICommandHandler<CreateGame>,
        ICommandHandler<UpdatePlayerName>,
        ICommandHandler<AddShip>,
        ICommandHandler<FireShot>
    {
        public GenericCommandHandler(ICommandRepository cmd)
        {
            Command = cmd;
        }

        private ICommandRepository Command { get; }

        public async Task Handle(CreateGame command)
        {
            await Command.Save(command);
        }

        public async Task Handle(UpdatePlayerName command)
        {
            await Command.Save(command);
        }

        public async Task Handle(AddShip command)
        {
            await Command.Save(command);
        }

        public async Task Handle(FireShot command)
        {
            await Command.Save(command);
        }
    }
}