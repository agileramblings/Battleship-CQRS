using System;
using System.Collections.Generic;
using Battleship.Domain.ReadModel;
using Battleship.Domain.ReadModel.Enums;
using Xunit;

namespace Battleship.Tests
{
    public class BoardTests
    {
        public static IEnumerable<object[]> GoodShipPlacements
        {
            get
            {
                // Heading North, row should increase
                yield return new object[]
                {
                    new Location('a', 1), Direction.N
                };
                // heading South, row should decrease
                yield return new object[]
                {
                    new Location('c', 1), Direction.S
                };
                // heading West, col should increase
                yield return new object[]
                {
                    new Location('a', 1), Direction.W
                };
                // heading East, col should decrease
                yield return new object[]
                {
                    new Location('a', 3), Direction.E
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
                    new Location('a', 1), Direction.S
                };
                // heading East, col should decrease off the board
                yield return new object[]
                {
                    new Location('a', 1), Direction.E
                };
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(8)]
        public void BoardShouldHaveDimensionSquaredValidLocations(uint dimension)
        {
            var sut = new Board {Dimensions = dimension};
            Assert.NotNull(sut.ValidLocations);
            Assert.Equal((int) Math.Pow(dimension, 2), sut.ValidLocations.Count);
        }

        [Theory]
        [MemberData(nameof(GoodShipPlacements))]
        public void ABoardWillIndicateIfAShipIsInAValidPosition(Location bowAt, Direction heading)
        {
            var shipToPlace = new ShipDetails
            {
                ClassName = "Destroyer",
                ClassSize = 3,
                BowAt = bowAt,
                Heading = heading
            };

            var sut = new Board {Dimensions = 8};
            Assert.True(sut.ShipFitsOnBoard(shipToPlace));
        }

        [Theory]
        [MemberData(nameof(BadShipPlacements))]
        public void ABoardWillIndicateIfAShipIsInAnInvalidPosition(Location bowAt, Direction heading)
        {
            var shipToPlace = new ShipDetails
            {
                ClassName = "Destroyer",
                ClassSize = 3,
                BowAt = bowAt,
                Heading = heading
            };

            var sut = new Board {Dimensions = 8};
            Assert.False(sut.ShipFitsOnBoard(shipToPlace));
        }

        [Fact]
        public void ABoardWillAllowIndicateThatTwoShipThatDoNotOverlapAreOk()
        {
            var sut = new Board {Dimensions = 8};

            var shipOne = new ShipDetails
            {
                ClassName = "Destroyer",
                ClassSize = 3,
                BowAt = new Location('a', 1),
                Heading = Direction.N
            };

            sut.AddShip(shipOne);
            Assert.Equal(1, sut.Ships.Count);

            var shipTwo = new ShipDetails
            {
                ClassName = "Destroyer",
                ClassSize = 3,
                BowAt = new Location('a', 5),
                Heading = Direction.N
            };

            Assert.True(sut.ShipFitsOnBoard(shipTwo));

            sut.AddShip(shipTwo);
            Assert.Equal(2, sut.Ships.Count);
        }

        [Fact]
        public void ABoardWillAllowIndicateThatTwoShipThatOverlapAreBad()
        {
            var sut = new Board {Dimensions = 8};

            var shipOne = new ShipDetails
            {
                ClassName = "Destroyer",
                ClassSize = 3,
                BowAt = new Location('a', 1),
                Heading = Direction.N
            };

            sut.AddShip(shipOne);
            Assert.Equal(1, sut.Ships.Count);

            var shipTwo = new ShipDetails
            {
                ClassName = "Destroyer",
                ClassSize = 3,
                BowAt = new Location('c', 1),
                Heading = Direction.N
            };

            Assert.False(sut.ShipFitsOnBoard(shipTwo));

            sut.AddShip(shipTwo);
            Assert.Equal(1, sut.Ships.Count);
        }

        [Fact]
        public void BoardCanHaveZeroValidLocations()
        {
            var sut = new Board();
            Assert.NotNull(sut.ValidLocations);
            Assert.Equal(0, sut.ValidLocations.Count);
        }

        [Fact]
        public void BoardShouldReportNoActiveBoatsIfNoBoatsAreActive()
        {
            var sut = new Board { Dimensions = 8 };

            var shipOne = new ShipDetails
            {
                ClassName = "Destroyer",
                ClassSize = 3,
                BowAt = new Location('a', 1),
                Heading = Direction.N,
                Status = ShipStatus.Sunk
            };

            sut.AddShip(shipOne);
            Assert.Equal(1, sut.Ships.Count);
            Assert.False(sut.HasActiveShips);
        }

        [Fact]
        public void BoardShouldReportActiveBoatsIfAtLeastOneBoatIsActive()
        {
            var sut = new Board { Dimensions = 8 };
            var shipOne = new ShipDetails
            {
                ClassName = "Destroyer",
                ClassSize = 3,
                BowAt = new Location('a', 1),
                Heading = Direction.N,
                Status = ShipStatus.Sunk
            };

            sut.AddShip(shipOne);
            Assert.Equal(1, sut.Ships.Count);
            var shipTwo = new ShipDetails
            {
                ClassName = "Destroyer",
                ClassSize = 3,
                BowAt = new Location('g', 4),
                Heading = Direction.E,
                Status = ShipStatus.Active
            };

            sut.AddShip(shipTwo);
            Assert.Equal(2, sut.Ships.Count);
            Assert.True(sut.HasActiveShips);
        }
    }
}