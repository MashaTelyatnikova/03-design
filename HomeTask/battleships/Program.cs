using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using NLog;

namespace battleships
{
    public static class Program
    {

        private static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: {0} <ai.aiPath>", Process.GetCurrentProcess().ProcessName);
                return;
            }
            var aiPath = args[0];

            if (File.Exists(aiPath))
            {
                TestSingleFile(aiPath, new Settings("settings.txt"));

                Console.ReadKey();

                TestSingleFile(aiPath, new Settings("settings2.txt"));
            }
            else
                Console.WriteLine("No AI aiPath-file " + aiPath);
        }

        private static void TestSingleFile(string aiPath, Settings settings)
        {
            var logger = LogManager.GetLogger("results");
            var tester = new AiTester(settings);
            var monitor = new ProcessMonitor(TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount),
                settings.MemoryLimit);

            var visualizer = new GameVisualizer();
            if (settings.Interactive)
                tester.GameStepWasMade += visualizer.Visualize;

            if (settings.Verbose)
                tester.GameCompleted += DisplayGameDetails;

            using (var ai = new Ai(aiPath))
            {
                ai.RunningProcess += monitor.Register;

                var statistic = tester.TestAi(ai, GamesGenerator.GenerateGames(settings, ai));

                logger.Info(statistic.Message);
                Console.WriteLine(statistic);
            }
        }

        private static void DisplayGameDetails(Game game)
        {
            Console.WriteLine(
                        "Game #{3,4}: Turns {0,4}, BadShots {1}{2}",
                        game.TurnsCount, game.BadShots, game.AiCrashed ? ", Crashed" : "", game.Index);
        }
    }
}