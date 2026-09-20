using System;
using System.Globalization;
using System.Text;

namespace RobotObstacle
{
    internal static class FuzzyTests
    {
        public static int Run(StringBuilder log)
        {
            int fail = 0;
            log.AppendLine("Canonical 9-rule tests (actual Mamdani centroid):");

            Check(10, -100, "TURN RIGHT", ref fail, log);
            Check(10, 0, "STOP", ref fail, log);
            Check(10, 100, "TURN LEFT", ref fail, log);
            Check(50, -100, "TURN RIGHT", ref fail, log);
            Check(50, 0, "FORWARD", ref fail, log);
            Check(50, 100, "TURN LEFT", ref fail, log);
            Check(90, -100, "FORWARD", ref fail, log);
            Check(90, 0, "FORWARD", ref fail, log);
            Check(90, 100, "FORWARD", ref fail, log);

            log.AppendLine();
            log.AppendLine("Boundary tests (must produce a finite centroid):");
            double[] bDistances = { 0, 20, 50, 80, 100 };
            double[] bDirections = { -100, -50, 0, 50, 100 };
            foreach (double d in bDistances)
            {
                foreach (double dir in bDirections)
                {
                    double crisp = MamdaniController.ComputeCrispOutput(d, dir);
                    string move = MamdaniController.MovementFromCrisp(crisp);
                    bool ok = !double.IsNaN(crisp);
                    if (!ok)
                        fail++;
                    log.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "{0} D={1,3}, Dir={2,4} => Crisp={3}, Move={4}",
                        ok ? "PASS" : "FAIL",
                        d, dir,
                        double.IsNaN(crisp) ? "NaN" : crisp.ToString("0.00", CultureInfo.InvariantCulture),
                        move));
                }
            }

            log.AppendLine();
            log.AppendLine(fail == 0
                ? "ALL TESTS PASSED"
                : ("FAILED: " + fail.ToString(CultureInfo.InvariantCulture)));
            return fail;
        }

        static void Check(double distance, double direction, string expectedMove, ref int fail, StringBuilder log)
        {
            double crisp = MamdaniController.ComputeCrispOutput(distance, direction);
            string move = MamdaniController.MovementFromCrisp(crisp);
            bool ok = move == expectedMove && !double.IsNaN(crisp);
            if (!ok)
                fail++;
            log.AppendLine(string.Format(CultureInfo.InvariantCulture,
                "{0} D={1}, Dir={2} => Crisp={3}, Move={4} (expected {5})",
                ok ? "PASS" : "FAIL",
                distance, direction,
                double.IsNaN(crisp) ? "NaN" : crisp.ToString("0.00", CultureInfo.InvariantCulture),
                move, expectedMove));
        }
    }
}
