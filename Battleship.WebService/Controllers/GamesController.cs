using System;
using System.Threading.Tasks;
using Battleship.Domain;
using Battleship.Domain.Commands;
using Battleship.Domain.CQRS;
using Battleship.Domain.CQRS.Persistence;
using Battleship.Domain.ReadModel;
using Battleship.Domain.ReadModel.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Battleship.WebService.Controllers
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly ICommandSender _bus;
        private readonly IReadModelFacade _read;
        private readonly IAggregateRepository _aggRepo;

        public GamesController(ICommandSender bus, IReadModelFacade read, IAggregateRepository aggRepo)
        {
            _bus = bus;
            _read = read;
            _aggRepo = aggRepo;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(new[] {new GameListItem(), new GameListItem()});
        }

        // GET api/values/5
        [HttpGet("{gameId:guid}")]
        public async Task<IActionResult> Get(Guid gameId)
        {
            var game = await _read.Get<GameDetails>(gameId);
            return Json(game);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> CreateGame()
        {
            var gameId = Guid.NewGuid();
            var cmd = new CreateGame(gameId, 10);
            await _bus.Send(cmd);
            return new RedirectToActionResult("Get", "Games", new {gameId});
        }

        // POST api/values
        [HttpPost]
        [Route("{gameId:guid}/player/{position:int}/{name}")]
        public async Task<IActionResult> AddPlayer(Guid gameId, uint position, string name, int gameVersion)
        {
            await _bus.Send(new UpdatePlayerName(Guid.NewGuid(), gameVersion, gameId, name, position));
            return new RedirectToActionResult("Get", "Games", new {gameId});
        }

        // PUT api/values/5
        [HttpPost]
        [Route("{gameId:guid}/{attackingPlayerIndex:int}/{targetPlayer:int}/{row}/{column:int}")]
        public async Task<IActionResult> FireShot(Guid gameId, uint attackingPlayerIndex, uint targetPlayer, char row, uint column, int gameVersion)
        {
            var game = await _aggRepo.GetById<Game>(gameId);
            var location = new Location(row, column);

            // See if user inputs are valid
            if (!game.ValidLocation(location, targetPlayer))
                return new BadRequestResult();

            await _bus.Send(new FireShot(Guid.NewGuid(), gameVersion, gameId, new Location(row, column), attackingPlayerIndex, targetPlayer));

            return new RedirectToActionResult("Get", "Games", new { gameId });
        }

        [HttpPost]
        [Route("{gameId:guid}/{playerPosition:int}/boat/{type}/{row}/{column:int}/{heading}")]
        public async Task<IActionResult> PlaceShip(Guid gameId, uint playerPosition, string type, char row, uint column, char heading, int gameVersion)
        {
            var game = await _aggRepo.GetById<Game>(gameId);
            var location = new Location(row, column);

            // See if user inputs are valid
            if (!game.ValidLocation(location, playerPosition))
                return new BadRequestResult();

            var shipToAdd = new ShipDetails
            {
                BowAt = location,
                Heading = ConvertToHeading(heading),
                ClassSize = 3,
                ClassName = "Destroyer",
                Status = ShipStatus.Active
            };

            // see if ship can be placed here
            var goodLocation = game.CanAddShip(shipToAdd, playerPosition);
            if (!goodLocation)
                return new BadRequestResult();
            var newCommandId = Guid.NewGuid();
            await _bus.Send(new AddShip(newCommandId, gameVersion, gameId, playerPosition, shipToAdd));

            return new RedirectToActionResult("Get", "Games", new { gameId });
        }

        private static Direction ConvertToHeading(char heading)
        {
            switch (char.ToUpper(heading))
            {
                case 'N': return Direction.N;
                case 'E': return Direction.E;
                case 'W': return Direction.W;
                case 'S': return Direction.S;
                default:
                    throw new ArgumentException($"You cannot convert {heading} into a Direction enumeration.",
                        nameof(heading));
            }
        }
    }
}