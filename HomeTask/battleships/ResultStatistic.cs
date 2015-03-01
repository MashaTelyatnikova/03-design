using System.Collections.Generic;
using System.Linq;
using battleships.GameUtils;

namespace battleships
{
    public class ResultStatistic
    {
        public string Message { get; private set; }

        public ResultStatistic(string aiName, IEnumerable<GameStatistic> gameStatistics, Settings settings)
        {

            var allShots = gameStatistics.Select(statistic => statistic.TurnsCount).OrderBy(shot => shot).ToList();
            var allBadShots = gameStatistics.Sum(statistic => statistic.BadShots);
            var crashes = gameStatistics.Count(statistic => statistic.IsCrashed);

            var crashPenalty = 100.0 * crashes / settings.CrashLimit;
            var mean = allShots.Average();
            var efficiencyScore = 100.0 * (settings.Width * settings.Height - mean) / (settings.Width * settings.Height);

            var sigma = allShots.Sigma();
            var median = allShots.Median();
            var gamesCount = gameStatistics.Count();
            var badFractionInPercent = (100.0 * allBadShots) / allShots.Sum();
            var score = efficiencyScore - crashPenalty - badFractionInPercent;

            Message = FormatTableRow(new object[] { aiName, mean, sigma, median, crashes, badFractionInPercent, gamesCount, score });
        }

        public override string ToString()
        {
            var headers = FormatTableRow(new object[] { "AiName", "Mean", "Sigma", "Median", "Crashes", "Bad%", "Games", "Score" });
            return string.Format("\nScore statistics\n================\n{0}\n{1}", headers, Message);
        }

        private static string FormatTableRow(IReadOnlyList<object> values)
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
