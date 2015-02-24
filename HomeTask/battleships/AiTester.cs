using System;
using System.Collections.Generic;

namespace battleships
{
    public class AiTester
    {
        private readonly Settings settings;

        public AiTester(Settings settings)
        {
            this.settings = settings;
        }

        public Statistic TestSingleFile(string exe)
        {
            var monitor = new ProcessMonitor(TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount),
                settings.MemoryLimit);
            
            var badShots = 0;
            var crashes = 0;
            var gamesPlayed = 0;
            var shots = new List<int>();
            
            using (var ai = new Ai(exe))
            {
                ai.RunningProcess += monitor.Register;
                
                foreach (var game in GamesGenerator.GenerateGames(settings, ai))
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
            }
            
            return new Statistic(exe, shots, crashes, badShots, gamesPlayed, settings);
        }

        private void RunGameToEnd(Game game)
        {
            while (!game.IsOver())
            {
                game.MakeStep();
                if (!settings.Interactive) continue;
                
                GameVisualizer.Visualize(game);
                Console.ReadKey();
            }
        }
    }
}