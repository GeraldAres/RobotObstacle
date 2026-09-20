using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RobotObstacle
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && string.Equals(args[1], "--test", StringComparison.OrdinalIgnoreCase))
            {
                var log = new StringBuilder();
                int fail = FuzzyTests.Run(log);
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test-results.txt");
                File.WriteAllText(path, log.ToString());
                Environment.Exit(fail == 0 ? 0 : 1);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
