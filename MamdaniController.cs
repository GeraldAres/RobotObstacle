using System;

namespace RobotObstacle
{
    // Shared Mamdani pipeline used by the UI, 2D heatmap, 3D surface, and tests.
    internal static class MamdaniController
    {
        public const double IntegrationStep = 0.5;

        public static string GetMovementTextFromCrisp(double crisp)
        {
            return MovementFromCrisp(crisp);
        }

        public static double TriangularMembership(double x, double a, double b, double c)
        {
            if (x < a || x > c)
                return 0.0;

            if (x <= b)
            {
                if (b == a)
                    return 1.0;
                return (x - a) / (b - a);
            }

            if (c == b)
                return 1.0;
            return (c - x) / (c - b);
        }

        public static double MuNear(double distance)
        {
            return TriangularMembership(distance, 0, 0, 50);
        }

        public static double MuMedium(double distance)
        {
            return TriangularMembership(distance, 20, 50, 80);
        }

        public static double MuFar(double distance)
        {
            return TriangularMembership(distance, 50, 100, 100);
        }

        public static double MuLeft(double direction)
        {
            return TriangularMembership(direction, -100, -100, 0);
        }

        public static double MuCenter(double direction)
        {
            return TriangularMembership(direction, -50, 0, 50);
        }

        public static double MuRight(double direction)
        {
            return TriangularMembership(direction, 0, 100, 100);
        }

        public static double MuTurnLeft(double y)
        {
            return TriangularMembership(y, 0, 0, 33);
        }

        public static double MuForward(double y)
        {
            return TriangularMembership(y, 0, 33, 66);
        }

        public static double MuTurnRight(double y)
        {
            return TriangularMembership(y, 33, 66, 100);
        }

        public static double MuStop(double y)
        {
            return TriangularMembership(y, 66, 100, 100);
        }

        public static void Fuzzify(
            double distance, double direction,
            out double near, out double medium, out double far,
            out double left, out double center, out double right)
        {
            near = MuNear(distance);
            medium = MuMedium(distance);
            far = MuFar(distance);
            left = MuLeft(direction);
            center = MuCenter(direction);
            right = MuRight(direction);
        }

        public static void EvaluateRules(
            double near, double medium, double far,
            double left, double center, double right,
            out double r1, out double r2, out double r3,
            out double r4, out double r5, out double r6,
            out double r7, out double r8, out double r9)
        {
            r1 = Math.Min(near, left);
            r2 = Math.Min(near, center);
            r3 = Math.Min(near, right);
            r4 = Math.Min(medium, left);
            r5 = Math.Min(medium, center);
            r6 = Math.Min(medium, right);
            r7 = Math.Min(far, left);
            r8 = Math.Min(far, center);
            r9 = Math.Min(far, right);
        }

        public static double DefuzzifyCentroid(
            double r1, double r2, double r3,
            double r4, double r5, double r6,
            double r7, double r8, double r9)
        {
            double weightedSum = 0.0;
            double membershipSum = 0.0;

            for (double y = 0.0; y <= 100.0; y += IntegrationStep)
            {
                double c1 = Math.Min(r1, MuTurnRight(y));
                double c2 = Math.Min(r2, MuStop(y));
                double c3 = Math.Min(r3, MuTurnLeft(y));
                double c4 = Math.Min(r4, MuTurnRight(y));
                double c5 = Math.Min(r5, MuForward(y));
                double c6 = Math.Min(r6, MuTurnLeft(y));
                double c7 = Math.Min(r7, MuForward(y));
                double c8 = Math.Min(r8, MuForward(y));
                double c9 = Math.Min(r9, MuForward(y));

                double mu = c1;
                mu = Math.Max(mu, c2);
                mu = Math.Max(mu, c3);
                mu = Math.Max(mu, c4);
                mu = Math.Max(mu, c5);
                mu = Math.Max(mu, c6);
                mu = Math.Max(mu, c7);
                mu = Math.Max(mu, c8);
                mu = Math.Max(mu, c9);

                weightedSum += y * mu;
                membershipSum += mu;
            }

            if (membershipSum == 0.0)
                return double.NaN;

            return weightedSum / membershipSum;
        }

        // Full Mamdani inference: fuzzify → rules → clip → aggregate → centroid.
        public static double ComputeCrispOutput(double distance, double direction)
        {
            double near, medium, far, left, center, right;
            Fuzzify(distance, direction, out near, out medium, out far, out left, out center, out right);

            double r1, r2, r3, r4, r5, r6, r7, r8, r9;
            EvaluateRules(near, medium, far, left, center, right,
                out r1, out r2, out r3, out r4, out r5, out r6, out r7, out r8, out r9);

            return DefuzzifyCentroid(r1, r2, r3, r4, r5, r6, r7, r8, r9);
        }

        public static string MovementFromCrisp(double crisp)
        {
            if (double.IsNaN(crisp))
                return "NO ACTIVATION";

            double[] peaks = { 0.0, 33.0, 66.0, 100.0 };
            string[] labels = { "TURN LEFT", "FORWARD", "TURN RIGHT", "STOP" };

            int best = 0;
            double bestDistance = Math.Abs(crisp - peaks[0]);
            for (int i = 1; i < peaks.Length; i++)
            {
                double d = Math.Abs(crisp - peaks[i]);
                if (d < bestDistance)
                {
                    bestDistance = d;
                    best = i;
                }
            }

            return labels[best];
        }

        // Grid of actual Mamdani centroids. i = distance 0..100, j = direction -100..100.
        public static double[,] ComputeSurfaceGrid(int resolution)
        {
            if (resolution < 2)
                resolution = 2;

            double[,] z = new double[resolution, resolution];
            for (int i = 0; i < resolution; i++)
            {
                double distance = (double)i / (resolution - 1) * 100.0;
                for (int j = 0; j < resolution; j++)
                {
                    double direction = (double)j / (resolution - 1) * 200.0 - 100.0;
                    z[i, j] = ComputeCrispOutput(distance, direction);
                }
            }

            return z;
        }
    }
}
