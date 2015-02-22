using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using battleships.Interfaces;
using NLog;

namespace battleships
{
    public class AiTester
    {
        private readonly Logger resultsLogger;
        private readonly Settings settings;
        private readonly IGameVisualizer gameVisualizer;
        private readonly IMapGenerator mapGenerator;
        private readonly ProcessMonitor processMonitor;
        private readonly IAiFactory aiFactory;
        private readonly IGameFactory gameFactory;
        private readonly TextWriter textWriter;
        private readonly TextReader textReader;

        public AiTester(Settings settings, IGameVisualizer gameVisualizer, IMapGenerator mapGenerator, ProcessMonitor processMonitor, 
            Logger resultsLogger, IAiFactory aiFactory, IGameFactory gameFactory, TextWriter textWriter, TextReader textReader)
        {
            this.settings = settings;
            this.aiFactory = aiFactory;
            this.gameVisualizer = gameVisualizer;
            this.mapGenerator = mapGenerator;
            this.processMonitor = processMonitor;
            this.resultsLogger = resultsLogger;
            this.gameFactory = gameFactory;
            this.textReader = textReader;
            this.textWriter = textWriter;
        }

        public void TestSingleFile(string exe)
        {
            var badShots = 0;
            var crashes = 0;
            var gamesPlayed = 0;
            var shots = new List<int>();
            var ai = aiFactory.CreateAi(exe, processMonitor);

            for (var gameIndex = 0; gameIndex < settings.GamesCount; gameIndex++)
            {
                var map = mapGenerator.GenerateMap();
                var game = gameFactory.CreateGame(map, ai);
                RunGameToEnd(game);
                gamesPlayed++;
                badShots += game.BadShots;
                if (game.AiCrashed)
                {
                    crashes++;
                    if (crashes > settings.CrashLimit) break;
                    ai = aiFactory.CreateAi(exe, processMonitor);
                }
                else
                    shots.Add(game.TurnsCount);
                if (settings.Verbose)
                {
                    textWriter.WriteLine(
                        "Game #{3,4}: Turns {0,4}, BadShots {1}{2}",
                        game.TurnsCount, game.BadShots, game.AiCrashed ? ", Crashed" : "", gameIndex);
                }
            }
            ai.Dispose();
            WriteTotal(ai, shots, crashes, badShots, gamesPlayed);
        }

        private void RunGameToEnd(IGame game)
        {
            while (!game.IsOver())
            {
                game.MakeStep();
                if (settings.Interactive)
                {
                    gameVisualizer.Visualize(game);
                    if (game.AiCrashed)
                        textWriter.WriteLine(game.LastError.Message);
                    textReader.ReadLine();
                }
            }
        }

        private void WriteTotal(IAi ai, List<int> shots, int crashes, int badShots, int gamesPlayed)
        {
            if (shots.Count == 0) shots.Add(1000 * 1000);
            shots.Sort();
            var median = shots.Count % 2 == 1 ? shots[shots.Count / 2] : (shots[shots.Count / 2] + shots[(shots.Count + 1) / 2]) / 2;
            var mean = shots.Average();
            var sigma = Math.Sqrt(shots.Average(s => (s - mean) * (s - mean)));
            var badFraction = (100.0 * badShots) / shots.Sum();
            var crashPenalty = 100.0 * crashes / settings.CrashLimit;
            var efficiencyScore = 100.0 * (settings.Width * settings.Height - mean) / (settings.Width * settings.Height);
            var score = efficiencyScore - crashPenalty - badFraction;
            var headers = FormatTableRow(new object[] { "AiName", "Mean", "Sigma", "Median", "Crashes", "Bad%", "Games", "Score" });
            var message = FormatTableRow(new object[] { ai.Name, mean, sigma, median, crashes, badFraction, gamesPlayed, score });
            resultsLogger.Info(message);
            textWriter.WriteLine();
            textWriter.WriteLine("Score statistics");
            textWriter.WriteLine("================");
            textWriter.WriteLine(headers);
            textWriter.WriteLine(message);
        }

        private string FormatTableRow(object[] values)
        {
            return FormatValue(values[0], 15)
                + string.Join(" ", values.Skip(1).Select(v => FormatValue(v, 7)));
        }

        private static string FormatValue(object v, int width)
        {
            return v.ToString().Replace("\t", " ").PadRight(width).Substring(0, width);
        }
    }
}