using System;
using System.Collections.Generic;
using System.Linq;
using battleships.GameUtils;

namespace battleships.AiUtils
{
    public class AiTester
    {
        private readonly Settings settings;
        public event Action<Game> CompletedGameStep;
        public event Action<Game> CompletedGame;

        public AiTester(Settings settings)
        {
            this.settings = settings;
        }

        public IEnumerable<GameStatistic> TestAi(Ai ai, IEnumerable<Game> games, int crashLimit)
        {
            var gameStatistics = new List<GameStatistic>();

            foreach (var game in games)
            {
                game.CompletedGameStep += CompletedGameStep;
                game.PlayToEnd();
                gameStatistics.Add(game.GetStatistic());

                if (game.AiCrashed)
                {
                    if (gameStatistics.Count(i => i.IsCrashed) > settings.CrashLimit)
                    {
                        break;
                    }

                    ai.Restart();
                }

                if (CompletedGame != null)
                {
                    CompletedGame(game);
                }

                yield return game.GetStatistic();
            }
        }
    }
}