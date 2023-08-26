using Battleship.Domain.Aggregates.Game;
using Battleship.Domain.Aggregates.Game.Events;
using Battleship.Domain.Core.Services.Persistence.CQRS;
using Battleship.Domain.Core.Services.Persistence.EventSource.Aggregates;
using MediatR;

namespace Battleship.Domain.EventHandlers
{
    public class GameEventHandler :
        INotificationHandler<GameCreated>,
        INotificationHandler<PlayerNameUpdated>,
        INotificationHandler<ShipAdded>,
        INotificationHandler<ShotFired>,
        INotificationHandler<GameWon>
    {
        private readonly IReadModelQuery _read;
        private readonly IReadModelPersistence _save;
        private readonly IAggregateRepository<Game> _repo;

        public GameEventHandler(IAggregateRepository<Game> repo, IReadModelQuery read, IReadModelPersistence save)
        {
            _repo = repo;
            _read = read;
            _save = save;
        }

        public async Task Handle(GameCreated message, CancellationToken cancellationToken)
        {
            var game = await _repo.GetAsync(message.AggParams.AggregateId);
            var projection = game.ToProjection();
            await _save.PutAggregateAsync(projection, "");

            // get GameList projection

            // add game to game list projection

            // save game list projection
        }

        public async Task Handle(PlayerNameUpdated message, CancellationToken cancellationToken)
        {
            var game = await _repo.GetAsync(message.AggParams.AggregateId);
            var projection = game.ToProjection();
            await _save.PutAggregateAsync(projection, "");
        }

        public async Task Handle(ShipAdded message, CancellationToken cancellationToken)
        {
            var game = await _repo.GetAsync(message.AggParams.AggregateId);
            var projection = game.ToProjection();
            await _save.PutAggregateAsync(projection, "");
        }

        public async Task Handle(ShotFired message, CancellationToken cancellationToken)
        {
            var game = await _repo.GetAsync(message.AggParams.AggregateId);
            var projection = game.ToProjection();
            await _save.PutAggregateAsync(projection, "");
        }

        public async Task Handle(GameWon message, CancellationToken cancellationToken)
        {
            var game = await _repo.GetAsync(message.AggParams.AggregateId);
            var projection = game.ToProjection();
            await _save.PutAggregateAsync(projection, "");
            // get GameList projection

            // add game to game list projection

            // save game list projection
        }
    }
}