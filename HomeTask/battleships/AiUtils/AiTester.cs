using System;
using battleships.GameUtils;

namespace battleships.AiUtils
{
    public class AiTester
    {
        public event Action<Game> CompletedGameStep;
        public event Action<Game> CompletedGame;
        public event Action AiCrashed;

        public GameStatistic Test(Game game)
        {
            game.CompletedGameStep += CompletedGameStep;
            game.PlayToEnd();

            if (game.AiCrashed && AiCrashed != null)
            {
                AiCrashed();
            }

            if (CompletedGame != null)
            {
                CompletedGame(game);
            }

            return game.GetStatistic();
        }
    }
}