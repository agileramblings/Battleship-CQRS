using System.Collections.Generic;
using System.Linq;
using Battleship.Domain;
using Battleship.Domain.Entities;
using Xunit;

namespace Battleship.Tests
{
    //public class ShipDetailTests
    //{
    //    public static IEnumerable<object[]> SuccessfulLocationsData
    //    {
    //        get
    //        {
    //            // Heading North, row should increase
    //            yield return new object[]
    //            {
    //                new Location('a', 1), Heading.N,
    //                new HashSet<int>
    //                {
    //                    new Location('a', 1).GetHashCode(),
    //                    new Location('b', 1).GetHashCode(),
    //                    new Location('c', 1).GetHashCode()
    //                }
    //            };
    //            // heading South, row should decrease
    //            yield return new object[]
    //            {
    //                new Location('c', 1), Heading.S,
    //                new HashSet<int>
    //                {
    //                    new Location('c', 1).GetHashCode(),
    //                    new Location('b', 1).GetHashCode(),
    //                    new Location('a', 1).GetHashCode()
    //                }
    //            };
    //            // heading West, col should increase
    //            yield return new object[]
    //            {
    //                new Location('a', 1), Heading.W,
    //                new HashSet<int>
    //                {
    //                    new Location('a', 1).GetHashCode(),
    //                    new Location('a', 2).GetHashCode(),
    //                    new Location('a', 3).GetHashCode()
    //                }
    //            };
    //            // heading East, col should decrease
    //            yield return new object[]
    //            {
    //                new Location('a', 3), Heading.E,
    //                new HashSet<int>
    //                {
    //                    new Location('a', 3).GetHashCode(),
    //                    new Location('a', 2).GetHashCode(),
    //                    new Location('a', 1).GetHashCode()
    //                }
    //            };
    //        }
    //    }

    //    public static IEnumerable<object[]> BadBoardLocationsData
    //    {
    //        get
    //        {
    //            // Heading South, row should decrease off the board
    //            yield return new object[]
    //            {
    //                new Location('a', 1), Heading.S,
    //                new HashSet<int>
    //                {
    //                    new Location('a', 1).GetHashCode(),
    //                    new Location('@', 1).GetHashCode(),
    //                    new Location('?', 1).GetHashCode()
    //                }
    //            };
    //            // heading East, col should decrease off the board
    //            yield return new object[]
    //            {
    //                new Location('a', 1), Heading.E,
    //                new HashSet<int>
    //                {
    //                    new Location('a', 1).GetHashCode(),
    //                    new Location('a', 0).GetHashCode(),
    //                    new Location('a', uint.MaxValue).GetHashCode()
    //                }
    //            };
    //        }
    //    }

    //    [Theory]
    //    [MemberData(nameof(BadBoardLocationsData))]
    //    public void ShipShouldHaveProperLocationsUsingOffBoardInstructions(Location bowAt, Heading heading,
    //        HashSet<int> correctLocations)
    //    {
    //        var sut = new ShipDetails
    //        {
    //            ClassName = "Destroyer",
    //            ClassSize = 3,
    //            BowAt = bowAt,
    //            Heading = heading
    //        };

    //        Assert.NotNull(sut.Locations);
    //        var intersection = sut.LocationSet.Intersect(correctLocations);
    //        var locationsInIntersection = intersection.Count();
    //        Assert.Equal(3, locationsInIntersection);
    //    }

    //    [Theory]
    //    [MemberData(nameof(SuccessfulLocationsData))]
    //    public void ShipShouldHaveProperLocations(Location bowAt, Heading heading, HashSet<int> correctLocations)
    //    {
    //        var sut = new ShipDetails
    //        {
    //            ClassName = "Destroyer",
    //            ClassSize = 3,
    //            BowAt = bowAt,
    //            Heading = heading
    //        };

    //        Assert.NotNull(sut.Locations);
    //        var intersection = sut.LocationSet.Intersect(correctLocations);
    //        var locationsInIntersection = intersection.Count();
    //        Assert.Equal(3, locationsInIntersection);
    //    }

    //    [Fact]
    //    public void ShipShouldHaveLocationsWhenBowAtAndHeadingAreSet()
    //    {
    //        var sut = new ShipDetails
    //        {
    //            ClassName = "Destroyer",
    //            ClassSize = 3,
    //            BowAt = new Location('a', 1),
    //            Heading = Heading.N
    //        };

    //        Assert.NotNull(sut.Locations);
    //        Assert.Contains(sut.BowAt.GetHashCode(), sut.LocationSet);
    //        Assert.Contains(new Location('b', 1).GetHashCode(), sut.LocationSet);
    //        Assert.Contains(new Location('c', 1).GetHashCode(), sut.LocationSet);
    //    }

    //    [Fact]
    //    public void ShipShouldNotHaveLocationsWithoutBowAtPropertySet()
    //    {
    //        var sut = new ShipDetails
    //        {
    //            ClassName = "Destroyer",
    //            ClassSize = 3,
    //            BowAt = new Location('a', 1)
    //        };
    //        Assert.Equal(0, sut.Locations.Sum(l => 1));
    //    }

    //    [Fact]
    //    public void ShipShouldNotHaveLocationsWithoutHeadingPropertySet()
    //    {
    //        var sut = new ShipDetails
    //        {
    //            ClassName = "Destroyer",
    //            ClassSize = 3,
    //            Heading = Heading.None
    //        };
    //        Assert.Equal(0, sut.Locations.Sum(l => 1));
    //    }
    //}
}