using Battleship.Domain;
using Battleship.Domain.Entities;
using Battleship.Domain.Enums;
using Xunit;

namespace Battleship.Tests;

public class BoardTests
{
    public static IEnumerable<object[]> GoodShipPlacements
    {
        get
        {
            // Heading North, row should increase
            yield return new object[]
            {
                new Location('a', 1), Heading.N
            };
            // heading South, row should decrease
            yield return new object[]
            {
                new Location('c', 1), Heading.S
            };
            // heading West, col should increase
            yield return new object[]
            {
                new Location('a', 1), Heading.W
            };
            // heading East, col should decrease
            yield return new object[]
            {
                new Location('a', 3), Heading.E
            };
        }
    }

    public static IEnumerable<object[]> BadShipPlacements
    {
        get
        {
            // Heading South, row should decrease off the board
            yield return new object[]
            {
                new Location('a', 1), Heading.S
            };
            // heading East, col should decrease off the board
            yield return new object[]
            {
                new Location('a', 1), Heading.E
            };
        }
    }

    [Theory]
    [MemberData(nameof(GoodShipPlacements))]
    public void ABoardWillIndicateIfAShipIsInAValidPosition(Location bowAt, Heading heading)
    {
        var shipToPlace = ShipFactory.BuildShip(ShipType.Cruiser, bowAt, heading);

        var sut = new Board (8);
        var (boatPlaced, message) = sut.AddShip(shipToPlace);
        Assert.True(boatPlaced);
        Assert.Equal("Ship added.", message);
    }

    [Theory]
    [MemberData(nameof(BadShipPlacements))]
    public void ABoardWillIndicateIfAShipIsInAnInvalidPosition(Location bowAt, Heading heading)
    {
        var shipToPlace = ShipFactory.BuildShip(ShipType.Cruiser, bowAt, heading);

        var sut = new Board (8);
        var (added, message) = sut.AddShip(shipToPlace);
        Assert.False(added);
        Assert.Equal("Ship would not be completely on the board.", message);
    }

    [Fact]
    public void ABoardWillAllowIndicateThatTwoShipThatDoNotOverlapAreOk()
    {
        var sut = new Board (8);

        var shipOne = ShipFactory.BuildShip(ShipType.Cruiser, new Location('a', 1), Heading.N);
        Assert.True(sut.AddShip(shipOne).Added);
        Assert.True(sut.HasActiveShips);
        Assert.Single(sut.Ships);

        var shipTwo = ShipFactory.BuildShip(ShipType.Cruiser, new Location('a', 5), Heading.N);
        Assert.True(sut.AddShip(shipTwo).Added);
        Assert.True(sut.HasActiveShips);
        Assert.Equal(2, sut.Ships.Count());
    }

    [Fact]
    public void ABoardWillAllowIndicateThatTwoShipThatOverlapAreBad()
    {
        var sut = new Board(8);

        var shipOne = ShipFactory.BuildShip(ShipType.Cruiser, new Location('a', 1), Heading.N);
        Assert.True(sut.AddShip(shipOne).Added);
        Assert.Single(sut.Ships);

        var shipTwo = ShipFactory.BuildShip(ShipType.Cruiser, new Location('c', 1), Heading.N);
        var (added, message) = sut.AddShip(shipTwo);
        Assert.False(added);
        Assert.Equal("Another ship already occupies some or all of these spaces.", message);
        Assert.Single(sut.Ships);
    }

    [Fact]
    public void BoardShouldReportActiveBoatsIfAtLeastOneBoatIsActive()
    {
        var sut = new Board(8);
        var shipOne = ShipFactory.BuildShip(ShipType.Cruiser, new Location('a', 1), Heading.N);
        sut.AddShip(shipOne);
        Assert.Single(sut.Ships);
        var shipTwo = ShipFactory.BuildShip(ShipType.Cruiser, new Location('g', 4), Heading.E);
        sut.AddShip(shipTwo);
        Assert.Equal(2, sut.Ships.Count());

        var result = sut.AddShotReceived(new Location('a', 1));
        Assert.True(result.Hit);
        Assert.Equal("Hit!", result.Message);
        result = sut.AddShotReceived(new Location('b', 1));
        Assert.True(result.Hit);
        Assert.Equal("Hit!", result.Message);
        result = sut.AddShotReceived(new Location('c', 1));
        Assert.True(result.Hit);
        Assert.True(result.SunkShip);
        Assert.Equal($"You sank the {ShipType.Cruiser.Name}!", result.Message);

        Assert.Equal(1, sut.Ships.Count(s => s.Status == ShipStatus.Active));
        Assert.True(sut.HasActiveShips);
    }
}