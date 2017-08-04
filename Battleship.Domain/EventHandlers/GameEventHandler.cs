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

        public void Handle(GameCreated message)
        {
            var newGame = new GameDetails
            {
                Id = message.Id,
                ActivatedOn = message.CreatedOn,
                Version = message.Version
            };
            _save.Put(newGame);
        }

        public void Handle(PlayerNameUpdated message)
        {
            var gameToUpdate = _read.Get<GameDetails>(message.GameId);
            gameToUpdate.Players[message.Position].Name = message.Name;
            gameToUpdate.Version = message.Version;
            _save.Put(gameToUpdate);
        }

        public void Handle(ShipAdded message)
        {
            var gameToUpdate = _read.Get<GameDetails>(message.GameId);
            gameToUpdate.Players[message.PlayerIndex].Board.AddShip(message.ShipToAdd);
            gameToUpdate.Version = message.Version;
            _save.Put(gameToUpdate);
        }

        public void Handle(ShotFired message)
        {
            var gameToUpdate = _read.Get<GameDetails>(message.GameId);
            gameToUpdate.Players[message.AggressorPlayer].Board.AddShotFired(message.Target);
            gameToUpdate.Players[message.TargetPlayer].Board.AddShotReceived(message.Target);
            gameToUpdate.Turn += 1;
            gameToUpdate.Version = message.Version;
            _save.Put(gameToUpdate);
        }

        public void Handle(BoardSizeSet message)
        {
            var gameToUpdate = _read.Get<GameDetails>(message.GameId);
            gameToUpdate.Dimensions = message.Size;
            foreach (var player in gameToUpdate.Players)
            {
                player.Board.Dimensions = message.Size;
            }
            gameToUpdate.Version = message.Version;
            _save.Put(gameToUpdate);
        }
    }
}