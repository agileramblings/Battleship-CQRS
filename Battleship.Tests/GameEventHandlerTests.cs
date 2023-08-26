using System;
using System.Threading.Tasks;
using Battleship.Domain.Aggregates.Game.Events;
using Battleship.Domain.Core.Services.Persistence.CQRS;
using Battleship.Domain.EventHandlers;
using Battleship.Domain.Projections;
using Moq;
using Xunit;

namespace Battleship.Tests
{
    //public class GameEventHandlerTests
    //{
    //    [Fact]
    //    public async void AGameEventHandlerShouldHandleThePlayerNameUpdatedEvent()
    //    {
    //        var mockReadFac = new Mock<IReadModelQuery>();
    //        var mockReadPer = new Mock<IReadModelPersistence>();
    //        var fakeEvent = new PlayerNameUpdated(Guid.Empty, "Dave", 1) {Version = 1};
    //        var gameDetails = new GameProjection();

    //        mockReadFac.Setup(m => m.GetItemAsync<GameProjection>(Guid.Empty, "")).Returns(Task.FromResult(gameDetails));
    //        var sut = new GameEventHandler(mockReadFac.Object, mockReadPer.Object);
    //        await sut.Handle(fakeEvent);

    //        Assert.Equal("Dave", gameDetails.Players[1].Name);
    //        Assert.Equal(1, gameDetails.Version);

    //        mockReadPer.Verify(m => m.Put(It.IsAny<GameDetails>()), Times.Once);
    //    }

    //    [Fact]
    //    public async void AGameEventHandlerShouldHandleTheGameCreatedEvent()
    //    {
    //        var mockReadFac = new Mock<IReadModelQuery>();
    //        var mockReadPer = new Mock<IReadModelPersistence>();
    //        var newGuid = Guid.NewGuid();
    //        var fakeEvent = new GameCreated(newGuid);

    //        var sut = new GameEventHandler(mockReadFac.Object, mockReadPer.Object);

    //        await sut.Handle(fakeEvent);

    //        mockReadPer.Verify(m => m.Put(It.IsAny<GameDetails>()), Times.Once);
    //    }
    //}
}