using System;
using System.Collections.Generic;
using System.Linq;

namespace battleships
{
    public static class GamesGenerator
    {
        public static IEnumerable<Game> GenerateGames(Settings settings, Ai ai)
        {
            var generator = new MapGenerator(settings);

            return Enumerable.Range(0, settings.GamesCount)
                    .Select(i => new Game(generator.GenerateMap(), ai, i));
        }
    }
}
