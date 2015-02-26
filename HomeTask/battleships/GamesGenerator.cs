using System;
using System.Collections.Generic;

namespace battleships
{
    public static class GamesGenerator
    {
        public static IEnumerable<Game> GenerateGames(Settings settings, Ai ai)
        {
            var generator = new MapGenerator(settings.Width, settings.Height, settings.Ships,
                new Random(settings.RandomSeed));

            for (var i = 0; i < settings.GamesCount; ++i)
            {
                var map = generator.GenerateMap();
                yield return new Game(map, ai, settings.Interactive);
            }
        } 
    }
}
