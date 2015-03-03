using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using battleships.AiUtils;
using battleships.GameUtils;
using battleships.MapUtils;
using MoreLinq;
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
            {
                Console.WriteLine("No AI aiPath-file " + aiPath);
            }
        }

        private static void TestSingleFile(string aiPath, Settings settings)
        {
            using (var ai = new Ai(aiPath))
            {
                var logger = LogManager.GetLogger("results");
                var tester = new AiTester();
                var monitor = new ProcessMonitor(TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount),
                    settings.MemoryLimit);
                var visualizer = new GameVisualizer();
                var aiRestarter = new AiRestarter(ai, settings.CrashLimit);
                var mapGenerator = new MapGenerator(settings);

                if (settings.Interactive)
                {
                    tester.CompletedGameStep += visualizer.VisualizeCompletedGameStep;
                }

                if (settings.Verbose)
                {
                    tester.CompletedGame += visualizer.VisualizeCompletedGame;
                }
                
                tester.AiCrashed += aiRestarter.RestartAi;
                ai.LaunchedProcess += monitor.Register;

                
                var gameStatistics = Enumerable.Range(0, settings.GamesCount)
                                                .Select(index => new Game(mapGenerator.GenerateMap(), ai, index))
                                                .Select(game => tester.Test(game))
                                                .TakeUntil(gameStatistic => aiRestarter.CrashLimitExceeded);
                
                var resultStatistic = new ResultStatistic(ai.Name, gameStatistics, settings);

                logger.Info(resultStatistic.Message);
                Console.WriteLine(resultStatistic);
            }
        }
    }
}