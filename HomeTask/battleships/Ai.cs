using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using battleships.Enums;
using NLog;

namespace battleships
{
    public class Ai : IDisposable
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private Process process;
        private readonly string exePath;
        public event Action<Process> RunningProcess;

        public Ai(string exePath)
        {
            this.exePath = exePath;
        }

        public void Restart()
        {
            this.process = null;
        }

        public string Name
        {
            get { return Path.GetFileNameWithoutExtension(exePath); }
        }

        public Vector Init(int width, int height, int[] shipSizes)
        {
            if (process == null || process.HasExited) process = RunProcess();
            SendMessage("Init {0} {1} {2}", width, height, string.Join(" ", shipSizes));
            return ReceiveNextShot();
        }

        public Vector GetNextShot(Vector lastShotTarget, ShotEffect lastShot)
        {
            SendMessage("{0} {1} {2}", lastShot, lastShotTarget.X, lastShotTarget.Y);
            return ReceiveNextShot();
        }

        public void SendMessage(string messageFormat, params object[] args)
        {
            var message = string.Format(messageFormat, args);
            process.StandardInput.WriteLine(message);
            Log.Debug("SEND: " + message);
        }

        public void Dispose()
        {
            if (process == null || process.HasExited) return;
            Log.Debug("CLOSE");
            process.StandardInput.Close();
            if (!process.WaitForExit(500))
                Log.Info("Not terminated {0}", process.ProcessName);
            try
            {
                process.Kill();
            }
            catch
            {
                //nothing to do
            }
            process = null;
        }

        private Process RunProcess()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            var aiProcess = Process.Start(startInfo);
            if (RunningProcess != null) 
                RunningProcess(aiProcess);
            return aiProcess;
        }

        private Vector ReceiveNextShot()
        {
            var output = process.StandardOutput.ReadLine();
            Log.Debug("RECEIVE " + output);
            if (output == null)
            {
                var err = process.StandardError.ReadToEnd();
                Console.WriteLine(err);
                Log.Info(err);
                throw new Exception("No ai output");
            }
            try
            {
                var parts = output.Split(' ').Select(int.Parse).ToList();
                return new Vector(parts[0], parts[1]);
            }
            catch (Exception e)
            {
                throw new Exception("Wrong ai output: " + output, e);
            }
        }
    }
}