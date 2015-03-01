using System;
using System.Collections.Generic;
using battleships.GameUtils;

namespace battleships.AiUtils
{
    public class AiTester
    {
        public event Action<Game> GameStepWasMade;
        public event Action<Game> GameCompleted;

        public IEnumerable<GameStatistic> TestAi(Ai ai, IEnumerable<Game> games, int crashLimit)
        {
            foreach (var game in games)
            {
                game.GameStepWasMade += GameStepWasMade;
                game.PlayToEnd();

                if (game.AiCrashed)
                {
                    crashLimit--;
                    if (crashLimit < 0) break;
                    ai.Restart();
                }

                if (GameCompleted != null)
                    GameCompleted(game);
                
                yield return game.GetStatistic();
            }
        }
    }
}