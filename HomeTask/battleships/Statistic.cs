using System.Collections.Generic;
using System.Linq;
namespace battleships
{
    public class Statistic
    {
        public string AiName { get; private set; }
        public double Mean { get; private set; }
        public double Sigma { get; private set; }
        public double Median { get; private set; }
        public int Crashes { get; private set; }
        public double BadFractionInPercent { get; private set; }
        public int GamesCount { get; private set; }
        public double Score { get; private set; }
        public string Message { get; private set; }

        public Statistic(string aiName, List<int> shots, int crashes, int badShots, int gamesPlayed, Settings settings)
        {
            GamesCount = gamesPlayed;
            AiName = aiName;

            if (shots.Count == 0)
                shots.Add(1000 * 1000);
            shots.Sort();
            
            Median = shots.Median();
            Mean = shots.Average();
            Sigma = shots.Sigma();
            
            BadFractionInPercent = (100.0 * badShots) / shots.Sum();
            var crashPenalty = 100.0 * crashes / settings.CrashLimit;
            var efficiencyScore = 100.0 * (settings.Width * settings.Height - Mean) / (settings.Width * settings.Height);
            Score = efficiencyScore - crashPenalty - BadFractionInPercent;
            Message = FormatTableRow(new object[] { AiName, Mean, Sigma, Median, Crashes, BadFractionInPercent, GamesCount, Score });
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
