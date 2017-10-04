using System;
using System.Linq;
using System.Threading.Tasks;
using Battleship.Domain;
using Battleship.Domain.CommandHandlers;
using Battleship.Domain.Commands;
using Battleship.Domain.CQRS.Persistence;
using Battleship.Domain.Events;
using Moq;
using Xunit;

namespace Battleship.Tests
{
    public class GameCommandHandlerTests
    {
        [Fact]
        public async void GameCommandHandlerHandlesCreateGameCommand()
        {
            // Arrange
            var mockAggregateRepo = new Mock<IAggregateRepository>();
            var newGameGuid = Guid.NewGuid();
            var fakeCommand = new CreateGame(newGameGuid, 8);
            var sut = new GameCommandHandler(mockAggregateRepo.Object);

            // Act
            await sut.Handle(fakeCommand);

            //Assert
            mockAggregateRepo.Verify(m => m.Save(It.IsAny<Game>(), -1), Times.Once);
        }

        [Fact]
        public async void GameCommandHandlerHandlesUpdatePlayerNameCommand()
        {
            // Arrange
            var mockAggregateRepo = new Mock<IAggregateRepository>();
            var fakeGame = new Game();
            mockAggregateRepo.Setup(m => m.GetById<Game>(Guid.Empty)).Returns(Task.FromResult(fakeGame));
            var newCommandId = Guid.NewGuid();
            var fakeCommand = new UpdatePlayerName(newCommandId, 0, Guid.Empty, "Dave", 1);
            var sut = new GameCommandHandler(mockAggregateRepo.Object);

            // Act
            await sut.Handle(fakeCommand);

            //Assert
            mockAggregateRepo.Verify(m => m.GetById<Game>(Guid.Empty), Times.Once);
            mockAggregateRepo.Verify(m => m.Save(It.IsAny<Game>(), It.IsAny<int>()), Times.Once);
            Assert.Equal(1, fakeGame.GetUncommittedChanges().Count());
            Assert.Equal(typeof(PlayerNameUpdated), fakeGame.GetUncommittedChanges().First().GetType());
        }
    }
}