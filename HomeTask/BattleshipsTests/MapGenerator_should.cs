using System;
using NUnit.Framework;

namespace battleships
{
    [TestFixture]
    public class MapGenerator_should
    {
        [Test]
        public void always_succeed_on_standard_map()
        {
            var gen = new MapGenerator(10, 10, new[] { 1, 1, 1, 1, 2, 2, 2, 3, 3, 4 }, new Random());
            for (var i = 0; i < 10000; i++)
                gen.GenerateMap();
        }
    }
}
