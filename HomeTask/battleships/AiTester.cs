using System;
using System.Collections.Generic;

namespace battleships
{
    public class AiTester
    {
        private readonly Settings settings;
        public event Action<Game> GameStepWasMade;

        public AiTester(Settings settings)
        {
            this.settings = settings;
        }

        public Statistic TestAi(Ai ai, IEnumerable<Game> games)
        {
            var badShots = 0;
            var crashes = 0;
            var gamesPlayed = 0;
            var shots = new List<int>();
            
            foreach (var game in games)
            {
                RunGameToEnd(game);
                
                gamesPlayed++;
                badShots += game.BadShots;
                
                if (game.AiCrashed)
                {
                    crashes++;
                    if (crashes > settings.CrashLimit) break;
                    ai.Restart();
                }
                else
                    shots.Add(game.TurnsCount);
                if (settings.Verbose)
                {
                    Console.WriteLine(
                        "Game #{3,4}: Turns {0,4}, BadShots {1}{2}",
                        game.TurnsCount, game.BadShots, game.AiCrashed ? ", Crashed" : "", gamesPlayed - 1);
                }
            }
            
            return new Statistic(ai.Name, shots, crashes, badShots, gamesPlayed, settings);
        }

        private void RunGameToEnd(Game game)
        {
            while (!game.IsOver())
            {
                game.MakeStep();
                if (GameStepWasMade != null)
                    GameStepWasMade(game);
            }
        }
    }
}