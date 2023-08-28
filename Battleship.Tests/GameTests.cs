using Battleship.Domain;
using Battleship.Domain.Aggregates.Game;
using Battleship.Domain.Core.Messaging;
using Battleship.Domain.Entities;
using Battleship.Domain.Enums;
using Kekiri;
using Kekiri.Xunit;
using NodaTime;
using Xunit;

namespace Battleship.Tests
{
    public class GameTests : ScenarioBase
    {
        private Game? _sut = null;
        private readonly Guid _gameId = Guid.NewGuid();
        private const uint Dimensions = 8U;
        private readonly IClock _clock = SystemClock.Instance;


        [Scenario]
        public void CreateANewGame()
        {
            Given(No_game_exists);
            When(A_new_game_is_created);
            Then(The_game_object_should_be_correctly_instantiated);
        }

        private void No_game_exists()
        {
            _sut = null;
        }

        private void A_new_game_is_created()
        {
            _sut = new Game(_gameId, Dimensions, _clock.ToEventParams());
        }

        private void The_game_object_should_be_correctly_instantiated()
        {
            Assert.NotNull(_sut);
            Assert.Equal(8U, _sut.Dimensions);
            Assert.Equal(2, _sut.Players.Length);
            Assert.NotNull(_sut.Players.First().Board);
            Assert.NotNull(_sut.Players.Last().Board);
        }

        [Scenario]
        public void ShipsArePlaced()
        {
            Given(No_game_exists)
                .And(A_new_game_is_created);
            When(A_ship_is_placed_in_a_valid_location);
            Then(The_game_object_has_a_ship_on_player_0_board);
        }

        private void A_ship_is_placed_in_a_valid_location()
        {
            _sut?.AddShip(ShipFactory.BuildShip(ShipType.Cruiser, new Location('a', 1), Heading.N),
                0 /* player index*/, 
                _clock.ToEventParams());
        }

        private void The_game_object_has_a_ship_on_player_0_board()
        {
            Assert.NotNull(_sut);
            Assert.Single(_sut.Players[0].Board.Ships);
        }

        [Scenario]
        public void PlayerIsAttacked()
        {
            Given(No_game_exists)
                .And(A_new_game_is_created)
                .And(A_ship_is_placed_in_a_valid_location);
            When(A_ship_is_attacked_and_sunk);
            Then(The_game_is_over);
        }

        private void A_ship_is_attacked_and_sunk()
        {
            _sut?.FireShot(new Location('a',1), 1, 0, _clock.ToEventParams());
            _sut?.FireShot(new Location('b', 1), 1, 0, _clock.ToEventParams());
            _sut?.FireShot(new Location('c', 1), 1, 0, _clock.ToEventParams());
        }

        private void The_game_is_over()
        {
            Assert.NotNull(_sut);
            Assert.NotNull(_sut.Winner);
            Assert.Equal($"You sank the {ShipType.Cruiser.Name}!", _sut.LastMessage);
            Assert.NotNull(_sut.LastPlayer);
            Assert.Equal(1U, _sut.LastPlayer.Position);
            Assert.Equal(GameStatus.Over, _sut.GameStatus);
        }
    }
}