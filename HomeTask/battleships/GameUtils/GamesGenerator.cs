using System.Collections.Generic;
using System.Linq;
using battleships.AiUtils;
using battleships.MapUtils;
using MoreLinq;

namespace battleships.GameUtils
{
    public static class GamesGenerator
    {
        public static IEnumerable<Game> GenerateGames(IEnumerable<Map> maps, Ai ai)
        {
            return maps.Index().Select(pair => new Game(pair.Value, ai, pair.Key));
        }
    }
}
