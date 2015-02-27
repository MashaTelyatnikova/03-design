using System;
using System.Collections.Generic;
using System.Linq;

namespace battleships
{
    public class AiTester
    {
        private readonly Settings settings;
        public event Action<Game> GameStepWasMade;
        public event Action<Game> GameCompleted;

        public AiTester(Settings settings)
        {
            this.settings = settings;
        }

        public ResultStatistic TestAi(Ai ai, IEnumerable<Game> games)
        {
            var gameStatistics = new List<GameStatistic>();

            foreach (var game in games)
            {
                game.GameStepWasMade += GameStepWasMade;
                game.PlayToEnd();
                gameStatistics.Add(game.GetStatistic());

                if (game.AiCrashed)
                {
                    if (gameStatistics.Count(i => i.IsCrashed) > settings.CrashLimit) break;
                    ai.Restart();
                }

                if (GameCompleted != null)
                    GameCompleted(game);
            }

            return new ResultStatistic(ai.Name, gameStatistics, settings);
        }
    }
}