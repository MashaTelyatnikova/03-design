using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using battleships.AiUtils;
using battleships.GameUtils;
using battleships.MapUtils;
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
            }
            else
                Console.WriteLine("No AI aiPath-file " + aiPath);
        }

        private static void TestSingleFile(string aiPath, Settings settings)
        {
            var logger = LogManager.GetLogger("results");
            var tester = new AiTester();
            var monitor = new ProcessMonitor(TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount),
                settings.MemoryLimit);

            var visualizer = new GameVisualizer();
            if (settings.Interactive)
                tester.GameStepWasMade += visualizer.VisualizeGameStep;

            if (settings.Verbose)
                tester.GameCompleted += visualizer.VisualizeGameEnd;

            using (var ai = new Ai(aiPath))
            {
                ai.RunningProcess += monitor.Register;

                var maps = new MapGenerator(settings).GenerateMaps();
                var games = GamesGenerator.GenerateGames(maps, ai).Take(settings.GamesCount);

                var gameStatistics = tester.TestAi(ai, games, settings.CrashLimit).ToList();
                var resultStatistic = new ResultStatistic(ai.Name, gameStatistics, settings);

                logger.Info(resultStatistic.Message);
                Console.WriteLine(resultStatistic);
            }
        }
    }
}