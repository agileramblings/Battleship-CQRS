using Battleship.Domain.Aggregates.Game;
using Battleship.Domain.Commands;
using Battleship.Domain.Core.Services.Persistence.Commands;
using Battleship.Domain.Core.Services.Persistence.EventSource.Aggregates;
using MediatR;

namespace Battleship.Application.CommandHandlers;

// This command handler is responsible for manipulating the AggregateRoot 
// which will generate internal events for the aggregate
// that will be used to alter the ReadModel 
public class GameCommandHandler :
    IRequestHandler<CreateGame>,
    IRequestHandler<UpdatePlayerName>,
    IRequestHandler<AddShip>,
    IRequestHandler<FireShot>
{
    private readonly IAggregateRepository<Game> _store;
    private readonly ICommandRepository _cmdRepo;

    public GameCommandHandler(IAggregateRepository<Game> store, ICommandRepository cmd)
    {
        _cmdRepo = cmd;
        _store = store;
    }

    public async Task Handle(CreateGame request, CancellationToken cancellationToken)
    {
        await _cmdRepo.SaveAsync(request);
        var newGame = new Game(Guid.Parse(request.AggParams.AggregateId), request.BoardSize, request.EventParams);
        await _store.SaveAsync(newGame, -1);
    }

    public async Task Handle(UpdatePlayerName request, CancellationToken cancellationToken)
    {
        await _cmdRepo.SaveAsync(request);
        var aggregateGame = await _store.GetAsync(request.AggParams.AggregateId);
        aggregateGame.UpdatePlayerName(request.NewName, request.Position, request.EventParams);
        await _store.SaveAsync(aggregateGame, aggregateGame.Version);
    }

    public async Task Handle(AddShip request, CancellationToken cancellationToken)
    {
        await _cmdRepo.SaveAsync(request);
        var aggregateGame = await _store.GetAsync(request.AggParams.AggregateId);
        if (aggregateGame.AddShip(request.ShipDetails, request.PlayerIndex, request.EventParams))
        {
            // ship was added, persist aggregate
            await _store.SaveAsync(aggregateGame, aggregateGame.Version);
        }
    }

    public async Task Handle(FireShot request, CancellationToken cancellationToken)
    {
        await _cmdRepo.SaveAsync(request);
        var aggregateGame = await _store.GetAsync(request.AggParams.AggregateId);
        aggregateGame.FireShot(request.Target, request.AttackingPlayerIndex, request.TargetPlayerIndex, request.EventParams);
        await _store.SaveAsync(aggregateGame, aggregateGame.Version);
    }
}