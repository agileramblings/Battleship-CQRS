using System;
using System.Threading.Tasks;
using Battleship.Domain.CQRS.Persistence;
using Battleship.Domain.EventHandlers;
using Battleship.Domain.Events;
using Battleship.Domain.ReadModel;
using Moq;
using Xunit;

namespace Battleship.Tests
{
    public class GameEventHandlerTests
    {
        public async void AGameEventHandlerShouldHandleThePlayerNameUpdatedEvent()
        {
            var mockReadFac = new Mock<IReadModelFacade>();
            var mockReadPer = new Mock<IReadModelPersistence>();
            var fakeEvent = new PlayerNameUpdated(Guid.Empty, "Dave", 1) {Version = 1};
            var gameDetails = new GameDetails();

            mockReadFac.Setup(m => m.Get<GameDetails>(Guid.Empty)).Returns(Task.FromResult(gameDetails));
            var sut = new GameEventHandler(mockReadFac.Object, mockReadPer.Object);
            await sut.Handle(fakeEvent);

            Assert.Equal("Dave", gameDetails.Players[1].Name);
            Assert.Equal(1, gameDetails.Version);

            mockReadPer.Verify(m => m.Put(It.IsAny<GameDetails>()), Times.Once);
        }

        [Fact]
        public async void AGameEventHandlerShouldHandleTheGameCreatedEvent()
        {
            var mockReadFac = new Mock<IReadModelFacade>();
            var mockReadPer = new Mock<IReadModelPersistence>();
            var newGuid = Guid.NewGuid();
            var fakeEvent = new GameCreated(newGuid);

            var sut = new GameEventHandler(mockReadFac.Object, mockReadPer.Object);

            await sut.Handle(fakeEvent);

            mockReadPer.Verify(m => m.Put(It.IsAny<GameDetails>()), Times.Once);
        }
    }
}