using System;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace RobotObstacle
{
    public partial class Form1 : Form
    {
        // Prevents TrackBar and NumericUpDown from updating each other in a loop.
        private bool syncingInputs;

        // Small step used to approximate the area under the aggregated fuzzy set.
        private const double IntegrationStep = 0.5;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblRanges.Text =
                "DISTANCE  universe: 0 to 100 cm" + Environment.NewLine +
                "  Near    triangle (0, 0, 50)" + Environment.NewLine +
                "  Medium  triangle (20, 50, 80)" + Environment.NewLine +
                "  Far     triangle (50, 100, 100)" + Environment.NewLine +
                Environment.NewLine +
                "DIRECTION  universe: -100 to 100" + Environment.NewLine +
                "  Left    triangle (-100, -100, 0)" + Environment.NewLine +
                "  Center  triangle (-50, 0, 50)" + Environment.NewLine +
                "  Right   triangle (0, 100, 100)" + Environment.NewLine +
                Environment.NewLine +
                "MOVEMENT  universe: 0 to 100  (numeric only" + Environment.NewLine +
                "so the centroid formula can be used)" + Environment.NewLine +
                "  Turn Left   peak at 0    triangle (0, 0, 33)" + Environment.NewLine +
                "  Forward     peak at 33   triangle (0, 33, 66)" + Environment.NewLine +
                "  Turn Right  peak at 66   triangle (33, 66, 100)" + Environment.NewLine +
                "  Stop        peak at 100  triangle (66, 100, 100)";

            RunMamdani();

            // Initialize charts and visualizations
            InitializeCharts();
            PlotAllMemberships();
            RenderControlSurface();
        }

        // ---------------------------------------------------------------------
        //  Triangular membership function
        //  μ = 0 outside [a, c]
        //  μ rises from a to peak b, then falls from b to c
        //  If a == b the set is a left shoulder (full membership at the left edge)
        //  If b == c the set is a right shoulder (full membership at the right edge)
        // ---------------------------------------------------------------------
        static double TriangularMembership(double x, double a, double b, double c)
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

        // ---------------------------------------------------------------------
        //  Input membership functions  (documented in the UI as well)
        // ---------------------------------------------------------------------
        static double MuNear(double distance)
        {
            return TriangularMembership(distance, 0, 0, 50);
        }

        static double MuMedium(double distance)
        {
            return TriangularMembership(distance, 20, 50, 80);
        }

        static double MuFar(double distance)
        {
            return TriangularMembership(distance, 50, 100, 100);
        }

        static double MuLeft(double direction)
        {
            return TriangularMembership(direction, -100, -100, 0);
        }

        static double MuCenter(double direction)
        {
            return TriangularMembership(direction, -50, 0, 50);
        }

        static double MuRight(double direction)
        {
            return TriangularMembership(direction, 0, 100, 100);
        }

        // Output sets live on a number line only so we can compute a centroid.
        // The numbers themselves are not "degrees"; they encode movement:
        //   0 = Turn Left,  33 = Forward,  66 = Turn Right,  100 = Stop
        static double MuTurnLeft(double y)
        {
            return TriangularMembership(y, 0, 0, 33);
        }

        static double MuForward(double y)
        {
            return TriangularMembership(y, 0, 33, 66);
        }

        static double MuTurnRight(double y)
        {
            return TriangularMembership(y, 33, 66, 100);
        }

        static double MuStop(double y)
        {
            return TriangularMembership(y, 66, 100, 100);
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            RunMamdani();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            syncingInputs = true;
            nudDistance.Value = 50;
            tbDistance.Value = 50;
            nudDirection.Value = 0;
            tbDirection.Value = 0;
            syncingInputs = false;

            txtCrisp.Text = "Press CALCULATE MOVEMENT to run Mamdani inference.";
            txtFuzz.Clear();
            txtRules.Clear();
            txtResult.Clear();
        }

        // ---------------------------------------------------------------------
        //  Complete Mamdani process:
        //  Crisp → Fuzzify → Evaluate rules → Implication → Aggregate → Centroid
        // ---------------------------------------------------------------------
        private void RunMamdani()
        {
            double distance = (double)nudDistance.Value;
            double direction = (double)nudDirection.Value;

            // 1) FUZZIFICATION
            // Convert each crisp input into membership degrees of every fuzzy set.
            double near = MuNear(distance);
            double medium = MuMedium(distance);
            double far = MuFar(distance);

            double left = MuLeft(direction);
            double center = MuCenter(direction);
            double right = MuRight(direction);

            // 2) RULE EVALUATION
            // 9 rules cover every Distance × Direction pair.
            // AND is implemented with Math.Min (standard Mamdani t-norm).
            // OR would use Math.Max (not needed in this rule base).
            //
            // Distance \ Direction | Left        | Center      | Right
            // Near                 | Turn Right  | Stop        | Turn Left
            // Medium               | Turn Right  | Forward     | Turn Left
            // Far                  | Forward     | Forward     | Forward

            double r1 = Math.Min(near, left);       // Near + Left   → Turn Right
            double r2 = Math.Min(near, center);     // Near + Center → Stop
            double r3 = Math.Min(near, right);      // Near + Right  → Turn Left
            double r4 = Math.Min(medium, left);     // Medium + Left → Turn Right
            double r5 = Math.Min(medium, center);   // Medium + Center → Forward
            double r6 = Math.Min(medium, right);    // Medium + Right → Turn Left
            double r7 = Math.Min(far, left);        // Far + Left    → Forward
            double r8 = Math.Min(far, center);      // Far + Center  → Forward
            double r9 = Math.Min(far, right);       // Far + Right   → Forward

            // 3) IMPLICATION + 4) AGGREGATION + 5) CENTROID DEFUZZIFICATION
            double crispOutput = DefuzzifyCentroid(
                r1, r2, r3, r4, r5, r6, r7, r8, r9);

            string movement = MovementFromCrisp(crispOutput);

            UpdateUi(
                distance, direction,
                near, medium, far,
                left, center, right,
                r1, r2, r3, r4, r5, r6, r7, r8, r9,
                crispOutput, movement);
        }

        // Implication: clip each output set with Math.Min(ruleStrength, μ_output(y))
        // Aggregation: combine clipped sets with Math.Max
        // Defuzzification: Center of Gravity
        //     crisp = Σ (y * μ(y))  /  Σ μ(y)
        static double DefuzzifyCentroid(
            double r1, double r2, double r3,
            double r4, double r5, double r6,
            double r7, double r8, double r9)
        {
            double weightedSum = 0.0;
            double membershipSum = 0.0;

            for (double y = 0.0; y <= 100.0; y += IntegrationStep)
            {
                // Clipped membership of each rule's output set at this y.
                double c1 = Math.Min(r1, MuTurnRight(y));
                double c2 = Math.Min(r2, MuStop(y));
                double c3 = Math.Min(r3, MuTurnLeft(y));
                double c4 = Math.Min(r4, MuTurnRight(y));
                double c5 = Math.Min(r5, MuForward(y));
                double c6 = Math.Min(r6, MuTurnLeft(y));
                double c7 = Math.Min(r7, MuForward(y));
                double c8 = Math.Min(r8, MuForward(y));
                double c9 = Math.Min(r9, MuForward(y));

                // Aggregate with MAX (Mamdani union of clipped output sets).
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
                return double.NaN; // indicate no activation

            return weightedSum / membershipSum;
        }

        // Map the numeric centroid back to a movement label using the
        // four output peaks: 0, 33, 66, 100.
        static string MovementFromCrisp(double crisp)
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

        private void UpdateUi(
            double distance, double direction,
            double near, double medium, double far,
            double left, double center, double right,
            double r1, double r2, double r3,
            double r4, double r5, double r6,
            double r7, double r8, double r9,
            double crispOutput, string movement)
        {
            txtCrisp.Text =
                "Distance:  " + Format(distance) + " cm" + Environment.NewLine +
                "Direction: " + Format(direction);

            txtFuzz.Text =
                "Distance:" + Environment.NewLine +
                "  Near:   " + Format(near) + Environment.NewLine +
                "  Medium: " + Format(medium) + Environment.NewLine +
                "  Far:    " + Format(far) + Environment.NewLine +
                Environment.NewLine +
                "Direction:" + Environment.NewLine +
                "  Left:   " + Format(left) + Environment.NewLine +
                "  Center: " + Format(center) + Environment.NewLine +
                "  Right:  " + Format(right);

            StringBuilder rules = new StringBuilder();
            rules.AppendLine("R1 Near + Left   → Turn Right : " + Format(r1));
            rules.AppendLine("R2 Near + Center → Stop       : " + Format(r2));
            rules.AppendLine("R3 Near + Right  → Turn Left  : " + Format(r3));
            rules.AppendLine("R4 Medium + Left → Turn Right : " + Format(r4));
            rules.AppendLine("R5 Medium + Ctr  → Forward    : " + Format(r5));
            rules.AppendLine("R6 Medium + Right→ Turn Left  : " + Format(r6));
            rules.AppendLine("R7 Far + Left    → Forward    : " + Format(r7));
            rules.AppendLine("R8 Far + Center  → Forward    : " + Format(r8));
            rules.AppendLine("R9 Far + Right   → Forward    : " + Format(r9));
            txtRules.Text = rules.ToString();

            txtResult.Text =
                "Crisp Output: " + (double.IsNaN(crispOutput) ? "NO ACTIVATION" : Format(crispOutput)) + Environment.NewLine +
                "Movement:     " + movement;

            // Update plots and control surface visual
            PlotAllMemberships();
            RenderControlSurface();
        }

        static string Format(double value)
        {
            return value.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private void tbDistance_Scroll(object sender, EventArgs e)
        {
            if (syncingInputs)
                return;
            syncingInputs = true;
            nudDistance.Value = tbDistance.Value;
            syncingInputs = false;
            RunMamdani();
        }

        private void nudDistance_ValueChanged(object sender, EventArgs e)
        {
            if (syncingInputs)
                return;
            syncingInputs = true;
            tbDistance.Value = (int)Math.Round((double)nudDistance.Value);
            syncingInputs = false;
        }

        private void tbDirection_Scroll(object sender, EventArgs e)
        {
            if (syncingInputs)
                return;
            syncingInputs = true;
            nudDirection.Value = tbDirection.Value;
            syncingInputs = false;
            RunMamdani();
        }

        private void nudDirection_ValueChanged(object sender, EventArgs e)
        {
            if (syncingInputs)
                return;
            syncingInputs = true;
            tbDirection.Value = (int)Math.Round((double)nudDirection.Value);
            syncingInputs = false;
            RunMamdani();
        }

        // -------------------------
        // Charts and Visualizations
        // -------------------------
        private void InitializeCharts()
        {
            // Initialize picture-box based plots (no-op placeholder)
            // Ensure picture boxes have a blank image so drawing can occur
            try
            {
                if (chartDistance.Image == null) chartDistance.Image = new Bitmap(chartDistance.Width > 0 ? chartDistance.Width : 300, chartDistance.Height > 0 ? chartDistance.Height : 150);
                if (chartDirection.Image == null) chartDirection.Image = new Bitmap(chartDirection.Width > 0 ? chartDirection.Width : 300, chartDirection.Height > 0 ? chartDirection.Height : 150);
                if (chartOutput.Image == null) chartOutput.Image = new Bitmap(chartOutput.Width > 0 ? chartOutput.Width : 300, chartOutput.Height > 0 ? chartOutput.Height : 150);
            }
            catch { }
        }

        // Chart support removed; use PictureBox bitmap drawing instead.

        private void PlotAllMemberships()
        {
            PlotDistanceMemberships();
            PlotDirectionMemberships();
            PlotOutputMemberships();
        }

        private void PlotDistanceMemberships()
        {
            try
            {
                int w = Math.Max(300, chartDistance.Width);
                int h = Math.Max(150, chartDistance.Height);
                using (var bmp = new Bitmap(w, h))
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    Pen pNear = Pens.Red;
                    Pen pMed = Pens.Green;
                    Pen pFar = Pens.Blue;
                    PointF[] ptsNear = new PointF[101];
                    PointF[] ptsMed = new PointF[101];
                    PointF[] ptsFar = new PointF[101];
                    for (int i = 0; i <= 100; i++)
                    {
                        double x = i;
                        float px = (float)i / 100f * (w - 20) + 10;
                        ptsNear[i] = new PointF(px, (float)((1 - MuNear(x)) * (h - 20) + 10));
                        ptsMed[i] = new PointF(px, (float)((1 - MuMedium(x)) * (h - 20) + 10));
                        ptsFar[i] = new PointF(px, (float)((1 - MuFar(x)) * (h - 20) + 10));
                    }
                    g.DrawLines(pNear, ptsNear);
                    g.DrawLines(pMed, ptsMed);
                    g.DrawLines(pFar, ptsFar);
                    picAssign(chartDistance, bmp);
                }
            }
            catch { }
        }

        private void PlotDirectionMemberships()
        {
            try
            {
                int w = Math.Max(300, chartDirection.Width);
                int h = Math.Max(150, chartDirection.Height);
                using (var bmp = new Bitmap(w, h))
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    Pen pLeft = Pens.Red;
                    Pen pCenter = Pens.Green;
                    Pen pRight = Pens.Blue;
                    var ptsL = new System.Collections.Generic.List<PointF>();
                    var ptsC = new System.Collections.Generic.List<PointF>();
                    var ptsR = new System.Collections.Generic.List<PointF>();
                    for (int i = -100; i <= 100; i += 2)
                    {
                        float px = (float)(i + 100) / 200f * (w - 20) + 10;
                        ptsL.Add(new PointF(px, (float)((1 - MuLeft(i)) * (h - 20) + 10)));
                        ptsC.Add(new PointF(px, (float)((1 - MuCenter(i)) * (h - 20) + 10)));
                        ptsR.Add(new PointF(px, (float)((1 - MuRight(i)) * (h - 20) + 10)));
                    }
                    g.DrawLines(pLeft, ptsL.ToArray());
                    g.DrawLines(pCenter, ptsC.ToArray());
                    g.DrawLines(pRight, ptsR.ToArray());
                    picAssign(chartDirection, bmp);
                }
            }
            catch { }
        }

        private void PlotOutputMemberships()
        {
            try
            {
                int w = Math.Max(300, chartOutput.Width);
                int h = Math.Max(300, chartOutput.Height);
                using (var bmp = new Bitmap(w, h))
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    Pen pTL = Pens.Red; Pen pF = Pens.Green; Pen pTR = Pens.Blue; Pen pS = Pens.Purple;
                    var ptsTL = new System.Collections.Generic.List<PointF>();
                    var ptsF = new System.Collections.Generic.List<PointF>();
                    var ptsTR = new System.Collections.Generic.List<PointF>();
                    var ptsS = new System.Collections.Generic.List<PointF>();
                    for (int i = 0; i <= 100; i++)
                    {
                        float px = (float)i / 100f * (w - 20) + 10;
                        ptsTL.Add(new PointF(px, (float)((1 - MuTurnLeft(i)) * (h - 20) + 10)));
                        ptsF.Add(new PointF(px, (float)((1 - MuForward(i)) * (h - 20) + 10)));
                        ptsTR.Add(new PointF(px, (float)((1 - MuTurnRight(i)) * (h - 20) + 10)));
                        ptsS.Add(new PointF(px, (float)((1 - MuStop(i)) * (h - 20) + 10)));
                    }
                    g.DrawLines(pTL, ptsTL.ToArray());
                    g.DrawLines(pF, ptsF.ToArray());
                    g.DrawLines(pTR, ptsTR.ToArray());
                    g.DrawLines(pS, ptsS.ToArray());
                    picAssign(chartOutput, bmp);
                }
            }
            catch { }
        }

        private void picAssign(System.Windows.Forms.PictureBox pic, Bitmap bmp)
        {
            try
            {
                pic.Image?.Dispose();
                pic.Image = (Bitmap)bmp.Clone();
            }
            catch { }
        }

        private void RenderControlSurface()
        {
            try
            {
                int w = picSurface.Width > 0 ? picSurface.Width : 256;
                int h = picSurface.Height > 0 ? picSurface.Height : 256;
                using (var bmp = new Bitmap(w, h))
                {
                    for (int px = 0; px < w; px++)
                    {
                        for (int py = 0; py < h; py++)
                        {
                            double distance = (double)px / (w - 1) * 100.0;
                            double direction = (double)py / (h - 1) * 200.0 - 100.0;
                            double near = MuNear(distance);
                            double medium = MuMedium(distance);
                            double far = MuFar(distance);
                            double left = MuLeft(direction);
                            double center = MuCenter(direction);
                            double right = MuRight(direction);
                            double r1 = Math.Min(near, left);
                            double r2 = Math.Min(near, center);
                            double r3 = Math.Min(near, right);
                            double r4 = Math.Min(medium, left);
                            double r5 = Math.Min(medium, center);
                            double r6 = Math.Min(medium, right);
                            double r7 = Math.Min(far, left);
                            double r8 = Math.Min(far, center);
                            double r9 = Math.Min(far, right);
                            double crisp = DefuzzifyCentroid(r1, r2, r3, r4, r5, r6, r7, r8, r9);
                            Color col;
                            if (double.IsNaN(crisp))
                                col = Color.Black;
                            else
                            {
                                // Map 0..100 to hue 240->0 (blue->red)
                                double t = crisp / 100.0;
                                int hue = (int)(240 - 240 * t);
                                col = HsvToRgb(hue, 0.9, 0.9);
                            }
                            bmp.SetPixel(px, h - 1 - py, col);
                        }
                    }
                    // Assign to PictureBox (clone to avoid disposed bitmap issues)
                    picSurface.Image?.Dispose();
                    picSurface.Image = (Bitmap)bmp.Clone();
                }
            }
            catch { }
        }

        // Simple HSV->RGB converter (h in degrees 0..360)
        private static Color HsvToRgb(int h, double s, double v)
        {
            double hh = (h % 360) / 60.0;
            int i = (int)Math.Floor(hh);
            double f = hh - i;
            double p = v * (1 - s);
            double q = v * (1 - s * f);
            double t = v * (1 - s * (1 - f));
            double r = 0, g = 0, b = 0;
            switch (i)
            {
                case 0: r = v; g = t; b = p; break;
                case 1: r = q; g = v; b = p; break;
                case 2: r = p; g = v; b = t; break;
                case 3: r = p; g = q; b = v; break;
                case 4: r = t; g = p; b = v; break;
                default: r = v; g = p; b = q; break;
            }
            return Color.FromArgb(255, ClampByte(r * 255), ClampByte(g * 255), ClampByte(b * 255));
        }

        private static int ClampByte(double v)
        {
            if (v < 0) return 0;
            if (v > 255) return 255;
            return (int)Math.Round(v);
        }

        // -------------------------
        // Test harness
        // -------------------------
        private void btnRunTests_Click(object sender, EventArgs e)
        {
            RunTests();
        }

        private void RunTests()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Canonical 9-rule tests:");
            var distances = new double[] { 10.0, 50.0, 90.0 }; // near, medium, far (representative)
            var directions = new double[] { -100.0, 0.0, 100.0 }; // left, center, right
            for (int i = 0; i < distances.Length; i++)
            {
                for (int j = 0; j < directions.Length; j++)
                {
                    double d = distances[i]; double dir = directions[j];
                    double near = MuNear(d); double med = MuMedium(d); double far = MuFar(d);
                    double left = MuLeft(dir); double center = MuCenter(dir); double right = MuRight(dir);
                    double r1 = Math.Min(near, left);
                    double r2 = Math.Min(near, center);
                    double r3 = Math.Min(near, right);
                    double r4 = Math.Min(med, left);
                    double r5 = Math.Min(med, center);
                    double r6 = Math.Min(med, right);
                    double r7 = Math.Min(far, left);
                    double r8 = Math.Min(far, center);
                    double r9 = Math.Min(far, right);
                    double crisp = DefuzzifyCentroid(r1, r2, r3, r4, r5, r6, r7, r8, r9);
                    string move = MovementFromCrisp(crisp);
                    sb.AppendLine($"D={d}, Dir={dir} => Crisp={(double.IsNaN(crisp)?"NaN":Format(crisp))}, Move={move}");
                }
            }
            sb.AppendLine();
            sb.AppendLine("Boundary tests:");
            var bDistances = new double[] { 0, 20, 50, 80, 100 };
            var bDirections = new double[] { -100, -50, 0, 50, 100 };
            foreach (var d in bDistances)
            {
                foreach (var dir in bDirections)
                {
                    double near = MuNear(d); double med = MuMedium(d); double far = MuFar(d);
                    double left = MuLeft(dir); double center = MuCenter(dir); double right = MuRight(dir);
                    double r1 = Math.Min(near, left);
                    double r2 = Math.Min(near, center);
                    double r3 = Math.Min(near, right);
                    double r4 = Math.Min(med, left);
                    double r5 = Math.Min(med, center);
                    double r6 = Math.Min(med, right);
                    double r7 = Math.Min(far, left);
                    double r8 = Math.Min(far, center);
                    double r9 = Math.Min(far, right);
                    double crisp = DefuzzifyCentroid(r1, r2, r3, r4, r5, r6, r7, r8, r9);
                    string move = MovementFromCrisp(crisp);
                    sb.AppendLine($"D={d,3}, Dir={dir,4} => Crisp={(double.IsNaN(crisp)?"NaN":Format(crisp))}, Move={move}");
                }
            }
            txtTests.Text = sb.ToString();
        }
    }
}
