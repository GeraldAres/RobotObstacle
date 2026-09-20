using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RobotObstacle
{
    public partial class Form1 : Form
    {
        private bool syncingInputs;
        private int surfaceGeneration;
        private double[,] surfaceZ;
        private int surfaceResolution;
        private Bitmap heatmapBase;
        private bool highResQueued;

        public const int LiveSurfaceResolution = 40;
        public const int HighResSurfaceResolution = 100;

        public Form1()
        {
            InitializeComponent();
            // surface debounce Timer is created in Designer as 'surfaceDebounce'.
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

            InitializeCharts();
            PlotAllMemberships();
            RunMamdani();
            RequestSurfaceCompute(LiveSurfaceResolution, false);
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
            PlotAllMemberships();
            DrawHeatmapWithMarker();
            UpdateSurfaceMarker();
        }

        private void RunMamdani()
        {
            double distance = (double)nudDistance.Value;
            double direction = (double)nudDirection.Value;

            double near, medium, far, left, center, right;
            MamdaniController.Fuzzify(distance, direction,
                out near, out medium, out far, out left, out center, out right);

            double r1, r2, r3, r4, r5, r6, r7, r8, r9;
            MamdaniController.EvaluateRules(near, medium, far, left, center, right,
                out r1, out r2, out r3, out r4, out r5, out r6, out r7, out r8, out r9);

            double crispOutput = MamdaniController.DefuzzifyCentroid(
                r1, r2, r3, r4, r5, r6, r7, r8, r9);

            string movement = MamdaniController.MovementFromCrisp(crispOutput);

            UpdateUi(
                distance, direction,
                near, medium, far,
                left, center, right,
                r1, r2, r3, r4, r5, r6, r7, r8, r9,
                crispOutput, movement);
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

            PlotAllMemberships();
            DrawHeatmapWithMarker();
            UpdateSurfaceMarker();
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
            RunMamdani();
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

        private void InitializeCharts()
        {
            try
            {
                if (chartDistance.Image == null)
                    chartDistance.Image = new Bitmap(Math.Max(1, chartDistance.Width), Math.Max(1, chartDistance.Height));
                if (chartDirection.Image == null)
                    chartDirection.Image = new Bitmap(Math.Max(1, chartDirection.Width), Math.Max(1, chartDirection.Height));
                if (chartOutput.Image == null)
                    chartOutput.Image = new Bitmap(Math.Max(1, chartOutput.Width), Math.Max(1, chartOutput.Height));
            }
            catch
            {
            }

            ChartArea area = chartSurface.ChartAreas["Default"];
            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.Inclination = 25;
            area.Area3DStyle.Rotation = 35;
            area.Area3DStyle.LightStyle = LightStyle.Simplistic;
            area.Area3DStyle.WallWidth = 0;
            area.Area3DStyle.PointDepth = 80;
            area.Area3DStyle.PointGapDepth = 0;
            area.AxisX.Title = "Distance (cm)";
            area.AxisX.Minimum = 0;
            area.AxisX.Maximum = 100;
            area.AxisY.Title = "Crisp Movement Output";
            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = 100;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.Enabled = false;

            tbRotation.Value = 35;
            tbInclination.Value = 25;
            lblSurfaceStatus.Text = "Building 40×40 live Mamdani surface...";
        }

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
                    PointF[] ptsNear = new PointF[101];
                    PointF[] ptsMed = new PointF[101];
                    PointF[] ptsFar = new PointF[101];
                    for (int i = 0; i <= 100; i++)
                    {
                        double x = i;
                        float px = (float)i / 100f * (w - 20) + 10;
                        ptsNear[i] = new PointF(px, (float)((1 - MamdaniController.MuNear(x)) * (h - 20) + 10));
                        ptsMed[i] = new PointF(px, (float)((1 - MamdaniController.MuMedium(x)) * (h - 20) + 10));
                        ptsFar[i] = new PointF(px, (float)((1 - MamdaniController.MuFar(x)) * (h - 20) + 10));
                    }
                    g.DrawLines(Pens.Red, ptsNear);
                    g.DrawLines(Pens.Green, ptsMed);
                    g.DrawLines(Pens.Blue, ptsFar);
                    picAssign(chartDistance, bmp);
                }
            }
            catch
            {
            }
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
                    var ptsL = new System.Collections.Generic.List<PointF>();
                    var ptsC = new System.Collections.Generic.List<PointF>();
                    var ptsR = new System.Collections.Generic.List<PointF>();
                    for (int i = -100; i <= 100; i += 2)
                    {
                        float px = (float)(i + 100) / 200f * (w - 20) + 10;
                        ptsL.Add(new PointF(px, (float)((1 - MamdaniController.MuLeft(i)) * (h - 20) + 10)));
                        ptsC.Add(new PointF(px, (float)((1 - MamdaniController.MuCenter(i)) * (h - 20) + 10)));
                        ptsR.Add(new PointF(px, (float)((1 - MamdaniController.MuRight(i)) * (h - 20) + 10)));
                    }
                    g.DrawLines(Pens.Red, ptsL.ToArray());
                    g.DrawLines(Pens.Green, ptsC.ToArray());
                    g.DrawLines(Pens.Blue, ptsR.ToArray());
                    picAssign(chartDirection, bmp);
                }
            }
            catch
            {
            }
        }

        private void PlotOutputMemberships()
        {
            try
            {
                int w = Math.Max(300, chartOutput.Width);
                int h = Math.Max(150, chartOutput.Height);
                using (var bmp = new Bitmap(w, h))
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    var ptsTL = new System.Collections.Generic.List<PointF>();
                    var ptsF = new System.Collections.Generic.List<PointF>();
                    var ptsTR = new System.Collections.Generic.List<PointF>();
                    var ptsS = new System.Collections.Generic.List<PointF>();
                    for (int i = 0; i <= 100; i++)
                    {
                        float px = (float)i / 100f * (w - 20) + 10;
                        ptsTL.Add(new PointF(px, (float)((1 - MamdaniController.MuTurnLeft(i)) * (h - 20) + 10)));
                        ptsF.Add(new PointF(px, (float)((1 - MamdaniController.MuForward(i)) * (h - 20) + 10)));
                        ptsTR.Add(new PointF(px, (float)((1 - MamdaniController.MuTurnRight(i)) * (h - 20) + 10)));
                        ptsS.Add(new PointF(px, (float)((1 - MamdaniController.MuStop(i)) * (h - 20) + 10)));
                    }
                    g.DrawLines(Pens.Red, ptsTL.ToArray());
                    g.DrawLines(Pens.Green, ptsF.ToArray());
                    g.DrawLines(Pens.Blue, ptsTR.ToArray());
                    g.DrawLines(Pens.Purple, ptsS.ToArray());
                    picAssign(chartOutput, bmp);
                }
            }
            catch
            {
            }
        }

        private void picAssign(PictureBox pic, Bitmap bmp)
        {
            try
            {
                Image old = pic.Image;
                pic.Image = (Bitmap)bmp.Clone();
                if (old != null)
                    old.Dispose();
            }
            catch
            {
            }
        }

        private void RequestSurfaceCompute(int resolution, bool isHighRes)
        {
            int gen = Interlocked.Increment(ref surfaceGeneration);
            if (lblSurfaceStatus != null)
            {
                lblSurfaceStatus.Text = isHighRes
                    ? "Computing 100×100 high-resolution Mamdani surface..."
                    : "Computing 40×40 live Mamdani surface...";
            }

            Task.Factory.StartNew(() =>
            {
                double[,] z = MamdaniController.ComputeSurfaceGrid(resolution);
                if (gen != surfaceGeneration)
                    return;

                if (IsDisposed)
                    return;

                try
                {
                    BeginInvoke(new Action(() => ApplySurfaceGrid(z, resolution, isHighRes, gen)));
                }
                catch (ObjectDisposedException)
                {
                }
            });
        }

        private void ApplySurfaceGrid(double[,] z, int resolution, bool isHighRes, int gen)
        {
            if (IsDisposed || gen != surfaceGeneration)
                return;

            surfaceZ = z;
            surfaceResolution = resolution;

            if (heatmapBase != null)
            {
                heatmapBase.Dispose();
                heatmapBase = null;
            }

            heatmapBase = BuildHeatmapBitmap(z, resolution);
            DrawHeatmapWithMarker();
            RebuildSurfaceChart(z, resolution);
            UpdateSurfaceMarker();

            lblSurfaceStatus.Text = isHighRes
                ? "High-res 100×100 surface (actual Mamdani centroids). Depth axis = Direction."
                : "Live 40×40 surface (actual Mamdani centroids). Depth axis = Direction.";

            if (!isHighRes && !highResQueued)
            {
                highResQueued = true;
                surfaceDebounce.Stop();
                surfaceDebounce.Interval = 400;
                surfaceDebounce.Start();
            }
        }

        private void surfaceDebounce_Tick(object sender, EventArgs e)
        {
            surfaceDebounce.Stop();
            RequestSurfaceCompute(HighResSurfaceResolution, true);
        }

        private void btnHighRes_Click(object sender, EventArgs e)
        {
            surfaceDebounce.Stop();
            RequestSurfaceCompute(HighResSurfaceResolution, true);
        }

        private Bitmap BuildHeatmapBitmap(double[,] z, int resolution)
        {
            int w = 256;
            int h = 256;
            var bmp = new Bitmap(w, h);
            for (int px = 0; px < w; px++)
            {
                for (int py = 0; py < h; py++)
                {
                    double gx = (double)px / (w - 1) * (resolution - 1);
                    double gy = (double)py / (h - 1) * (resolution - 1);
                    double crisp = SampleGrid(z, resolution, gx, gy);
                    Color col = double.IsNaN(crisp) ? Color.Black : HsvToRgb((int)(240 - 240 * (crisp / 100.0)), 0.9, 0.9);
                    bmp.SetPixel(px, h - 1 - py, col);
                }
            }
            return bmp;
        }

        static double SampleGrid(double[,] z, int resolution, double gx, double gy)
        {
            int i0 = (int)Math.Floor(gx);
            int j0 = (int)Math.Floor(gy);
            int i1 = Math.Min(resolution - 1, i0 + 1);
            int j1 = Math.Min(resolution - 1, j0 + 1);
            i0 = Math.Max(0, Math.Min(resolution - 1, i0));
            j0 = Math.Max(0, Math.Min(resolution - 1, j0));
            double tx = gx - i0;
            double ty = gy - j0;
            double v00 = z[i0, j0];
            double v10 = z[i1, j0];
            double v01 = z[i0, j1];
            double v11 = z[i1, j1];
            if (double.IsNaN(v00) || double.IsNaN(v10) || double.IsNaN(v01) || double.IsNaN(v11))
                return v00;
            double a = v00 * (1 - tx) + v10 * tx;
            double b = v01 * (1 - tx) + v11 * tx;
            return a * (1 - ty) + b * ty;
        }

        private void DrawHeatmapWithMarker()
        {
            if (picSurface == null)
                return;

            int w = picSurface.Width > 0 ? picSurface.Width : 256;
            int h = picSurface.Height > 0 ? picSurface.Height : 256;
            var bmp = new Bitmap(w, h);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                if (heatmapBase != null)
                    g.DrawImage(heatmapBase, 0, 0, w, h);

                double distance = (double)nudDistance.Value;
                double direction = (double)nudDirection.Value;
                float mx = (float)(distance / 100.0 * (w - 1));
                float my = (float)((1.0 - (direction + 100.0) / 200.0) * (h - 1));
                using (Pen p = new Pen(Color.White, 2f))
                using (Pen p2 = new Pen(Color.Black, 1f))
                {
                    g.DrawLine(p, mx - 8, my, mx + 8, my);
                    g.DrawLine(p, mx, my - 8, mx, my + 8);
                    g.DrawEllipse(p2, mx - 6, my - 6, 12, 12);
                    g.DrawEllipse(p, mx - 5, my - 5, 10, 10);
                }
            }

            Image old = picSurface.Image;
            picSurface.Image = bmp;
            if (old != null)
                old.Dispose();
        }

        private void RebuildSurfaceChart(double[,] z, int resolution)
        {
            chartSurface.Series.Clear();
            chartSurface.Legends.Clear();

            // One line series per Direction sample. With 3D enabled, series are
            // stacked along the depth axis, which is Direction (−100 at the back).
            for (int j = 0; j < resolution; j++)
            {
                Series row = new Series("dir" + j.ToString(CultureInfo.InvariantCulture));
                row.ChartType = SeriesChartType.Line;
                row.ChartArea = "Default";
                row.IsVisibleInLegend = false;
                row.BorderWidth = 1;
                for (int i = 0; i < resolution; i++)
                {
                    double distance = (double)i / (resolution - 1) * 100.0;
                    double crisp = z[i, j];
                    if (double.IsNaN(crisp))
                        crisp = 0;
                    int idx = row.Points.AddXY(distance, crisp);
                    row.Points[idx].Color = HsvToRgb((int)(240 - 240 * (crisp / 100.0)), 0.9, 0.9);
                }
                chartSurface.Series.Add(row);
            }

            Series marker = new Series("Current input");
            marker.ChartType = SeriesChartType.Point;
            marker.ChartArea = "Default";
            marker.MarkerStyle = MarkerStyle.Diamond;
            marker.MarkerSize = 12;
            marker.MarkerColor = Color.White;
            marker.MarkerBorderColor = Color.Black;
            marker.MarkerBorderWidth = 2;
            chartSurface.Series.Add(marker);
        }

        private void UpdateSurfaceMarker()
        {
            if (chartSurface.Series.Count == 0)
                return;

            Series marker = chartSurface.Series.FindByName("Current input");
            if (marker == null)
                return;

            double distance = (double)nudDistance.Value;
            double direction = (double)nudDirection.Value;
            double crisp = MamdaniController.ComputeCrispOutput(distance, direction);
            if (double.IsNaN(crisp))
                crisp = 0;

            marker.Points.Clear();
            marker.Points.AddXY(distance, crisp);

            // Place the marker series at the Direction depth that matches the input.
            if (surfaceResolution > 1)
            {
                int j = (int)Math.Round((direction + 100.0) / 200.0 * (surfaceResolution - 1));
                if (j < 0) j = 0;
                if (j >= surfaceResolution) j = surfaceResolution - 1;
                int current = chartSurface.Series.IndexOf(marker);
                if (current >= 0 && current != j)
                {
                    chartSurface.Series.RemoveAt(current);
                    if (j > chartSurface.Series.Count)
                        j = chartSurface.Series.Count;
                    chartSurface.Series.Insert(j, marker);
                }
            }
        }

        private void tbRotation_Scroll(object sender, EventArgs e)
        {
            chartSurface.ChartAreas["Default"].Area3DStyle.Rotation = tbRotation.Value;
        }

        private void tbInclination_Scroll(object sender, EventArgs e)
        {
            chartSurface.ChartAreas["Default"].Area3DStyle.Inclination = tbInclination.Value;
        }

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

        private void btnRunTests_Click(object sender, EventArgs e)
        {
            RunTests();
        }

        private void RunTests()
        {
            var sb = new StringBuilder();
            FuzzyTests.Run(sb);
            txtTests.Text = sb.ToString();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Interlocked.Increment(ref surfaceGeneration);
            if (heatmapBase != null)
            {
                heatmapBase.Dispose();
                heatmapBase = null;
            }
            base.OnFormClosing(e);
        }
    }
}
