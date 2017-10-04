using System.Threading.Tasks;
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

        public async Task Handle(AddShip command)
        {
            var aggregateGame = await _store.GetById<Game>(command.GameId);
            if (aggregateGame.AddShip(command.ShipDetails, command.PlayerIndex))
            {
                // ship was added, persist aggregate
                await _store.Save(aggregateGame, aggregateGame.Version);
            }
        }

        public async Task Handle(CreateGame command)
        {
            var newGame = new Game(command.CommandId, command.BoardSize);
            await _store.Save(newGame, -1);
        }

        public async Task Handle(FireShot command)
        {
            var aggregateGame = await _store.GetById<Game>(command.GameId);
            aggregateGame.FireShot(command.Target, command.AttackingPlayerIndex, command.TargetPlayerIndex);
            await _store.Save(aggregateGame, aggregateGame.Version);
        }

        public async Task Handle(UpdatePlayerName command)
        {
            var aggregateGame = await _store.GetById<Game>(command.GameId);
            aggregateGame.UpdatePlayer(command.Name, command.Position);
            await _store.Save(aggregateGame, aggregateGame.Version);
        }
    }
}