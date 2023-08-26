using Battleship.Application.CommandHandlers;
using Battleship.Domain.Aggregates.Game;
using Battleship.Domain.Aggregates.Game.Events;
using Battleship.Domain.Commands;
using Battleship.Domain.Core.Messaging;
using Battleship.Domain.Core.Services.Persistence.Commands;
using Battleship.Domain.Core.Services.Persistence.EventSource.Aggregates;
using Moq;
using NodaTime;
using Xunit;

namespace Battleship.Tests
{
    public class GameCommandHandlerTests
    {
        [Fact]
        public async void GameCommandHandlerHandlesCreateGameCommand()
        {
            // Arrange
            var mockAggregateRepo = new Mock<IAggregateRepository<Game>>();
            var mockCommandReport = new Mock<ICommandRepository>();
            var newGameGuid = Guid.NewGuid();
            var fakeCommand = new CreateGame(8, 
                new AggregateParams(newGameGuid.ToString(), -1, false, Guid.Empty), 
                new EventParams("", Instant.MinValue, "", Guid.Empty));
            var sut = new GameCommandHandler(mockAggregateRepo.Object, mockCommandReport.Object);

            // Act
            await sut.Handle(fakeCommand, CancellationToken.None);

            //Assert
            mockAggregateRepo.Verify(m => m.SaveAsync(It.IsAny<Game>(), -1, true, false), Times.Once);
        }

        [Fact]
        public async void GameCommandHandlerHandlesUpdatePlayerNameCommand()
        {
            // Arrange
            var mockAggregateRepo = new Mock<IAggregateRepository<Game>>();
            var mockCommandReport = new Mock<ICommandRepository>();
            var newGameGuid = Guid.NewGuid();
            var fakeGame = new Game(newGameGuid, 8, new EventParams("", Instant.MinValue, "", Guid.Empty));
            fakeGame.MarkChangesAsCommitted();
            mockAggregateRepo.Setup(m => m.GetAsync(newGameGuid.ToString()))
                .Returns(Task.FromResult(fakeGame));
            var updatePlayerCommand = new UpdatePlayerName("Dave", 0, 
                new AggregateParams(fakeGame.AggregateId, 0, false, Guid.Empty), 
                new EventParams("", Instant.MinValue, "", Guid.Empty));
            var sut = new GameCommandHandler(mockAggregateRepo.Object, mockCommandReport.Object);

            // Act
            await sut.Handle(updatePlayerCommand, CancellationToken.None);

            //Assert
            mockAggregateRepo.Verify(m => m.GetAsync(newGameGuid.ToString()), Times.Once);
            mockAggregateRepo.Verify(m => m.SaveAsync(It.IsAny<Game>(), It.IsAny<int>(), true, false), Times.Once);
            Assert.Single(fakeGame.GetUncommittedChanges());
            Assert.Equal(typeof(PlayerNameUpdated), fakeGame.GetUncommittedChanges().First().GetType());
        }
    }
}