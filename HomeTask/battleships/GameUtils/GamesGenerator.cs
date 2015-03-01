using System.Collections.Generic;
using System.Linq;
using battleships.AiUtils;
using battleships.MapUtils;

namespace battleships.GameUtils
{
    public static class GamesGenerator
    {
        public static IEnumerable<Game> GenerateGames(IEnumerable<Map> maps, Ai ai)
        {
            return maps.Select((map, index) => new Game(map, ai, index));
        }
    }
}
