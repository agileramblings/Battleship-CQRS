using System.Collections.Generic;
using System.Linq;
using Battleship.Domain.ReadModel;
using Battleship.Domain.ReadModel.Enums;
using Xunit;

namespace Battleship.Tests
{
    public class ShipDetailTests
    {
        public static IEnumerable<object[]> SuccessfulLocationsData
        {
            get
            {
                // Heading North, row should increase
                yield return new object[]
                {
                    new Location('a', 1), Direction.N,
                    new HashSet<int>
                    {
                        new Location('a', 1).GetHashCode(),
                        new Location('b', 1).GetHashCode(),
                        new Location('c', 1).GetHashCode()
                    }
                };
                // heading South, row should decrease
                yield return new object[]
                {
                    new Location('c', 1), Direction.S,
                    new HashSet<int>
                    {
                        new Location('c', 1).GetHashCode(),
                        new Location('b', 1).GetHashCode(),
                        new Location('a', 1).GetHashCode()
                    }
                };
                // heading West, col should increase
                yield return new object[]
                {
                    new Location('a', 1), Direction.W,
                    new HashSet<int>
                    {
                        new Location('a', 1).GetHashCode(),
                        new Location('a', 2).GetHashCode(),
                        new Location('a', 3).GetHashCode()
                    }
                };
                // heading East, col should decrease
                yield return new object[]
                {
                    new Location('a', 3), Direction.E,
                    new HashSet<int>
                    {
                        new Location('a', 3).GetHashCode(),
                        new Location('a', 2).GetHashCode(),
                        new Location('a', 1).GetHashCode()
                    }
                };
            }
        }

        public static IEnumerable<object[]> BadBoardLocationsData
        {
            get
            {
                // Heading South, row should decrease off the board
                yield return new object[]
                {
                    new Location('a', 1), Direction.S,
                    new HashSet<int>
                    {
                        new Location('a', 1).GetHashCode(),
                        new Location('@', 1).GetHashCode(),
                        new Location('?', 1).GetHashCode()
                    }
                };
                // heading East, col should decrease off the board
                yield return new object[]
                {
                    new Location('a', 1), Direction.E,
                    new HashSet<int>
                    {
                        new Location('a', 1).GetHashCode(),
                        new Location('a', 0).GetHashCode(),
                        new Location('a', uint.MaxValue).GetHashCode()
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(BadBoardLocationsData))]
        public void ShipShouldHaveProperLocationsUsingOffBoardInstructions(Location bowAt, Direction heading,
            HashSet<int> correctLocations)
        {
            var sut = new ShipDetails
            {
                ClassName = "Destroyer",
                ClassSize = 3,
                BowAt = bowAt,
                Heading = heading
            };

            Assert.NotNull(sut.Locations);
            var intersection = sut.LocationSet.Intersect(correctLocations);
            var locationsInIntersection = intersection.Count();
            Assert.Equal(3, locationsInIntersection);
        }

        [Theory]
        [MemberData(nameof(SuccessfulLocationsData))]
        public void ShipShouldHaveProperLocations(Location bowAt, Direction heading, HashSet<int> correctLocations)
        {
            var sut = new ShipDetails
            {
                ClassName = "Destroyer",
                ClassSize = 3,
                BowAt = bowAt,
                Heading = heading
            };

            Assert.NotNull(sut.Locations);
            var intersection = sut.LocationSet.Intersect(correctLocations);
            var locationsInIntersection = intersection.Count();
            Assert.Equal(3, locationsInIntersection);
        }

        [Fact]
        public void ShipShouldHaveLocationsWhenBowAtAndHeadingAreSet()
        {
            var sut = new ShipDetails
            {
                ClassName = "Destroyer",
                ClassSize = 3,
                BowAt = new Location('a', 1),
                Heading = Direction.N
            };

            Assert.NotNull(sut.Locations);
            Assert.True(sut.LocationSet.Contains(sut.BowAt.GetHashCode()));
            Assert.True(sut.LocationSet.Contains(new Location('b', 1).GetHashCode()));
            Assert.True(sut.LocationSet.Contains(new Location('c', 1).GetHashCode()));
        }

        [Fact]
        public void ShipShouldNotHaveLocationsWithoutBowAtPropertySet()
        {
            var sut = new ShipDetails
            {
                ClassName = "Destroyer",
                ClassSize = 3,
                BowAt = new Location('a', 1)
            };
            Assert.Equal(0, sut.Locations.Count);
        }

        [Fact]
        public void ShipShouldNotHaveLocationsWithoutHeadingPropertySet()
        {
            var sut = new ShipDetails
            {
                ClassName = "Destroyer",
                ClassSize = 3,
                Heading = Direction.None
            };
            Assert.Equal(0, sut.Locations.Count);
        }
    }
}