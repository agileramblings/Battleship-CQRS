using Battleship.Domain.Commands;
using Battleship.Domain.CQRS.Persistence;

namespace Battleship.Domain.CommandHandlers
{
    // This command handler is responsible for manipulating the AggregateRoot 
    // which will generate internal events for the aggregate
    // that will be used to alter the ReadModel 
    public class GameCommandHandler :
        ICommandHandler<CreateGame>,
        ICommandHandler<UpdatePlayerName>,
        ICommandHandler<AddShip>,
        ICommandHandler<FireShot>
    {
        private readonly IAggregateRepository _store;

        public GameCommandHandler(IAggregateRepository store)
        {
            _store = store;
        }

        public void Handle(AddShip command)
        {
            var aggregateGame = _store.GetById<Game>(command.GameId);
            aggregateGame.AddShip(command.ShipDetails, command.PlayerIndex);
            _store.Save(aggregateGame, aggregateGame.Version);
        }

        public void Handle(CreateGame command)
        {
            var newGame = new Game(command.CommandId, command.BoardSize);
            _store.Save(newGame, -1);
        }

        public void Handle(FireShot command)
        {
            var aggregateGame = _store.GetById<Game>(command.GameId);
            aggregateGame.FireShot(command.Target, command.AttackingPlayerIndex, command.TargetPlayerIndex);
            _store.Save(aggregateGame, aggregateGame.Version);
        }

        public void Handle(UpdatePlayerName command)
        {
            var aggregateGame = _store.GetById<Game>(command.GameId);
            aggregateGame.UpdatePlayer(command.Name, command.Position);
            _store.Save(aggregateGame, aggregateGame.Version);
        }
    }
}