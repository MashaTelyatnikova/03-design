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
            var settings = new Settings()
            {
                Width = 10,
                Height = 10,
                Ships = new[] {1, 1, 1, 1, 2, 2, 2, 3, 3, 4},
                RandomSeed = 0
            };
            var gen = new MapGenerator(settings);
            for (var i = 0; i < 10000; i++)
                gen.GenerateMap();
        }
    }
}
