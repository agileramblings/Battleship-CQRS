using Battleship.Domain;
using Xunit;

namespace Battleship.Tests
{
    public class LocationTests
    {
        [Fact]
        public void LocationShouldTakeCharIntConstructor()
        {
            var sut = new Location('a', 1);
            Assert.NotNull(sut);
        }

        [Fact]
        public void LocationShouldAlwaysConvertRowToUpperCase()
        {
            var sut = new Location('a', 1);
            Assert.Equal('A', sut.Row);
        }

        [Fact]
        public void LocationShouldEmitStringOverride()
        {
            var sut = new Location('a', 1);
            Assert.Equal("A:1", sut.ToString());
        }

        [Fact]
        public void TwoLocationsShouldBeEqualIfValueIsEqual()
        {
            var sut1 = new Location('a', 1);
            var sut2 = new Location('a', 1);
            Assert.Equal(sut1, sut2);
        }

        [Fact]
        public void TwoLocationShouldBeEqualIfSameObject()
        {
            var sut = new Location('a', 1);
            Assert.True(sut.Equals(sut));
        }
    }
}
