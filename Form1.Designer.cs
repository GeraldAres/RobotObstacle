namespace RobotObstacle
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.surfaceDebounce = new System.Windows.Forms.Timer(this.components);
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpInputs = new System.Windows.Forms.GroupBox();
            this.lblDistance = new System.Windows.Forms.Label();
            this.nudDistance = new System.Windows.Forms.NumericUpDown();
            this.lblDistanceUnit = new System.Windows.Forms.Label();
            this.tbDistance = new System.Windows.Forms.TrackBar();
            this.lblDirection = new System.Windows.Forms.Label();
            this.nudDirection = new System.Windows.Forms.NumericUpDown();
            this.lblDirectionUnit = new System.Windows.Forms.Label();
            this.tbDirection = new System.Windows.Forms.TrackBar();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.grpRanges = new System.Windows.Forms.GroupBox();
            this.lblRanges = new System.Windows.Forms.Label();
            this.grpCrisp = new System.Windows.Forms.GroupBox();
            this.txtCrisp = new System.Windows.Forms.TextBox();
            this.grpFuzz = new System.Windows.Forms.GroupBox();
            this.txtFuzz = new System.Windows.Forms.TextBox();
            this.grpRules = new System.Windows.Forms.GroupBox();
            this.txtRules = new System.Windows.Forms.TextBox();
            this.grpResult = new System.Windows.Forms.GroupBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.chartDistance = new System.Windows.Forms.PictureBox();
            this.chartDirection = new System.Windows.Forms.PictureBox();
            this.chartOutput = new System.Windows.Forms.PictureBox();
            this.grpHeatmap = new System.Windows.Forms.GroupBox();
            this.picSurface = new System.Windows.Forms.PictureBox();
            this.lblHeatmapHint = new System.Windows.Forms.Label();
            this.btnRunTests = new System.Windows.Forms.Button();
            this.grpTests = new System.Windows.Forms.GroupBox();
            this.txtTests = new System.Windows.Forms.TextBox();
            this.grp3D = new System.Windows.Forms.GroupBox();
            this.chartSurface = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblRotation = new System.Windows.Forms.Label();
            this.tbRotation = new System.Windows.Forms.TrackBar();
            this.lblInclination = new System.Windows.Forms.Label();
            this.tbInclination = new System.Windows.Forms.TrackBar();
            this.btnHighRes = new System.Windows.Forms.Button();
            this.lblSurfaceStatus = new System.Windows.Forms.Label();
            this.grpInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDirection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDirection)).BeginInit();
            this.grpRanges.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDirection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartOutput)).BeginInit();
            this.grpHeatmap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSurface)).BeginInit();
            this.grpCrisp.SuspendLayout();
            this.grpFuzz.SuspendLayout();
            this.grpRules.SuspendLayout();
            this.grpResult.SuspendLayout();
            this.grpTests.SuspendLayout();
            this.grp3D.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSurface)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRotation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbInclination)).BeginInit();
            this.SuspendLayout();
            //
            // surfaceDebounce
            //
            this.surfaceDebounce.Interval = 400;
            this.surfaceDebounce.Tick += new System.EventHandler(this.surfaceDebounce_Tick);
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(16, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(720, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Mamdani Fuzzy Logic Controller  –  Robot Obstacle Avoidance";
            //
            // grpInputs
            //
            this.grpInputs.Controls.Add(this.lblDistance);
            this.grpInputs.Controls.Add(this.nudDistance);
            this.grpInputs.Controls.Add(this.lblDistanceUnit);
            this.grpInputs.Controls.Add(this.tbDistance);
            this.grpInputs.Controls.Add(this.lblDirection);
            this.grpInputs.Controls.Add(this.nudDirection);
            this.grpInputs.Controls.Add(this.lblDirectionUnit);
            this.grpInputs.Controls.Add(this.tbDirection);
            this.grpInputs.Controls.Add(this.btnCalculate);
            this.grpInputs.Controls.Add(this.btnReset);
            this.grpInputs.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.grpInputs.Location = new System.Drawing.Point(16, 50);
            this.grpInputs.Name = "grpInputs";
            this.grpInputs.Size = new System.Drawing.Size(400, 280);
            this.grpInputs.TabIndex = 1;
            this.grpInputs.TabStop = false;
            this.grpInputs.Text = "Crisp Inputs";
            //
            // lblDistance
            //
            this.lblDistance.AutoSize = true;
            this.lblDistance.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblDistance.Location = new System.Drawing.Point(16, 32);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new System.Drawing.Size(160, 17);
            this.lblDistance.TabIndex = 0;
            this.lblDistance.Text = "Distance from obstacle";
            //
            // nudDistance
            //
            this.nudDistance.DecimalPlaces = 1;
            this.nudDistance.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.nudDistance.Location = new System.Drawing.Point(220, 30);
            this.nudDistance.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudDistance.Name = "nudDistance";
            this.nudDistance.Size = new System.Drawing.Size(80, 25);
            this.nudDistance.TabIndex = 1;
            this.nudDistance.Value = new decimal(new int[] { 25, 0, 0, 0 });
            this.nudDistance.ValueChanged += new System.EventHandler(this.nudDistance_ValueChanged);
            //
            // lblDistanceUnit
            //
            this.lblDistanceUnit.AutoSize = true;
            this.lblDistanceUnit.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblDistanceUnit.Location = new System.Drawing.Point(306, 32);
            this.lblDistanceUnit.Name = "lblDistanceUnit";
            this.lblDistanceUnit.Size = new System.Drawing.Size(26, 17);
            this.lblDistanceUnit.TabIndex = 2;
            this.lblDistanceUnit.Text = "cm";
            //
            // tbDistance
            //
            this.tbDistance.LargeChange = 10;
            this.tbDistance.Location = new System.Drawing.Point(16, 60);
            this.tbDistance.Maximum = 100;
            this.tbDistance.Name = "tbDistance";
            this.tbDistance.Size = new System.Drawing.Size(364, 45);
            this.tbDistance.TabIndex = 3;
            this.tbDistance.TickFrequency = 10;
            this.tbDistance.Value = 25;
            this.tbDistance.Scroll += new System.EventHandler(this.tbDistance_Scroll);
            //
            // lblDirection
            //
            this.lblDirection.AutoSize = true;
            this.lblDirection.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblDirection.Location = new System.Drawing.Point(16, 118);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(140, 17);
            this.lblDirection.TabIndex = 4;
            this.lblDirection.Text = "Direction of obstacle";
            //
            // nudDirection
            //
            this.nudDirection.DecimalPlaces = 1;
            this.nudDirection.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.nudDirection.Location = new System.Drawing.Point(220, 116);
            this.nudDirection.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudDirection.Minimum = new decimal(new int[] { 100, 0, 0, -2147483648 });
            this.nudDirection.Name = "nudDirection";
            this.nudDirection.Size = new System.Drawing.Size(80, 25);
            this.nudDirection.TabIndex = 5;
            this.nudDirection.Value = new decimal(new int[] { 40, 0, 0, -2147483648 });
            this.nudDirection.ValueChanged += new System.EventHandler(this.nudDirection_ValueChanged);
            //
            // lblDirectionUnit
            //
            this.lblDirectionUnit.AutoSize = true;
            this.lblDirectionUnit.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblDirectionUnit.Location = new System.Drawing.Point(306, 118);
            this.lblDirectionUnit.Name = "lblDirectionUnit";
            this.lblDirectionUnit.Size = new System.Drawing.Size(70, 17);
            this.lblDirectionUnit.TabIndex = 6;
            this.lblDirectionUnit.Text = "(-100..100)";
            //
            // tbDirection
            //
            this.tbDirection.LargeChange = 10;
            this.tbDirection.Location = new System.Drawing.Point(16, 146);
            this.tbDirection.Maximum = 100;
            this.tbDirection.Minimum = -100;
            this.tbDirection.Name = "tbDirection";
            this.tbDirection.Size = new System.Drawing.Size(364, 45);
            this.tbDirection.TabIndex = 7;
            this.tbDirection.TickFrequency = 20;
            this.tbDirection.Value = -40;
            this.tbDirection.Scroll += new System.EventHandler(this.tbDirection_Scroll);
            //
            // btnCalculate
            //
            this.btnCalculate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCalculate.Location = new System.Drawing.Point(16, 210);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(230, 48);
            this.btnCalculate.TabIndex = 8;
            this.btnCalculate.Text = "CALCULATE MOVEMENT";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            //
            // btnReset
            //
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnReset.Location = new System.Drawing.Point(256, 210);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(124, 48);
            this.btnReset.TabIndex = 9;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            //
            // grpRanges
            //
            this.grpRanges.Controls.Add(this.lblRanges);
            this.grpRanges.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.grpRanges.Location = new System.Drawing.Point(16, 342);
            this.grpRanges.Name = "grpRanges";
            this.grpRanges.Size = new System.Drawing.Size(400, 250);
            this.grpRanges.TabIndex = 2;
            this.grpRanges.TabStop = false;
            this.grpRanges.Text = "Membership Function Ranges";
            //
            // lblRanges
            //
            this.lblRanges.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.lblRanges.Location = new System.Drawing.Point(12, 24);
            this.lblRanges.Name = "lblRanges";
            this.lblRanges.Size = new System.Drawing.Size(376, 214);
            this.lblRanges.TabIndex = 0;
            this.lblRanges.Text = "";
            //
            // grpCrisp
            //
            this.grpCrisp.Controls.Add(this.txtCrisp);
            this.grpCrisp.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.grpCrisp.Location = new System.Drawing.Point(432, 50);
            this.grpCrisp.Name = "grpCrisp";
            this.grpCrisp.Size = new System.Drawing.Size(540, 96);
            this.grpCrisp.TabIndex = 3;
            this.grpCrisp.TabStop = false;
            this.grpCrisp.Text = "1. Crisp Inputs";
            //
            // txtCrisp
            //
            this.txtCrisp.BackColor = System.Drawing.Color.White;
            this.txtCrisp.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtCrisp.Location = new System.Drawing.Point(12, 24);
            this.txtCrisp.Multiline = true;
            this.txtCrisp.Name = "txtCrisp";
            this.txtCrisp.ReadOnly = true;
            this.txtCrisp.Size = new System.Drawing.Size(512, 58);
            this.txtCrisp.TabIndex = 0;
            this.txtCrisp.Text = "Press CALCULATE MOVEMENT to run Mamdani inference.";
            //
            // grpFuzz
            //
            this.grpFuzz.Controls.Add(this.txtFuzz);
            this.grpFuzz.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.grpFuzz.Location = new System.Drawing.Point(432, 154);
            this.grpFuzz.Name = "grpFuzz";
            this.grpFuzz.Size = new System.Drawing.Size(540, 168);
            this.grpFuzz.TabIndex = 4;
            this.grpFuzz.TabStop = false;
            this.grpFuzz.Text = "2. Fuzzification";
            //
            // txtFuzz
            //
            this.txtFuzz.BackColor = System.Drawing.Color.White;
            this.txtFuzz.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtFuzz.Location = new System.Drawing.Point(12, 24);
            this.txtFuzz.Multiline = true;
            this.txtFuzz.Name = "txtFuzz";
            this.txtFuzz.ReadOnly = true;
            this.txtFuzz.Size = new System.Drawing.Size(512, 128);
            this.txtFuzz.TabIndex = 0;
            //
            // grpRules
            //
            this.grpRules.Controls.Add(this.txtRules);
            this.grpRules.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.grpRules.Location = new System.Drawing.Point(432, 330);
            this.grpRules.Name = "grpRules";
            this.grpRules.Size = new System.Drawing.Size(540, 200);
            this.grpRules.TabIndex = 5;
            this.grpRules.TabStop = false;
            this.grpRules.Text = "3. Rule Firing Strengths  (AND = Min)";
            //
            // txtRules
            //
            this.txtRules.BackColor = System.Drawing.Color.White;
            this.txtRules.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.txtRules.Location = new System.Drawing.Point(12, 24);
            this.txtRules.Multiline = true;
            this.txtRules.Name = "txtRules";
            this.txtRules.ReadOnly = true;
            this.txtRules.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRules.Size = new System.Drawing.Size(512, 160);
            this.txtRules.TabIndex = 0;
            //
            // grpResult
            //
            this.grpResult.Controls.Add(this.txtResult);
            this.grpResult.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.grpResult.Location = new System.Drawing.Point(432, 538);
            this.grpResult.Name = "grpResult";
            this.grpResult.Size = new System.Drawing.Size(540, 100);
            this.grpResult.TabIndex = 6;
            this.grpResult.TabStop = false;
            this.grpResult.Text = "4. Aggregation + Centroid Defuzzification";
            //
            // txtResult
            //
            this.txtResult.BackColor = System.Drawing.Color.White;
            this.txtResult.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            this.txtResult.Location = new System.Drawing.Point(12, 24);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(512, 62);
            this.txtResult.TabIndex = 0;
            //
            // chartDistance
            //
            this.chartDistance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chartDistance.Location = new System.Drawing.Point(16, 650);
            this.chartDistance.Name = "chartDistance";
            this.chartDistance.Size = new System.Drawing.Size(350, 150);
            this.chartDistance.TabIndex = 20;
            this.chartDistance.TabStop = false;
            //
            // chartDirection
            //
            this.chartDirection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chartDirection.Location = new System.Drawing.Point(372, 650);
            this.chartDirection.Name = "chartDirection";
            this.chartDirection.Size = new System.Drawing.Size(350, 150);
            this.chartDirection.TabIndex = 21;
            this.chartDirection.TabStop = false;
            //
            // chartOutput
            //
            this.chartOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chartOutput.Location = new System.Drawing.Point(990, 50);
            this.chartOutput.Name = "chartOutput";
            this.chartOutput.Size = new System.Drawing.Size(330, 180);
            this.chartOutput.TabIndex = 22;
            this.chartOutput.TabStop = false;
            //
            // grpHeatmap
            //
            this.grpHeatmap.Controls.Add(this.picSurface);
            this.grpHeatmap.Controls.Add(this.lblHeatmapHint);
            this.grpHeatmap.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.grpHeatmap.Location = new System.Drawing.Point(990, 240);
            this.grpHeatmap.Name = "grpHeatmap";
            this.grpHeatmap.Size = new System.Drawing.Size(330, 360);
            this.grpHeatmap.TabIndex = 23;
            this.grpHeatmap.TabStop = false;
            this.grpHeatmap.Text = "2D Control Surface (heatmap)";
            //
            // picSurface
            //
            this.picSurface.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSurface.Location = new System.Drawing.Point(12, 24);
            this.picSurface.Name = "picSurface";
            this.picSurface.Size = new System.Drawing.Size(306, 292);
            this.picSurface.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSurface.TabIndex = 0;
            this.picSurface.TabStop = false;
            //
            // lblHeatmapHint
            //
            this.lblHeatmapHint.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblHeatmapHint.Location = new System.Drawing.Point(12, 322);
            this.lblHeatmapHint.Name = "lblHeatmapHint";
            this.lblHeatmapHint.Size = new System.Drawing.Size(306, 32);
            this.lblHeatmapHint.TabIndex = 1;
            this.lblHeatmapHint.Text = "X = Distance (0→100). Y = Direction (−100 bottom, +100 top). Crosshair = current input. Color = Mamdani centroid.";
            //
            // btnRunTests
            //
            this.btnRunTests.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnRunTests.Location = new System.Drawing.Point(990, 12);
            this.btnRunTests.Name = "btnRunTests";
            this.btnRunTests.Size = new System.Drawing.Size(128, 28);
            this.btnRunTests.TabIndex = 24;
            this.btnRunTests.Text = "Run Tests";
            this.btnRunTests.UseVisualStyleBackColor = true;
            this.btnRunTests.Click += new System.EventHandler(this.btnRunTests_Click);
            //
            // grpTests
            //
            this.grpTests.Controls.Add(this.txtTests);
            this.grpTests.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.grpTests.Location = new System.Drawing.Point(728, 650);
            this.grpTests.Name = "grpTests";
            this.grpTests.Size = new System.Drawing.Size(592, 150);
            this.grpTests.TabIndex = 25;
            this.grpTests.TabStop = false;
            this.grpTests.Text = "Tests";
            //
            // txtTests
            //
            this.txtTests.BackColor = System.Drawing.Color.White;
            this.txtTests.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.txtTests.Location = new System.Drawing.Point(12, 24);
            this.txtTests.Multiline = true;
            this.txtTests.Name = "txtTests";
            this.txtTests.ReadOnly = true;
            this.txtTests.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTests.Size = new System.Drawing.Size(568, 112);
            this.txtTests.TabIndex = 0;
            //
            // grp3D
            //
            this.grp3D.Controls.Add(this.chartSurface);
            this.grp3D.Controls.Add(this.lblRotation);
            this.grp3D.Controls.Add(this.tbRotation);
            this.grp3D.Controls.Add(this.lblInclination);
            this.grp3D.Controls.Add(this.tbInclination);
            this.grp3D.Controls.Add(this.btnHighRes);
            this.grp3D.Controls.Add(this.lblSurfaceStatus);
            this.grp3D.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.grp3D.Location = new System.Drawing.Point(16, 812);
            this.grp3D.Name = "grp3D";
            this.grp3D.Size = new System.Drawing.Size(1304, 360);
            this.grp3D.TabIndex = 26;
            this.grp3D.TabStop = false;
            this.grp3D.Text = "3D Fuzzy Control Surface   X = Distance   depth = Direction   height = Crisp Movement Output";
            //
            // chartSurface
            //
            chartArea1.Name = "Default";
            this.chartSurface.ChartAreas.Add(chartArea1);
            this.chartSurface.Location = new System.Drawing.Point(12, 24);
            this.chartSurface.Name = "chartSurface";
            this.chartSurface.Size = new System.Drawing.Size(900, 280);
            this.chartSurface.TabIndex = 0;
            this.chartSurface.Text = "chartSurface";
            //
            // lblRotation
            //
            this.lblRotation.AutoSize = true;
            this.lblRotation.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRotation.Location = new System.Drawing.Point(930, 28);
            this.lblRotation.Name = "lblRotation";
            this.lblRotation.Size = new System.Drawing.Size(56, 15);
            this.lblRotation.TabIndex = 1;
            this.lblRotation.Text = "Rotation";
            //
            // tbRotation
            //
            this.tbRotation.Location = new System.Drawing.Point(930, 48);
            this.tbRotation.Maximum = 180;
            this.tbRotation.Minimum = -180;
            this.tbRotation.Name = "tbRotation";
            this.tbRotation.Size = new System.Drawing.Size(350, 45);
            this.tbRotation.TabIndex = 2;
            this.tbRotation.TickFrequency = 30;
            this.tbRotation.Value = 35;
            this.tbRotation.Scroll += new System.EventHandler(this.tbRotation_Scroll);
            //
            // lblInclination
            //
            this.lblInclination.AutoSize = true;
            this.lblInclination.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblInclination.Location = new System.Drawing.Point(930, 96);
            this.lblInclination.Name = "lblInclination";
            this.lblInclination.Size = new System.Drawing.Size(68, 15);
            this.lblInclination.TabIndex = 3;
            this.lblInclination.Text = "Inclination";
            //
            // tbInclination
            //
            this.tbInclination.Location = new System.Drawing.Point(930, 116);
            this.tbInclination.Maximum = 90;
            this.tbInclination.Minimum = -90;
            this.tbInclination.Name = "tbInclination";
            this.tbInclination.Size = new System.Drawing.Size(350, 45);
            this.tbInclination.TabIndex = 4;
            this.tbInclination.TickFrequency = 15;
            this.tbInclination.Value = 25;
            this.tbInclination.Scroll += new System.EventHandler(this.tbInclination_Scroll);
            //
            // btnHighRes
            //
            this.btnHighRes.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnHighRes.Location = new System.Drawing.Point(930, 180);
            this.btnHighRes.Name = "btnHighRes";
            this.btnHighRes.Size = new System.Drawing.Size(350, 36);
            this.btnHighRes.TabIndex = 5;
            this.btnHighRes.Text = "Render High-Res 100×100";
            this.btnHighRes.UseVisualStyleBackColor = true;
            this.btnHighRes.Click += new System.EventHandler(this.btnHighRes_Click);
            //
            // lblSurfaceStatus
            //
            this.lblSurfaceStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSurfaceStatus.Location = new System.Drawing.Point(930, 228);
            this.lblSurfaceStatus.Name = "lblSurfaceStatus";
            this.lblSurfaceStatus.Size = new System.Drawing.Size(350, 110);
            this.lblSurfaceStatus.TabIndex = 6;
            this.lblSurfaceStatus.Text = "Surface uses the real Mamdani centroid at every grid point. White diamond = current input.";
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1348, 900);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.grpInputs);
            this.Controls.Add(this.grpRanges);
            this.Controls.Add(this.grpCrisp);
            this.Controls.Add(this.grpFuzz);
            this.Controls.Add(this.grpRules);
            this.Controls.Add(this.grpResult);
            this.Controls.Add(this.chartDistance);
            this.Controls.Add(this.chartDirection);
            this.Controls.Add(this.chartOutput);
            this.Controls.Add(this.grpHeatmap);
            this.Controls.Add(this.btnRunTests);
            this.Controls.Add(this.grpTests);
            this.Controls.Add(this.grp3D);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mamdani FLC – Robot Obstacle Avoidance";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpInputs.ResumeLayout(false);
            this.grpInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDirection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDirection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDirection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartOutput)).EndInit();
            this.grpHeatmap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picSurface)).EndInit();
            this.grpRanges.ResumeLayout(false);
            this.grpCrisp.ResumeLayout(false);
            this.grpCrisp.PerformLayout();
            this.grpFuzz.ResumeLayout(false);
            this.grpFuzz.PerformLayout();
            this.grpRules.ResumeLayout(false);
            this.grpRules.PerformLayout();
            this.grpResult.ResumeLayout(false);
            this.grpResult.PerformLayout();
            this.grpTests.ResumeLayout(false);
            this.grpTests.PerformLayout();
            this.grp3D.ResumeLayout(false);
            this.grp3D.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSurface)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRotation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbInclination)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer surfaceDebounce;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpInputs;
        private System.Windows.Forms.Label lblDistance;
        private System.Windows.Forms.NumericUpDown nudDistance;
        private System.Windows.Forms.Label lblDistanceUnit;
        private System.Windows.Forms.TrackBar tbDistance;
        private System.Windows.Forms.Label lblDirection;
        private System.Windows.Forms.NumericUpDown nudDirection;
        private System.Windows.Forms.Label lblDirectionUnit;
        private System.Windows.Forms.TrackBar tbDirection;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.GroupBox grpRanges;
        private System.Windows.Forms.Label lblRanges;
        private System.Windows.Forms.GroupBox grpCrisp;
        private System.Windows.Forms.TextBox txtCrisp;
        private System.Windows.Forms.GroupBox grpFuzz;
        private System.Windows.Forms.TextBox txtFuzz;
        private System.Windows.Forms.GroupBox grpRules;
        private System.Windows.Forms.TextBox txtRules;
        private System.Windows.Forms.GroupBox grpResult;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.PictureBox chartDistance;
        private System.Windows.Forms.PictureBox chartDirection;
        private System.Windows.Forms.PictureBox chartOutput;
        private System.Windows.Forms.GroupBox grpHeatmap;
        private System.Windows.Forms.PictureBox picSurface;
        private System.Windows.Forms.Label lblHeatmapHint;
        private System.Windows.Forms.Button btnRunTests;
        private System.Windows.Forms.GroupBox grpTests;
        private System.Windows.Forms.TextBox txtTests;
        private System.Windows.Forms.GroupBox grp3D;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSurface;
        private System.Windows.Forms.Label lblRotation;
        private System.Windows.Forms.TrackBar tbRotation;
        private System.Windows.Forms.Label lblInclination;
        private System.Windows.Forms.TrackBar tbInclination;
        private System.Windows.Forms.Button btnHighRes;
        private System.Windows.Forms.Label lblSurfaceStatus;
    }
}
