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

        public void Handle(CreateGame command)
        {
            Command.Save(command);
        }

        public void Handle(UpdatePlayerName command)
        {
            Command.Save(command);
        }

        public void Handle(AddShip command)
        {
            Command.Save(command);
        }

        public void Handle(FireShot command)
        {
            Command.Save(command);
        }
    }
}