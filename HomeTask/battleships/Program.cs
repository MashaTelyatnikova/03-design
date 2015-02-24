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
                Console.WriteLine("Usage: {0} <ai.exe>", Process.GetCurrentProcess().ProcessName);
                return;
            }
            var aiPath = args[0];
            var settings = new Settings("settings.txt");
            var tester = new AiTester(settings);

            var logger = LogManager.GetLogger("results");

            if (File.Exists(aiPath))
            {
                var statistic = tester.TestSingleFile(aiPath);
                logger.Info(statistic.Message);
                Console.WriteLine(statistic);
            }
            else
                Console.WriteLine("No AI exe-file " + aiPath);
        }
    }
}