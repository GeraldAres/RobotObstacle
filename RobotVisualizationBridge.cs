using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace RobotObstacle
{
    internal sealed class RobotVisualizationBridge : IDisposable
    {
        private readonly object syncRoot = new object();
        private readonly string statePath;
        private readonly string pythonScriptPath;
        private readonly string pythonExecutablePath;
        private Process pythonProcess;
        private bool launched;

        public RobotVisualizationBridge()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            statePath = Path.Combine(baseDir, "robot_visualization_state.json");
            pythonScriptPath = Path.Combine(baseDir, "RobotVisualization.py");
            pythonExecutablePath = ResolvePythonExecutable();
        }

        public static string BuildPayload(double distance, double direction, double crispOutput, string movement)
        {
            string safeMovement = string.IsNullOrWhiteSpace(movement) ? "FORWARD" : movement.Trim();
            return string.Format(CultureInfo.InvariantCulture,
                "{{\"distance\":{0:0.00},\"direction\":{1:0.00},\"crisp_output\":{2:0.00},\"movement\":\"{3}\"}}",
                distance, direction, crispOutput, safeMovement);
        }

        public void SendState(double distance, double direction, double crispOutput, string movement)
        {
            if (double.IsNaN(crispOutput))
                return;

            lock (syncRoot)
            {
                EnsurePythonRunning();
                File.WriteAllText(statePath, BuildPayload(distance, direction, crispOutput, movement));
            }
        }

        private void EnsurePythonRunning()
        {
            if (launched && pythonProcess != null && !pythonProcess.HasExited)
                return;

            if (string.IsNullOrWhiteSpace(pythonExecutablePath) || !File.Exists(pythonExecutablePath))
                return;

            if (!File.Exists(pythonScriptPath))
                return;

            var psi = new ProcessStartInfo
            {
                FileName = pythonExecutablePath,
                Arguments = string.Format(CultureInfo.InvariantCulture, "\"{0}\" --state-file \"{1}\"", pythonScriptPath, statePath),
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                UseShellExecute = false,
                CreateNoWindow = false
            };

            pythonProcess = Process.Start(psi);
            launched = true;
        }

        private static string ResolvePythonExecutable()
        {
            string[] candidates = new[]
            {
                Environment.GetEnvironmentVariable("PYTHON"),
                Environment.GetEnvironmentVariable("PYTHON_EXE"),
                "C:/Program Files/Python313/python.exe",
                "C:/Python313/python.exe",
                "python",
                "python3"
            };

            foreach (string candidate in candidates)
            {
                if (string.IsNullOrWhiteSpace(candidate))
                    continue;

                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = candidate,
                        Arguments = "-c \"import sys; print(sys.executable)\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    };

                    using (Process probe = Process.Start(psi))
                    {
                        if (probe == null)
                            continue;

                        string output = probe.StandardOutput.ReadToEnd();
                        probe.WaitForExit(5000);
                        if (probe.ExitCode == 0 && !string.IsNullOrWhiteSpace(output))
                        {
                            return output.Trim();
                        }
                    }
                }
                catch
                {
                }
            }

            return string.Empty;
        }

        public void Dispose()
        {
            lock (syncRoot)
            {
                if (pythonProcess != null && !pythonProcess.HasExited)
                {
                    try
                    {
                        pythonProcess.Kill();
                        pythonProcess.WaitForExit(5000);
                    }
                    catch
                    {
                    }
                }

                pythonProcess = null;
            }
        }
    }
}
