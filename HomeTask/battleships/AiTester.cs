using System;
using System.Collections.Generic;

namespace battleships
{
    public class AiTester
    {
        private readonly Settings settings;
        public event Action<string> TestingCompleted;

        public AiTester(Settings settings)
        {
            this.settings = settings;
        }

        public void TestSingleFile(string exe)
        {
            var gen = new MapGenerator(settings.Width, settings.Height, settings.Ships, new Random(settings.RandomSeed));
            var vis = new GameVisualizer();
            var monitor = new ProcessMonitor(TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount),
                settings.MemoryLimit);
            var badShots = 0;
            var crashes = 0;
            var gamesPlayed = 0;
            var shots = new List<int>();
            var ai = new Ai(exe);
            ai.RunningProcess += monitor.Register;

            for (var gameIndex = 0; gameIndex < settings.GamesCount; gameIndex++)
            {
                var map = gen.GenerateMap();
                var game = new Game(map, ai);
                RunGameToEnd(game, vis);
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
                        game.TurnsCount, game.BadShots, game.AiCrashed ? ", Crashed" : "", gameIndex);
                }
            }
            ai.Dispose();
            var statistic = new Statistic(ai.Name, shots, crashes, badShots, gamesPlayed, settings);
            
            if (TestingCompleted != null) 
                TestingCompleted(statistic.Message);
            
            Console.WriteLine(statistic);
        }

        private void RunGameToEnd(Game game, GameVisualizer vis)
        {
            while (!game.IsOver())
            {
                game.MakeStep();
                if (!settings.Interactive) continue;
                
                vis.Visualize(game);
                if (game.AiCrashed)
                    Console.WriteLine(game.LastError.Message);
                Console.ReadKey();
            }
        }
    }
}