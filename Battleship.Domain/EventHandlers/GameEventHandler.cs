using System.Threading.Tasks;
using Battleship.Domain.CQRS;
using Battleship.Domain.CQRS.Persistence;
using Battleship.Domain.Events;
using Battleship.Domain.ReadModel;

namespace Battleship.Domain.EventHandlers
{
    public class GameEventHandler :
        IHandles<GameCreated>,
        IHandles<BoardSizeSet>,
        IHandles<PlayerNameUpdated>,
        IHandles<ShipAdded>,
        IHandles<ShotFired>
    {
        private readonly IReadModelFacade _read;
        private readonly IReadModelPersistence _save;

        public GameEventHandler(IReadModelFacade read, IReadModelPersistence save)
        {
            _read = read;
            _save = save;
        }

        public async Task Handle(GameCreated message)
        {
            var newGame = new GameDetails
            {
                Id = message.Id,
                ActivatedOn = message.CreatedOn,
                Version = message.Version
            };
            await _save.Put(newGame);
        }

        public async Task Handle(PlayerNameUpdated message)
        {
            var gameToUpdate = await _read.Get<GameDetails>(message.GameId);
            gameToUpdate.Players[message.Position].Name = message.Name;
            gameToUpdate.Version = message.Version;
            await _save.Put(gameToUpdate);
        }

        public async Task Handle(ShipAdded message)
        {
            var gameToUpdate = await _read.Get<GameDetails>(message.GameId);
            gameToUpdate.Players[message.PlayerIndex].Board.AddShip(message.ShipToAdd);
            gameToUpdate.Version = message.Version;
            await _save.Put(gameToUpdate);
        }

        public async Task Handle(ShotFired message)
        {
            var gameToUpdate = await _read.Get<GameDetails>(message.GameId);
            gameToUpdate.Players[message.AggressorPlayer].Board.AddShotFired(message.Target);
            gameToUpdate.Players[message.TargetPlayer].Board.AddShotReceived(message.Target);
            gameToUpdate.Turn += 1;
            gameToUpdate.Version = message.Version;
            await _save.Put(gameToUpdate);
        }

        public async Task Handle(BoardSizeSet message)
        {
            var gameToUpdate = await _read.Get<GameDetails>(message.GameId);
            gameToUpdate.Dimensions = message.Size;
            foreach (var player in gameToUpdate.Players)
            {
                player.Board.Dimensions = message.Size;
            }
            gameToUpdate.Version = message.Version;
            await _save.Put(gameToUpdate);
        }
    }
}