using Battleship.Domain;
using Battleship.Domain.Entities;
using Xunit;

namespace Battleship.Tests
{
    public class ShipTypeTests
    {
        [Fact]
        public void ShipTypeValueEqualityWorks()
        {
            var t1 = ShipType.Cruiser;
            var t2 = ShipType.Cruiser;
            
            Assert.Equal(t1, t2);
        }

        [Fact]
        public void CharHashTest()
        {
            var t1 = 'a'.GetHashCode();
            var t2 = 'a'.GetHashCode();
            Assert.Equal(t1, t2);
            Assert.Equal(6357089, t1);
        }

        [Fact]
        public void StringHashTest()
        {
            var t1 = "abcdefg".GetDeterministicHashCode();
            var t2 = "abcdefg".GetDeterministicHashCode();
            Assert.Equal(t1, t2);
            Assert.Equal(135607550, t1);
        }
    }
}