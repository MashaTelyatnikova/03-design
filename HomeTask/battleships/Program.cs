using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using Ninject;
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

		    var ninjectKernel = new StandardKernel(new AiTesterModule());
			var aiPath = args[0];

            var tester = new AiTester(ninjectKernel.Get<Settings>(), ninjectKernel.Get<IGameVisualizer>(), ninjectKernel.Get<IMapGenerator>(),
                ninjectKernel.Get<ProcessMonitor>(), ninjectKernel.Get<Logger>());

            if (File.Exists(aiPath))
                tester.TestSingleFile(aiPath);
            else
                Console.WriteLine("No AI exe-file " + aiPath);
		}
	}
}