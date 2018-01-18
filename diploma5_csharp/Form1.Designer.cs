namespace diploma5_csharp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.buttonShadowDetectionLab = new System.Windows.Forms.Button();
            this.textBoxShadowDetectionLabThreshold = new System.Windows.Forms.TextBox();
            this.buttonShadowDetectionMS = new System.Windows.Forms.Button();
            this.textBoxShadowDetectionLMSThreshold = new System.Windows.Forms.TextBox();
            this.buttonShadowRemovalAditiveMethod = new System.Windows.Forms.Button();
            this.buttonShadowRemovalConstantMethod = new System.Windows.Forms.Button();
            this.buttonShadowRemovalLabMethod = new System.Windows.Forms.Button();
            this.buttonShadowRemovalCombinedMethod = new System.Windows.Forms.Button();
            this.buttonShadowRemovalBasicLightModelMethod = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonImpaintShadowEdges = new System.Windows.Forms.Button();
            this.buttonSmoothShadowEdgesUsingGaussianFilter = new System.Windows.Forms.Button();
            this.buttonSmoothShadowEdgesUsingMedianFilter = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonRemoveFogUsingDarkChannelMethod = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDetectionResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportMetricsToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetMethodsStatisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBoxShowOptionalWindows = new System.Windows.Forms.CheckBox();
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonShadowRemovalLabMethod2 = new System.Windows.Forms.Button();
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod = new System.Windows.Forms.Button();
            this.buttonTestEmguCVCudaMeanShift = new System.Windows.Forms.Button();
            this.buttonMSTest = new System.Windows.Forms.Button();
            this.textBoxTestMsKernel = new System.Windows.Forms.TextBox();
            this.textBoxTestMsSigma = new System.Windows.Forms.TextBox();
            this.buttonDetectUsingModifiedRatioOfHueOverIntensityMethod = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_RatioConstancyMethod_sigma = new System.Windows.Forms.TextBox();
            this.textBox_RatioConstancyMethod_kernel = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.buttonRobbyTanFogRemovalMethod = new System.Windows.Forms.Button();
            this.buttonRemoveFogUsingMedianChannelPrior = new System.Windows.Forms.Button();
            this.buttonRemoveFogUsingIdcpWithClahe = new System.Windows.Forms.Button();
            this.buttonApplyAGC = new System.Windows.Forms.Button();
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads = new System.Windows.Forms.Button();
            this.buttonRemoveFogUsingDCPAndDFT = new System.Windows.Forms.Button();
            this.buttonRemoveFogUsingCustomMethod = new System.Windows.Forms.Button();
            this.buttonRemoveFogUsingLocalExtremaMethod = new System.Windows.Forms.Button();
            this.buttonRemoveFogUsingPhysicsBasedMethod = new System.Windows.Forms.Button();
            this.textBoxFvmMetric = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation = new System.Windows.Forms.Button();
            this.buttonRemoveFogUsingMultiCoreDSPMethod = new System.Windows.Forms.Button();
            this.buttonRunAllMethods = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxMseMetric = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxNaeMetric = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBoxScMetric = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBoxPsnrMEtric = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBoxAdMetric = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.textBoxRMSMetricDiff = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.textBoxShannonEntropyDiff = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.textBoxRMSMetric = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.textBoxShannonEntropy = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.textBoxMEthodExecTime = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.checkBoxUpdateStats = new System.Windows.Forms.CheckBox();
            this.checkBoxRunAllMethodsFog = new System.Windows.Forms.CheckBox();
            this.checkBoxRunAllMethodsDust = new System.Windows.Forms.CheckBox();
            this.checkBoxRunAllMethodsSaveAllImages = new System.Windows.Forms.CheckBox();
            this.label26 = new System.Windows.Forms.Label();
            this.textBoxSSIMMetric = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.textBox_RatioConstancyMethod_Imax = new System.Windows.Forms.TextBox();
            this.textBox_RatioConstancyMethod_Imin = new System.Windows.Forms.TextBox();
            this.buttonComputeMetrics = new System.Windows.Forms.Button();
            this.buttonTestVideo = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label11 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.textBoxShadowEdgeInpaint_KernelRadius = new System.Windows.Forms.TextBox();
            this.textBoxShadowEdgeInpaint_DilationKernelSize = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.textBoxShadowEdgeGaussian_KernelRadius = new System.Windows.Forms.TextBox();
            this.textBoxShadowEdgeGaussian_DilationKernelSize = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.textBoxShadowEdgeMedian_KernelRadius = new System.Windows.Forms.TextBox();
            this.textBoxShadowEdgeMedian_DilationKernelSIze = new System.Windows.Forms.TextBox();
            this.checkBoxMinifyLargeImages = new System.Windows.Forms.CheckBox();
            this.groupBoxComputeMetrics = new System.Windows.Forms.GroupBox();
            this.checkBoxUniformFog = new System.Windows.Forms.CheckBox();
            this.checkBoxHeterogeneousFog = new System.Windows.Forms.CheckBox();
            this.checkBoxCloudyHeterogeneousFog = new System.Windows.Forms.CheckBox();
            this.checkBoxCloudyFog = new System.Windows.Forms.CheckBox();
            this.checkBoxTestDustMethods = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.groupBoxComputeMetrics.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(8, 51);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(403, 391);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(417, 51);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(403, 391);
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Location = new System.Drawing.Point(826, 51);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(403, 391);
            this.pictureBox3.TabIndex = 7;
            this.pictureBox3.TabStop = false;
            // 
            // buttonShadowDetectionLab
            // 
            this.buttonShadowDetectionLab.BackColor = System.Drawing.SystemColors.Control;
            this.buttonShadowDetectionLab.Location = new System.Drawing.Point(263, 473);
            this.buttonShadowDetectionLab.Name = "buttonShadowDetectionLab";
            this.buttonShadowDetectionLab.Size = new System.Drawing.Size(75, 34);
            this.buttonShadowDetectionLab.TabIndex = 8;
            this.buttonShadowDetectionLab.Text = "LAB";
            this.buttonShadowDetectionLab.UseVisualStyleBackColor = false;
            this.buttonShadowDetectionLab.Click += new System.EventHandler(this.buttonShadowDetectionLab_Click);
            // 
            // textBoxShadowDetectionLabThreshold
            // 
            this.textBoxShadowDetectionLabThreshold.Location = new System.Drawing.Point(344, 486);
            this.textBoxShadowDetectionLabThreshold.Name = "textBoxShadowDetectionLabThreshold";
            this.textBoxShadowDetectionLabThreshold.Size = new System.Drawing.Size(50, 20);
            this.textBoxShadowDetectionLabThreshold.TabIndex = 9;
            // 
            // buttonShadowDetectionMS
            // 
            this.buttonShadowDetectionMS.BackColor = System.Drawing.SystemColors.Control;
            this.buttonShadowDetectionMS.Location = new System.Drawing.Point(263, 510);
            this.buttonShadowDetectionMS.Name = "buttonShadowDetectionMS";
            this.buttonShadowDetectionMS.Size = new System.Drawing.Size(75, 34);
            this.buttonShadowDetectionMS.TabIndex = 10;
            this.buttonShadowDetectionMS.Text = "MS";
            this.buttonShadowDetectionMS.UseVisualStyleBackColor = false;
            this.buttonShadowDetectionMS.Click += new System.EventHandler(this.buttonShadowDetectionMS_Click);
            // 
            // textBoxShadowDetectionLMSThreshold
            // 
            this.textBoxShadowDetectionLMSThreshold.Location = new System.Drawing.Point(344, 523);
            this.textBoxShadowDetectionLMSThreshold.Name = "textBoxShadowDetectionLMSThreshold";
            this.textBoxShadowDetectionLMSThreshold.Size = new System.Drawing.Size(50, 20);
            this.textBoxShadowDetectionLMSThreshold.TabIndex = 11;
            // 
            // buttonShadowRemovalAditiveMethod
            // 
            this.buttonShadowRemovalAditiveMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonShadowRemovalAditiveMethod.Location = new System.Drawing.Point(417, 465);
            this.buttonShadowRemovalAditiveMethod.Name = "buttonShadowRemovalAditiveMethod";
            this.buttonShadowRemovalAditiveMethod.Size = new System.Drawing.Size(75, 27);
            this.buttonShadowRemovalAditiveMethod.TabIndex = 15;
            this.buttonShadowRemovalAditiveMethod.Text = "Aditive";
            this.buttonShadowRemovalAditiveMethod.UseVisualStyleBackColor = false;
            this.buttonShadowRemovalAditiveMethod.Click += new System.EventHandler(this.buttonShadowRemovalAditiveMethod_Click);
            // 
            // buttonShadowRemovalConstantMethod
            // 
            this.buttonShadowRemovalConstantMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonShadowRemovalConstantMethod.Location = new System.Drawing.Point(417, 594);
            this.buttonShadowRemovalConstantMethod.Name = "buttonShadowRemovalConstantMethod";
            this.buttonShadowRemovalConstantMethod.Size = new System.Drawing.Size(75, 27);
            this.buttonShadowRemovalConstantMethod.TabIndex = 16;
            this.buttonShadowRemovalConstantMethod.Text = "Constant";
            this.buttonShadowRemovalConstantMethod.UseVisualStyleBackColor = false;
            this.buttonShadowRemovalConstantMethod.Click += new System.EventHandler(this.buttonShadowRemovalConstantMethod_Click);
            // 
            // buttonShadowRemovalLabMethod
            // 
            this.buttonShadowRemovalLabMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonShadowRemovalLabMethod.Location = new System.Drawing.Point(417, 561);
            this.buttonShadowRemovalLabMethod.Name = "buttonShadowRemovalLabMethod";
            this.buttonShadowRemovalLabMethod.Size = new System.Drawing.Size(75, 27);
            this.buttonShadowRemovalLabMethod.TabIndex = 17;
            this.buttonShadowRemovalLabMethod.Text = "LAB";
            this.buttonShadowRemovalLabMethod.UseVisualStyleBackColor = false;
            this.buttonShadowRemovalLabMethod.Click += new System.EventHandler(this.buttonShadowRemovalLabMethod_Click);
            // 
            // buttonShadowRemovalCombinedMethod
            // 
            this.buttonShadowRemovalCombinedMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonShadowRemovalCombinedMethod.Location = new System.Drawing.Point(417, 528);
            this.buttonShadowRemovalCombinedMethod.Name = "buttonShadowRemovalCombinedMethod";
            this.buttonShadowRemovalCombinedMethod.Size = new System.Drawing.Size(75, 27);
            this.buttonShadowRemovalCombinedMethod.TabIndex = 18;
            this.buttonShadowRemovalCombinedMethod.Text = "Combined";
            this.buttonShadowRemovalCombinedMethod.UseVisualStyleBackColor = false;
            this.buttonShadowRemovalCombinedMethod.Click += new System.EventHandler(this.buttonShadowRemovalCombinedMethod_Click);
            // 
            // buttonShadowRemovalBasicLightModelMethod
            // 
            this.buttonShadowRemovalBasicLightModelMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonShadowRemovalBasicLightModelMethod.Location = new System.Drawing.Point(417, 498);
            this.buttonShadowRemovalBasicLightModelMethod.Name = "buttonShadowRemovalBasicLightModelMethod";
            this.buttonShadowRemovalBasicLightModelMethod.Size = new System.Drawing.Size(75, 27);
            this.buttonShadowRemovalBasicLightModelMethod.TabIndex = 19;
            this.buttonShadowRemovalBasicLightModelMethod.Text = "Basic";
            this.buttonShadowRemovalBasicLightModelMethod.UseVisualStyleBackColor = false;
            this.buttonShadowRemovalBasicLightModelMethod.Click += new System.EventHandler(this.buttonShadowRemovalBasicLightModelMethod_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(260, 445);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 15);
            this.label1.TabIndex = 20;
            this.label1.Text = "Shadow detection";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(414, 445);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 15);
            this.label2.TabIndex = 21;
            this.label2.Text = "Shadow removal";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(528, 447);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 15);
            this.label3.TabIndex = 22;
            this.label3.Text = "Shadow edges processing";
            // 
            // buttonImpaintShadowEdges
            // 
            this.buttonImpaintShadowEdges.BackColor = System.Drawing.SystemColors.Control;
            this.buttonImpaintShadowEdges.Location = new System.Drawing.Point(531, 469);
            this.buttonImpaintShadowEdges.Name = "buttonImpaintShadowEdges";
            this.buttonImpaintShadowEdges.Size = new System.Drawing.Size(97, 32);
            this.buttonImpaintShadowEdges.TabIndex = 23;
            this.buttonImpaintShadowEdges.Text = "Inpaint";
            this.buttonImpaintShadowEdges.UseVisualStyleBackColor = false;
            this.buttonImpaintShadowEdges.Click += new System.EventHandler(this.buttonImpaintShadowEdges_Click);
            // 
            // buttonSmoothShadowEdgesUsingGaussianFilter
            // 
            this.buttonSmoothShadowEdgesUsingGaussianFilter.BackColor = System.Drawing.SystemColors.Control;
            this.buttonSmoothShadowEdgesUsingGaussianFilter.Location = new System.Drawing.Point(531, 506);
            this.buttonSmoothShadowEdgesUsingGaussianFilter.Name = "buttonSmoothShadowEdgesUsingGaussianFilter";
            this.buttonSmoothShadowEdgesUsingGaussianFilter.Size = new System.Drawing.Size(97, 36);
            this.buttonSmoothShadowEdgesUsingGaussianFilter.TabIndex = 24;
            this.buttonSmoothShadowEdgesUsingGaussianFilter.Text = "Gaussian filter";
            this.buttonSmoothShadowEdgesUsingGaussianFilter.UseVisualStyleBackColor = false;
            this.buttonSmoothShadowEdgesUsingGaussianFilter.Click += new System.EventHandler(this.buttonSmoothShadowEdgesUsingGaussianFilter_Click);
            // 
            // buttonSmoothShadowEdgesUsingMedianFilter
            // 
            this.buttonSmoothShadowEdgesUsingMedianFilter.BackColor = System.Drawing.SystemColors.Control;
            this.buttonSmoothShadowEdgesUsingMedianFilter.Location = new System.Drawing.Point(531, 548);
            this.buttonSmoothShadowEdgesUsingMedianFilter.Name = "buttonSmoothShadowEdgesUsingMedianFilter";
            this.buttonSmoothShadowEdgesUsingMedianFilter.Size = new System.Drawing.Size(97, 34);
            this.buttonSmoothShadowEdgesUsingMedianFilter.TabIndex = 25;
            this.buttonSmoothShadowEdgesUsingMedianFilter.Text = "Median filter";
            this.buttonSmoothShadowEdgesUsingMedianFilter.UseVisualStyleBackColor = false;
            this.buttonSmoothShadowEdgesUsingMedianFilter.Click += new System.EventHandler(this.buttonSmoothShadowEdgesUsingMedianFilter_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(709, 448);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 15);
            this.label4.TabIndex = 26;
            this.label4.Text = "Fog removal";
            // 
            // buttonRemoveFogUsingDarkChannelMethod
            // 
            this.buttonRemoveFogUsingDarkChannelMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingDarkChannelMethod.Location = new System.Drawing.Point(712, 469);
            this.buttonRemoveFogUsingDarkChannelMethod.Name = "buttonRemoveFogUsingDarkChannelMethod";
            this.buttonRemoveFogUsingDarkChannelMethod.Size = new System.Drawing.Size(108, 32);
            this.buttonRemoveFogUsingDarkChannelMethod.TabIndex = 27;
            this.buttonRemoveFogUsingDarkChannelMethod.Text = "DCP";
            this.buttonRemoveFogUsingDarkChannelMethod.UseVisualStyleBackColor = false;
            this.buttonRemoveFogUsingDarkChannelMethod.Click += new System.EventHandler(this.buttonRemoveFogUsingDarkChannelMethod_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.windowsToolStripMenuItem,
            this.imageToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1235, 24);
            this.menuStrip1.TabIndex = 28;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveDetectionResultToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exportMetricsToCSVToolStripMenuItem,
            this.resetMethodsStatisticsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveDetectionResultToolStripMenuItem
            // 
            this.saveDetectionResultToolStripMenuItem.Name = "saveDetectionResultToolStripMenuItem";
            this.saveDetectionResultToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.saveDetectionResultToolStripMenuItem.Text = "Save (Detection result)";
            this.saveDetectionResultToolStripMenuItem.Click += new System.EventHandler(this.saveDetectionResultToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exportMetricsToCSVToolStripMenuItem
            // 
            this.exportMetricsToCSVToolStripMenuItem.Name = "exportMetricsToCSVToolStripMenuItem";
            this.exportMetricsToCSVToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.exportMetricsToCSVToolStripMenuItem.Text = "Export Metrics to CSV";
            this.exportMetricsToCSVToolStripMenuItem.Click += new System.EventHandler(this.exportMetricsToCSVToolStripMenuItem_Click);
            // 
            // resetMethodsStatisticsToolStripMenuItem
            // 
            this.resetMethodsStatisticsToolStripMenuItem.Name = "resetMethodsStatisticsToolStripMenuItem";
            this.resetMethodsStatisticsToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.resetMethodsStatisticsToolStripMenuItem.Text = "Reset methods statistics";
            this.resetMethodsStatisticsToolStripMenuItem.Click += new System.EventHandler(this.resetMethodsStatisticsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeAllToolStripMenuItem});
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.windowsToolStripMenuItem.Text = "Windows";
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.closeAllToolStripMenuItem.Text = "Close all";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.closeAllToolStripMenuItem_Click);
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreToolStripMenuItem});
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.imageToolStripMenuItem.Text = "Image";
            // 
            // restoreToolStripMenuItem
            // 
            this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            this.restoreToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.restoreToolStripMenuItem.Text = "Restore";
            this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
            // 
            // checkBoxShowOptionalWindows
            // 
            this.checkBoxShowOptionalWindows.AutoSize = true;
            this.checkBoxShowOptionalWindows.Location = new System.Drawing.Point(6, 19);
            this.checkBoxShowOptionalWindows.Name = "checkBoxShowOptionalWindows";
            this.checkBoxShowOptionalWindows.Size = new System.Drawing.Size(148, 17);
            this.checkBoxShowOptionalWindows.TabIndex = 29;
            this.checkBoxShowOptionalWindows.Text = "Show Optional Windows?";
            this.checkBoxShowOptionalWindows.UseVisualStyleBackColor = true;
            // 
            // buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod
            // 
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.Location = new System.Drawing.Point(846, 469);
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.Name = "buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMet" +
    "hod";
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.Size = new System.Drawing.Size(88, 31);
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.TabIndex = 31;
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.Text = "TTFIO";
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.UseVisualStyleBackColor = false;
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.Click += new System.EventHandler(this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(843, 447);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 15);
            this.label5.TabIndex = 30;
            this.label5.Text = "Dust removal";
            // 
            // buttonShadowRemovalLabMethod2
            // 
            this.buttonShadowRemovalLabMethod2.Location = new System.Drawing.Point(423, 702);
            this.buttonShadowRemovalLabMethod2.Name = "buttonShadowRemovalLabMethod2";
            this.buttonShadowRemovalLabMethod2.Size = new System.Drawing.Size(75, 23);
            this.buttonShadowRemovalLabMethod2.TabIndex = 32;
            this.buttonShadowRemovalLabMethod2.Text = "LAB2";
            this.buttonShadowRemovalLabMethod2.UseVisualStyleBackColor = true;
            this.buttonShadowRemovalLabMethod2.Visible = false;
            this.buttonShadowRemovalLabMethod2.Click += new System.EventHandler(this.buttonShadowRemovalLabMethod2_Click);
            // 
            // buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod
            // 
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod.Location = new System.Drawing.Point(846, 507);
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod.Name = "buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod";
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod.Size = new System.Drawing.Size(88, 32);
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod.TabIndex = 33;
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod.Text = "RGBRRC";
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod.UseVisualStyleBackColor = false;
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod.Click += new System.EventHandler(this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod_Click);
            // 
            // buttonTestEmguCVCudaMeanShift
            // 
            this.buttonTestEmguCVCudaMeanShift.Location = new System.Drawing.Point(276, 702);
            this.buttonTestEmguCVCudaMeanShift.Name = "buttonTestEmguCVCudaMeanShift";
            this.buttonTestEmguCVCudaMeanShift.Size = new System.Drawing.Size(141, 23);
            this.buttonTestEmguCVCudaMeanShift.TabIndex = 34;
            this.buttonTestEmguCVCudaMeanShift.Text = "TestEmguCVCudaMeanShift";
            this.buttonTestEmguCVCudaMeanShift.UseVisualStyleBackColor = true;
            this.buttonTestEmguCVCudaMeanShift.Visible = false;
            this.buttonTestEmguCVCudaMeanShift.Click += new System.EventHandler(this.buttonTestEmguCVCudaMeanShift_Click);
            // 
            // buttonMSTest
            // 
            this.buttonMSTest.Location = new System.Drawing.Point(276, 674);
            this.buttonMSTest.Name = "buttonMSTest";
            this.buttonMSTest.Size = new System.Drawing.Size(75, 23);
            this.buttonMSTest.TabIndex = 35;
            this.buttonMSTest.Text = "TestMS";
            this.buttonMSTest.UseVisualStyleBackColor = true;
            this.buttonMSTest.Visible = false;
            this.buttonMSTest.Click += new System.EventHandler(this.buttonMSTest_Click);
            // 
            // textBoxTestMsKernel
            // 
            this.textBoxTestMsKernel.Location = new System.Drawing.Point(357, 676);
            this.textBoxTestMsKernel.Name = "textBoxTestMsKernel";
            this.textBoxTestMsKernel.Size = new System.Drawing.Size(28, 20);
            this.textBoxTestMsKernel.TabIndex = 36;
            this.textBoxTestMsKernel.Visible = false;
            // 
            // textBoxTestMsSigma
            // 
            this.textBoxTestMsSigma.Location = new System.Drawing.Point(391, 677);
            this.textBoxTestMsSigma.Name = "textBoxTestMsSigma";
            this.textBoxTestMsSigma.Size = new System.Drawing.Size(28, 20);
            this.textBoxTestMsSigma.TabIndex = 37;
            this.textBoxTestMsSigma.Visible = false;
            // 
            // buttonDetectUsingModifiedRatioOfHueOverIntensityMethod
            // 
            this.buttonDetectUsingModifiedRatioOfHueOverIntensityMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonDetectUsingModifiedRatioOfHueOverIntensityMethod.Location = new System.Drawing.Point(263, 547);
            this.buttonDetectUsingModifiedRatioOfHueOverIntensityMethod.Name = "buttonDetectUsingModifiedRatioOfHueOverIntensityMethod";
            this.buttonDetectUsingModifiedRatioOfHueOverIntensityMethod.Size = new System.Drawing.Size(75, 34);
            this.buttonDetectUsingModifiedRatioOfHueOverIntensityMethod.TabIndex = 38;
            this.buttonDetectUsingModifiedRatioOfHueOverIntensityMethod.Text = "HSI";
            this.buttonDetectUsingModifiedRatioOfHueOverIntensityMethod.UseVisualStyleBackColor = false;
            this.buttonDetectUsingModifiedRatioOfHueOverIntensityMethod.Click += new System.EventHandler(this.buttonDetectUsingModifiedRatioOfHueOverIntensityMethod_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(354, 660);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 39;
            this.label6.Text = "kernel";
            this.label6.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(388, 661);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 40;
            this.label7.Text = "sigma";
            this.label7.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(971, 503);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 44;
            this.label8.Text = "sigma";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(937, 503);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 43;
            this.label9.Text = "kernel";
            // 
            // textBox_RatioConstancyMethod_sigma
            // 
            this.textBox_RatioConstancyMethod_sigma.Location = new System.Drawing.Point(974, 519);
            this.textBox_RatioConstancyMethod_sigma.Name = "textBox_RatioConstancyMethod_sigma";
            this.textBox_RatioConstancyMethod_sigma.Size = new System.Drawing.Size(28, 20);
            this.textBox_RatioConstancyMethod_sigma.TabIndex = 42;
            // 
            // textBox_RatioConstancyMethod_kernel
            // 
            this.textBox_RatioConstancyMethod_kernel.Location = new System.Drawing.Point(940, 519);
            this.textBox_RatioConstancyMethod_kernel.Name = "textBox_RatioConstancyMethod_kernel";
            this.textBox_RatioConstancyMethod_kernel.Size = new System.Drawing.Size(28, 20);
            this.textBox_RatioConstancyMethod_kernel.TabIndex = 41;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(937, 466);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 46;
            this.label10.Text = "dzeta (ζ)";
            // 
            // textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta
            // 
            this.textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta.Location = new System.Drawing.Point(940, 480);
            this.textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta.Name = "textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta";
            this.textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta.Size = new System.Drawing.Size(44, 20);
            this.textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta.TabIndex = 45;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(344, 509);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(50, 13);
            this.label12.TabIndex = 48;
            this.label12.Text = "threshold";
            // 
            // buttonRobbyTanFogRemovalMethod
            // 
            this.buttonRobbyTanFogRemovalMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRobbyTanFogRemovalMethod.Location = new System.Drawing.Point(273, 734);
            this.buttonRobbyTanFogRemovalMethod.Name = "buttonRobbyTanFogRemovalMethod";
            this.buttonRobbyTanFogRemovalMethod.Size = new System.Drawing.Size(121, 32);
            this.buttonRobbyTanFogRemovalMethod.TabIndex = 49;
            this.buttonRobbyTanFogRemovalMethod.Text = "RobbyTan UC";
            this.buttonRobbyTanFogRemovalMethod.UseVisualStyleBackColor = false;
            this.buttonRobbyTanFogRemovalMethod.Visible = false;
            this.buttonRobbyTanFogRemovalMethod.Click += new System.EventHandler(this.buttonRobbyTanFogRemovalMethod_Click);
            // 
            // buttonRemoveFogUsingMedianChannelPrior
            // 
            this.buttonRemoveFogUsingMedianChannelPrior.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingMedianChannelPrior.Location = new System.Drawing.Point(712, 507);
            this.buttonRemoveFogUsingMedianChannelPrior.Name = "buttonRemoveFogUsingMedianChannelPrior";
            this.buttonRemoveFogUsingMedianChannelPrior.Size = new System.Drawing.Size(108, 32);
            this.buttonRemoveFogUsingMedianChannelPrior.TabIndex = 50;
            this.buttonRemoveFogUsingMedianChannelPrior.Text = "MCP";
            this.buttonRemoveFogUsingMedianChannelPrior.UseVisualStyleBackColor = false;
            this.buttonRemoveFogUsingMedianChannelPrior.Click += new System.EventHandler(this.buttonRemoveFogUsingMedianChannelPrior_Click);
            // 
            // buttonRemoveFogUsingIdcpWithClahe
            // 
            this.buttonRemoveFogUsingIdcpWithClahe.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingIdcpWithClahe.Location = new System.Drawing.Point(712, 545);
            this.buttonRemoveFogUsingIdcpWithClahe.Name = "buttonRemoveFogUsingIdcpWithClahe";
            this.buttonRemoveFogUsingIdcpWithClahe.Size = new System.Drawing.Size(108, 32);
            this.buttonRemoveFogUsingIdcpWithClahe.TabIndex = 51;
            this.buttonRemoveFogUsingIdcpWithClahe.Text = "IDCP with CLAHE";
            this.buttonRemoveFogUsingIdcpWithClahe.UseVisualStyleBackColor = false;
            this.buttonRemoveFogUsingIdcpWithClahe.Click += new System.EventHandler(this.buttonRemoveFogUsingIdcpWithClahe_Click);
            // 
            // buttonApplyAGC
            // 
            this.buttonApplyAGC.BackColor = System.Drawing.SystemColors.Control;
            this.buttonApplyAGC.Location = new System.Drawing.Point(1117, 466);
            this.buttonApplyAGC.Name = "buttonApplyAGC";
            this.buttonApplyAGC.Size = new System.Drawing.Size(97, 34);
            this.buttonApplyAGC.TabIndex = 52;
            this.buttonApplyAGC.Text = "AGC";
            this.buttonApplyAGC.UseVisualStyleBackColor = false;
            this.buttonApplyAGC.Click += new System.EventHandler(this.buttonApplyAGC_Click);
            // 
            // buttonEnhaceVisibilityUsingRobbyTanMethodForRoads
            // 
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.BackColor = System.Drawing.SystemColors.Control;
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.Location = new System.Drawing.Point(846, 545);
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.Name = "buttonEnhaceVisibilityUsingRobbyTanMethodForRoads";
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.Size = new System.Drawing.Size(88, 32);
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.TabIndex = 54;
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.Text = "RTFR";
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.UseVisualStyleBackColor = false;
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.Click += new System.EventHandler(this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads_Click);
            // 
            // buttonRemoveFogUsingDCPAndDFT
            // 
            this.buttonRemoveFogUsingDCPAndDFT.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingDCPAndDFT.Location = new System.Drawing.Point(712, 583);
            this.buttonRemoveFogUsingDCPAndDFT.Name = "buttonRemoveFogUsingDCPAndDFT";
            this.buttonRemoveFogUsingDCPAndDFT.Size = new System.Drawing.Size(108, 32);
            this.buttonRemoveFogUsingDCPAndDFT.TabIndex = 55;
            this.buttonRemoveFogUsingDCPAndDFT.Text = "DCP with DFT";
            this.buttonRemoveFogUsingDCPAndDFT.UseVisualStyleBackColor = false;
            this.buttonRemoveFogUsingDCPAndDFT.Click += new System.EventHandler(this.buttonRemoveFogUsingDCPAndDFT_Click);
            // 
            // buttonRemoveFogUsingCustomMethod
            // 
            this.buttonRemoveFogUsingCustomMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingCustomMethod.Location = new System.Drawing.Point(712, 659);
            this.buttonRemoveFogUsingCustomMethod.Name = "buttonRemoveFogUsingCustomMethod";
            this.buttonRemoveFogUsingCustomMethod.Size = new System.Drawing.Size(108, 32);
            this.buttonRemoveFogUsingCustomMethod.TabIndex = 56;
            this.buttonRemoveFogUsingCustomMethod.Text = "CUS";
            this.buttonRemoveFogUsingCustomMethod.UseVisualStyleBackColor = false;
            this.buttonRemoveFogUsingCustomMethod.Click += new System.EventHandler(this.buttonRemoveFogUsingCustomMethod_Click);
            // 
            // buttonRemoveFogUsingLocalExtremaMethod
            // 
            this.buttonRemoveFogUsingLocalExtremaMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingLocalExtremaMethod.Location = new System.Drawing.Point(527, 734);
            this.buttonRemoveFogUsingLocalExtremaMethod.Name = "buttonRemoveFogUsingLocalExtremaMethod";
            this.buttonRemoveFogUsingLocalExtremaMethod.Size = new System.Drawing.Size(121, 32);
            this.buttonRemoveFogUsingLocalExtremaMethod.TabIndex = 57;
            this.buttonRemoveFogUsingLocalExtremaMethod.Text = "Local Extrema UC";
            this.buttonRemoveFogUsingLocalExtremaMethod.UseVisualStyleBackColor = false;
            this.buttonRemoveFogUsingLocalExtremaMethod.Visible = false;
            this.buttonRemoveFogUsingLocalExtremaMethod.Click += new System.EventHandler(this.buttonRemoveFogUsingLocalExtremaMethod_Click);
            // 
            // buttonRemoveFogUsingPhysicsBasedMethod
            // 
            this.buttonRemoveFogUsingPhysicsBasedMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingPhysicsBasedMethod.Location = new System.Drawing.Point(400, 734);
            this.buttonRemoveFogUsingPhysicsBasedMethod.Name = "buttonRemoveFogUsingPhysicsBasedMethod";
            this.buttonRemoveFogUsingPhysicsBasedMethod.Size = new System.Drawing.Size(121, 32);
            this.buttonRemoveFogUsingPhysicsBasedMethod.TabIndex = 58;
            this.buttonRemoveFogUsingPhysicsBasedMethod.Text = "Physics Based UC";
            this.buttonRemoveFogUsingPhysicsBasedMethod.UseVisualStyleBackColor = false;
            this.buttonRemoveFogUsingPhysicsBasedMethod.Visible = false;
            this.buttonRemoveFogUsingPhysicsBasedMethod.Click += new System.EventHandler(this.buttonRemoveFogUsingPhysicsBasedMethod_Click);
            // 
            // textBoxFvmMetric
            // 
            this.textBoxFvmMetric.Location = new System.Drawing.Point(903, 638);
            this.textBoxFvmMetric.Name = "textBoxFvmMetric";
            this.textBoxFvmMetric.Size = new System.Drawing.Size(60, 20);
            this.textBoxFvmMetric.TabIndex = 59;
            this.textBoxFvmMetric.Visible = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(850, 641);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 13);
            this.label13.TabIndex = 60;
            this.label13.Text = "FVM";
            this.label13.Visible = false;
            // 
            // buttonRemoveFogUsingCustomMethodWithDepthEstimation
            // 
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.Location = new System.Drawing.Point(712, 697);
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.Name = "buttonRemoveFogUsingCustomMethodWithDepthEstimation";
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.Size = new System.Drawing.Size(108, 32);
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.TabIndex = 61;
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.Text = "CUSD";
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.UseVisualStyleBackColor = false;
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.Click += new System.EventHandler(this.buttonRemoveFogUsingCustomMethodWithDepthEstimation_Click);
            // 
            // buttonRemoveFogUsingMultiCoreDSPMethod
            // 
            this.buttonRemoveFogUsingMultiCoreDSPMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingMultiCoreDSPMethod.Location = new System.Drawing.Point(712, 621);
            this.buttonRemoveFogUsingMultiCoreDSPMethod.Name = "buttonRemoveFogUsingMultiCoreDSPMethod";
            this.buttonRemoveFogUsingMultiCoreDSPMethod.Size = new System.Drawing.Size(108, 32);
            this.buttonRemoveFogUsingMultiCoreDSPMethod.TabIndex = 62;
            this.buttonRemoveFogUsingMultiCoreDSPMethod.Text = "DSP";
            this.buttonRemoveFogUsingMultiCoreDSPMethod.UseVisualStyleBackColor = false;
            this.buttonRemoveFogUsingMultiCoreDSPMethod.Click += new System.EventHandler(this.buttonRemoveFogUsingMultiCoreDSPMethod_Click);
            // 
            // buttonRunAllMethods
            // 
            this.buttonRunAllMethods.Location = new System.Drawing.Point(162, 12);
            this.buttonRunAllMethods.Name = "buttonRunAllMethods";
            this.buttonRunAllMethods.Size = new System.Drawing.Size(73, 95);
            this.buttonRunAllMethods.TabIndex = 63;
            this.buttonRunAllMethods.Text = "Start";
            this.buttonRunAllMethods.UseVisualStyleBackColor = true;
            this.buttonRunAllMethods.Click += new System.EventHandler(this.buttonRunAllMethods_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(850, 665);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(30, 13);
            this.label14.TabIndex = 65;
            this.label14.Text = "MSE";
            this.label14.Visible = false;
            // 
            // textBoxMseMetric
            // 
            this.textBoxMseMetric.Location = new System.Drawing.Point(903, 662);
            this.textBoxMseMetric.Name = "textBoxMseMetric";
            this.textBoxMseMetric.Size = new System.Drawing.Size(60, 20);
            this.textBoxMseMetric.TabIndex = 64;
            this.textBoxMseMetric.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(850, 689);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(29, 13);
            this.label15.TabIndex = 67;
            this.label15.Text = "NAE";
            this.label15.Visible = false;
            // 
            // textBoxNaeMetric
            // 
            this.textBoxNaeMetric.Location = new System.Drawing.Point(903, 686);
            this.textBoxNaeMetric.Name = "textBoxNaeMetric";
            this.textBoxNaeMetric.Size = new System.Drawing.Size(60, 20);
            this.textBoxNaeMetric.TabIndex = 66;
            this.textBoxNaeMetric.Visible = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(985, 637);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(21, 13);
            this.label16.TabIndex = 69;
            this.label16.Text = "SC";
            this.label16.Visible = false;
            // 
            // textBoxScMetric
            // 
            this.textBoxScMetric.Location = new System.Drawing.Point(1037, 633);
            this.textBoxScMetric.Name = "textBoxScMetric";
            this.textBoxScMetric.Size = new System.Drawing.Size(60, 20);
            this.textBoxScMetric.TabIndex = 68;
            this.textBoxScMetric.Visible = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(985, 663);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(37, 13);
            this.label17.TabIndex = 71;
            this.label17.Text = "PSNR";
            this.label17.Visible = false;
            // 
            // textBoxPsnrMEtric
            // 
            this.textBoxPsnrMEtric.Location = new System.Drawing.Point(1037, 660);
            this.textBoxPsnrMEtric.Name = "textBoxPsnrMEtric";
            this.textBoxPsnrMEtric.Size = new System.Drawing.Size(60, 20);
            this.textBoxPsnrMEtric.TabIndex = 70;
            this.textBoxPsnrMEtric.Visible = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(985, 689);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(22, 13);
            this.label18.TabIndex = 73;
            this.label18.Text = "AD";
            this.label18.Visible = false;
            // 
            // textBoxAdMetric
            // 
            this.textBoxAdMetric.Location = new System.Drawing.Point(1037, 686);
            this.textBoxAdMetric.Name = "textBoxAdMetric";
            this.textBoxAdMetric.Size = new System.Drawing.Size(60, 20);
            this.textBoxAdMetric.TabIndex = 72;
            this.textBoxAdMetric.Visible = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(984, 714);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(47, 13);
            this.label19.TabIndex = 75;
            this.label19.Text = "RMSDiff";
            this.label19.Visible = false;
            // 
            // textBoxRMSMetricDiff
            // 
            this.textBoxRMSMetricDiff.Location = new System.Drawing.Point(1037, 711);
            this.textBoxRMSMetricDiff.Name = "textBoxRMSMetricDiff";
            this.textBoxRMSMetricDiff.Size = new System.Drawing.Size(60, 20);
            this.textBoxRMSMetricDiff.TabIndex = 74;
            this.textBoxRMSMetricDiff.Visible = false;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(984, 740);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(50, 39);
            this.label20.TabIndex = 77;
            this.label20.Text = "Shannon\r\nEntropy\r\nDiff";
            this.label20.Visible = false;
            // 
            // textBoxShannonEntropyDiff
            // 
            this.textBoxShannonEntropyDiff.Location = new System.Drawing.Point(1037, 737);
            this.textBoxShannonEntropyDiff.Name = "textBoxShannonEntropyDiff";
            this.textBoxShannonEntropyDiff.Size = new System.Drawing.Size(60, 20);
            this.textBoxShannonEntropyDiff.TabIndex = 76;
            this.textBoxShannonEntropyDiff.Visible = false;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(850, 717);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(31, 13);
            this.label21.TabIndex = 79;
            this.label21.Text = "RMS";
            this.label21.Visible = false;
            // 
            // textBoxRMSMetric
            // 
            this.textBoxRMSMetric.Location = new System.Drawing.Point(903, 714);
            this.textBoxRMSMetric.Name = "textBoxRMSMetric";
            this.textBoxRMSMetric.Size = new System.Drawing.Size(60, 20);
            this.textBoxRMSMetric.TabIndex = 78;
            this.textBoxRMSMetric.Visible = false;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(850, 743);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(50, 26);
            this.label22.TabIndex = 81;
            this.label22.Text = "Shannon\r\nEntropy\r";
            this.label22.Visible = false;
            // 
            // textBoxShannonEntropy
            // 
            this.textBoxShannonEntropy.Location = new System.Drawing.Point(903, 740);
            this.textBoxShannonEntropy.Name = "textBoxShannonEntropy";
            this.textBoxShannonEntropy.Size = new System.Drawing.Size(60, 20);
            this.textBoxShannonEntropy.TabIndex = 80;
            this.textBoxShannonEntropy.Visible = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(849, 615);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(30, 13);
            this.label23.TabIndex = 83;
            this.label23.Text = "Time";
            this.label23.Visible = false;
            // 
            // textBoxMEthodExecTime
            // 
            this.textBoxMEthodExecTime.Location = new System.Drawing.Point(903, 612);
            this.textBoxMEthodExecTime.Name = "textBoxMEthodExecTime";
            this.textBoxMEthodExecTime.Size = new System.Drawing.Size(60, 20);
            this.textBoxMEthodExecTime.TabIndex = 82;
            this.textBoxMEthodExecTime.Visible = false;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label24.Location = new System.Drawing.Point(1114, 448);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(42, 15);
            this.label24.TabIndex = 84;
            this.label24.Text = "Other";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label25.Location = new System.Drawing.Point(849, 594);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(54, 15);
            this.label25.TabIndex = 85;
            this.label25.Text = "Metrics";
            this.label25.Visible = false;
            // 
            // checkBoxUpdateStats
            // 
            this.checkBoxUpdateStats.AutoSize = true;
            this.checkBoxUpdateStats.Checked = true;
            this.checkBoxUpdateStats.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUpdateStats.Location = new System.Drawing.Point(6, 18);
            this.checkBoxUpdateStats.Name = "checkBoxUpdateStats";
            this.checkBoxUpdateStats.Size = new System.Drawing.Size(92, 17);
            this.checkBoxUpdateStats.TabIndex = 86;
            this.checkBoxUpdateStats.Text = "Update stats?";
            this.checkBoxUpdateStats.UseVisualStyleBackColor = true;
            // 
            // checkBoxRunAllMethodsFog
            // 
            this.checkBoxRunAllMethodsFog.AutoSize = true;
            this.checkBoxRunAllMethodsFog.Checked = true;
            this.checkBoxRunAllMethodsFog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRunAllMethodsFog.Location = new System.Drawing.Point(6, 63);
            this.checkBoxRunAllMethodsFog.Name = "checkBoxRunAllMethodsFog";
            this.checkBoxRunAllMethodsFog.Size = new System.Drawing.Size(44, 17);
            this.checkBoxRunAllMethodsFog.TabIndex = 87;
            this.checkBoxRunAllMethodsFog.Text = "Fog";
            this.checkBoxRunAllMethodsFog.UseVisualStyleBackColor = true;
            // 
            // checkBoxRunAllMethodsDust
            // 
            this.checkBoxRunAllMethodsDust.AutoSize = true;
            this.checkBoxRunAllMethodsDust.Checked = true;
            this.checkBoxRunAllMethodsDust.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRunAllMethodsDust.Location = new System.Drawing.Point(6, 84);
            this.checkBoxRunAllMethodsDust.Name = "checkBoxRunAllMethodsDust";
            this.checkBoxRunAllMethodsDust.Size = new System.Drawing.Size(48, 17);
            this.checkBoxRunAllMethodsDust.TabIndex = 88;
            this.checkBoxRunAllMethodsDust.Text = "Dust";
            this.checkBoxRunAllMethodsDust.UseVisualStyleBackColor = true;
            // 
            // checkBoxRunAllMethodsSaveAllImages
            // 
            this.checkBoxRunAllMethodsSaveAllImages.AutoSize = true;
            this.checkBoxRunAllMethodsSaveAllImages.Checked = true;
            this.checkBoxRunAllMethodsSaveAllImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRunAllMethodsSaveAllImages.Location = new System.Drawing.Point(6, 40);
            this.checkBoxRunAllMethodsSaveAllImages.Name = "checkBoxRunAllMethodsSaveAllImages";
            this.checkBoxRunAllMethodsSaveAllImages.Size = new System.Drawing.Size(106, 17);
            this.checkBoxRunAllMethodsSaveAllImages.TabIndex = 89;
            this.checkBoxRunAllMethodsSaveAllImages.Text = "Save all images?";
            this.checkBoxRunAllMethodsSaveAllImages.UseVisualStyleBackColor = true;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(985, 612);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(33, 13);
            this.label26.TabIndex = 91;
            this.label26.Text = "SSIM";
            this.label26.Visible = false;
            // 
            // textBoxSSIMMetric
            // 
            this.textBoxSSIMMetric.Location = new System.Drawing.Point(1037, 608);
            this.textBoxSSIMMetric.Name = "textBoxSSIMMetric";
            this.textBoxSSIMMetric.Size = new System.Drawing.Size(60, 20);
            this.textBoxSSIMMetric.TabIndex = 90;
            this.textBoxSSIMMetric.Visible = false;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(1039, 503);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(29, 13);
            this.label27.TabIndex = 95;
            this.label27.Text = "Imax";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(1005, 503);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(26, 13);
            this.label28.TabIndex = 94;
            this.label28.Text = "Imin";
            // 
            // textBox_RatioConstancyMethod_Imax
            // 
            this.textBox_RatioConstancyMethod_Imax.Location = new System.Drawing.Point(1042, 519);
            this.textBox_RatioConstancyMethod_Imax.Name = "textBox_RatioConstancyMethod_Imax";
            this.textBox_RatioConstancyMethod_Imax.Size = new System.Drawing.Size(28, 20);
            this.textBox_RatioConstancyMethod_Imax.TabIndex = 93;
            // 
            // textBox_RatioConstancyMethod_Imin
            // 
            this.textBox_RatioConstancyMethod_Imin.Location = new System.Drawing.Point(1008, 519);
            this.textBox_RatioConstancyMethod_Imin.Name = "textBox_RatioConstancyMethod_Imin";
            this.textBox_RatioConstancyMethod_Imin.Size = new System.Drawing.Size(28, 20);
            this.textBox_RatioConstancyMethod_Imin.TabIndex = 92;
            // 
            // buttonComputeMetrics
            // 
            this.buttonComputeMetrics.Location = new System.Drawing.Point(162, 15);
            this.buttonComputeMetrics.Name = "buttonComputeMetrics";
            this.buttonComputeMetrics.Size = new System.Drawing.Size(73, 93);
            this.buttonComputeMetrics.TabIndex = 96;
            this.buttonComputeMetrics.Text = "Start";
            this.buttonComputeMetrics.UseVisualStyleBackColor = true;
            this.buttonComputeMetrics.Click += new System.EventHandler(this.buttonComputeMetrics_Click);
            // 
            // buttonTestVideo
            // 
            this.buttonTestVideo.Location = new System.Drawing.Point(275, 645);
            this.buttonTestVideo.Name = "buttonTestVideo";
            this.buttonTestVideo.Size = new System.Drawing.Size(75, 23);
            this.buttonTestVideo.TabIndex = 97;
            this.buttonTestVideo.Text = "Test video";
            this.buttonTestVideo.UseVisualStyleBackColor = true;
            this.buttonTestVideo.Visible = false;
            this.buttonTestVideo.Click += new System.EventHandler(this.buttonTestVideo_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipTitle = "asdasd";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(344, 473);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 13);
            this.label11.TabIndex = 47;
            this.label11.Text = "threshold";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(8, 35);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(62, 13);
            this.label29.TabIndex = 98;
            this.label29.Text = "Input image";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(414, 35);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(81, 13);
            this.label30.TabIndex = 99;
            this.label30.Text = "Detection result";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(823, 35);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(70, 13);
            this.label31.TabIndex = 100;
            this.label31.Text = "Output image";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(672, 465);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(35, 13);
            this.label32.TabIndex = 104;
            this.label32.Text = "radius";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(631, 465);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(42, 13);
            this.label33.TabIndex = 103;
            this.label33.Text = "dkernel";
            // 
            // textBoxShadowEdgeInpaint_KernelRadius
            // 
            this.textBoxShadowEdgeInpaint_KernelRadius.Location = new System.Drawing.Point(675, 481);
            this.textBoxShadowEdgeInpaint_KernelRadius.Name = "textBoxShadowEdgeInpaint_KernelRadius";
            this.textBoxShadowEdgeInpaint_KernelRadius.Size = new System.Drawing.Size(28, 20);
            this.textBoxShadowEdgeInpaint_KernelRadius.TabIndex = 102;
            // 
            // textBoxShadowEdgeInpaint_DilationKernelSize
            // 
            this.textBoxShadowEdgeInpaint_DilationKernelSize.Location = new System.Drawing.Point(634, 481);
            this.textBoxShadowEdgeInpaint_DilationKernelSize.Name = "textBoxShadowEdgeInpaint_DilationKernelSize";
            this.textBoxShadowEdgeInpaint_DilationKernelSize.Size = new System.Drawing.Size(28, 20);
            this.textBoxShadowEdgeInpaint_DilationKernelSize.TabIndex = 101;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(672, 506);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(36, 13);
            this.label34.TabIndex = 108;
            this.label34.Text = "kernel";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(631, 506);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(42, 13);
            this.label35.TabIndex = 107;
            this.label35.Text = "dkernel";
            // 
            // textBoxShadowEdgeGaussian_KernelRadius
            // 
            this.textBoxShadowEdgeGaussian_KernelRadius.Location = new System.Drawing.Point(675, 522);
            this.textBoxShadowEdgeGaussian_KernelRadius.Name = "textBoxShadowEdgeGaussian_KernelRadius";
            this.textBoxShadowEdgeGaussian_KernelRadius.Size = new System.Drawing.Size(28, 20);
            this.textBoxShadowEdgeGaussian_KernelRadius.TabIndex = 106;
            // 
            // textBoxShadowEdgeGaussian_DilationKernelSize
            // 
            this.textBoxShadowEdgeGaussian_DilationKernelSize.Location = new System.Drawing.Point(634, 522);
            this.textBoxShadowEdgeGaussian_DilationKernelSize.Name = "textBoxShadowEdgeGaussian_DilationKernelSize";
            this.textBoxShadowEdgeGaussian_DilationKernelSize.Size = new System.Drawing.Size(28, 20);
            this.textBoxShadowEdgeGaussian_DilationKernelSize.TabIndex = 105;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(672, 546);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(36, 13);
            this.label36.TabIndex = 112;
            this.label36.Text = "kernel";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(631, 546);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(42, 13);
            this.label37.TabIndex = 111;
            this.label37.Text = "dkernel";
            // 
            // textBoxShadowEdgeMedian_KernelRadius
            // 
            this.textBoxShadowEdgeMedian_KernelRadius.Location = new System.Drawing.Point(675, 562);
            this.textBoxShadowEdgeMedian_KernelRadius.Name = "textBoxShadowEdgeMedian_KernelRadius";
            this.textBoxShadowEdgeMedian_KernelRadius.Size = new System.Drawing.Size(28, 20);
            this.textBoxShadowEdgeMedian_KernelRadius.TabIndex = 110;
            // 
            // textBoxShadowEdgeMedian_DilationKernelSIze
            // 
            this.textBoxShadowEdgeMedian_DilationKernelSIze.Location = new System.Drawing.Point(634, 562);
            this.textBoxShadowEdgeMedian_DilationKernelSIze.Name = "textBoxShadowEdgeMedian_DilationKernelSIze";
            this.textBoxShadowEdgeMedian_DilationKernelSIze.Size = new System.Drawing.Size(28, 20);
            this.textBoxShadowEdgeMedian_DilationKernelSIze.TabIndex = 109;
            // 
            // checkBoxMinifyLargeImages
            // 
            this.checkBoxMinifyLargeImages.AutoSize = true;
            this.checkBoxMinifyLargeImages.Checked = true;
            this.checkBoxMinifyLargeImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMinifyLargeImages.Location = new System.Drawing.Point(6, 38);
            this.checkBoxMinifyLargeImages.Name = "checkBoxMinifyLargeImages";
            this.checkBoxMinifyLargeImages.Size = new System.Drawing.Size(121, 17);
            this.checkBoxMinifyLargeImages.TabIndex = 113;
            this.checkBoxMinifyLargeImages.Text = "Minify large images?";
            this.checkBoxMinifyLargeImages.UseVisualStyleBackColor = true;
            this.checkBoxMinifyLargeImages.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBoxComputeMetrics
            // 
            this.groupBoxComputeMetrics.Controls.Add(this.checkBoxTestDustMethods);
            this.groupBoxComputeMetrics.Controls.Add(this.checkBoxCloudyFog);
            this.groupBoxComputeMetrics.Controls.Add(this.checkBoxCloudyHeterogeneousFog);
            this.groupBoxComputeMetrics.Controls.Add(this.checkBoxHeterogeneousFog);
            this.groupBoxComputeMetrics.Controls.Add(this.checkBoxUniformFog);
            this.groupBoxComputeMetrics.Controls.Add(this.buttonComputeMetrics);
            this.groupBoxComputeMetrics.Location = new System.Drawing.Point(8, 532);
            this.groupBoxComputeMetrics.Name = "groupBoxComputeMetrics";
            this.groupBoxComputeMetrics.Size = new System.Drawing.Size(241, 118);
            this.groupBoxComputeMetrics.TabIndex = 114;
            this.groupBoxComputeMetrics.TabStop = false;
            this.groupBoxComputeMetrics.Text = "Compute metrics";
            // 
            // checkBoxUniformFog
            // 
            this.checkBoxUniformFog.AutoSize = true;
            this.checkBoxUniformFog.Checked = true;
            this.checkBoxUniformFog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUniformFog.Location = new System.Drawing.Point(6, 15);
            this.checkBoxUniformFog.Name = "checkBoxUniformFog";
            this.checkBoxUniformFog.Size = new System.Drawing.Size(80, 17);
            this.checkBoxUniformFog.TabIndex = 115;
            this.checkBoxUniformFog.Text = "Uniform fog";
            this.checkBoxUniformFog.UseVisualStyleBackColor = true;
            // 
            // checkBoxHeterogeneousFog
            // 
            this.checkBoxHeterogeneousFog.AutoSize = true;
            this.checkBoxHeterogeneousFog.Location = new System.Drawing.Point(6, 35);
            this.checkBoxHeterogeneousFog.Name = "checkBoxHeterogeneousFog";
            this.checkBoxHeterogeneousFog.Size = new System.Drawing.Size(117, 17);
            this.checkBoxHeterogeneousFog.TabIndex = 116;
            this.checkBoxHeterogeneousFog.Text = "Heterogeneous fog";
            this.checkBoxHeterogeneousFog.UseVisualStyleBackColor = true;
            // 
            // checkBoxCloudyHeterogeneousFog
            // 
            this.checkBoxCloudyHeterogeneousFog.AutoSize = true;
            this.checkBoxCloudyHeterogeneousFog.Location = new System.Drawing.Point(6, 73);
            this.checkBoxCloudyHeterogeneousFog.Name = "checkBoxCloudyHeterogeneousFog";
            this.checkBoxCloudyHeterogeneousFog.Size = new System.Drawing.Size(150, 17);
            this.checkBoxCloudyHeterogeneousFog.TabIndex = 117;
            this.checkBoxCloudyHeterogeneousFog.Text = "Cloudy heterogeneous fog";
            this.checkBoxCloudyHeterogeneousFog.UseVisualStyleBackColor = true;
            // 
            // checkBoxCloudyFog
            // 
            this.checkBoxCloudyFog.AutoSize = true;
            this.checkBoxCloudyFog.Checked = true;
            this.checkBoxCloudyFog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCloudyFog.Location = new System.Drawing.Point(6, 53);
            this.checkBoxCloudyFog.Name = "checkBoxCloudyFog";
            this.checkBoxCloudyFog.Size = new System.Drawing.Size(76, 17);
            this.checkBoxCloudyFog.TabIndex = 118;
            this.checkBoxCloudyFog.Text = "Cloudy fog";
            this.checkBoxCloudyFog.UseVisualStyleBackColor = true;
            // 
            // checkBoxTestDustMethods
            // 
            this.checkBoxTestDustMethods.AutoSize = true;
            this.checkBoxTestDustMethods.Location = new System.Drawing.Point(6, 91);
            this.checkBoxTestDustMethods.Name = "checkBoxTestDustMethods";
            this.checkBoxTestDustMethods.Size = new System.Drawing.Size(119, 17);
            this.checkBoxTestDustMethods.TabIndex = 119;
            this.checkBoxTestDustMethods.Text = "Test dust methods?";
            this.checkBoxTestDustMethods.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxShowOptionalWindows);
            this.groupBox1.Controls.Add(this.checkBoxMinifyLargeImages);
            this.groupBox1.Location = new System.Drawing.Point(8, 448);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(241, 77);
            this.groupBox1.TabIndex = 115;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Base application settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxUpdateStats);
            this.groupBox2.Controls.Add(this.checkBoxRunAllMethodsFog);
            this.groupBox2.Controls.Add(this.checkBoxRunAllMethodsDust);
            this.groupBox2.Controls.Add(this.checkBoxRunAllMethodsSaveAllImages);
            this.groupBox2.Controls.Add(this.buttonRunAllMethods);
            this.groupBox2.Location = new System.Drawing.Point(8, 656);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(241, 113);
            this.groupBox2.TabIndex = 116;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Run all methods";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1235, 782);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxComputeMetrics);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.label37);
            this.Controls.Add(this.textBoxShadowEdgeMedian_KernelRadius);
            this.Controls.Add(this.textBoxShadowEdgeMedian_DilationKernelSIze);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.textBoxShadowEdgeGaussian_KernelRadius);
            this.Controls.Add(this.textBoxShadowEdgeGaussian_DilationKernelSize);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.textBoxShadowEdgeInpaint_KernelRadius);
            this.Controls.Add(this.textBoxShadowEdgeInpaint_DilationKernelSize);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.buttonTestVideo);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.textBox_RatioConstancyMethod_Imax);
            this.Controls.Add(this.textBox_RatioConstancyMethod_Imin);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.textBoxSSIMMetric);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.textBoxMEthodExecTime);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.textBoxShannonEntropy);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.textBoxRMSMetric);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.textBoxShannonEntropyDiff);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.textBoxRMSMetricDiff);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.textBoxAdMetric);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.textBoxPsnrMEtric);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.textBoxScMetric);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.textBoxNaeMetric);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.textBoxMseMetric);
            this.Controls.Add(this.buttonRemoveFogUsingMultiCoreDSPMethod);
            this.Controls.Add(this.buttonRemoveFogUsingCustomMethodWithDepthEstimation);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBoxFvmMetric);
            this.Controls.Add(this.buttonRemoveFogUsingPhysicsBasedMethod);
            this.Controls.Add(this.buttonRemoveFogUsingLocalExtremaMethod);
            this.Controls.Add(this.buttonRemoveFogUsingCustomMethod);
            this.Controls.Add(this.buttonRemoveFogUsingDCPAndDFT);
            this.Controls.Add(this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads);
            this.Controls.Add(this.buttonApplyAGC);
            this.Controls.Add(this.buttonRemoveFogUsingIdcpWithClahe);
            this.Controls.Add(this.buttonRemoveFogUsingMedianChannelPrior);
            this.Controls.Add(this.buttonRobbyTanFogRemovalMethod);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox_RatioConstancyMethod_sigma);
            this.Controls.Add(this.textBox_RatioConstancyMethod_kernel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttonDetectUsingModifiedRatioOfHueOverIntensityMethod);
            this.Controls.Add(this.textBoxTestMsSigma);
            this.Controls.Add(this.textBoxTestMsKernel);
            this.Controls.Add(this.buttonMSTest);
            this.Controls.Add(this.buttonTestEmguCVCudaMeanShift);
            this.Controls.Add(this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod);
            this.Controls.Add(this.buttonShadowRemovalLabMethod2);
            this.Controls.Add(this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonRemoveFogUsingDarkChannelMethod);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonSmoothShadowEdgesUsingMedianFilter);
            this.Controls.Add(this.buttonSmoothShadowEdgesUsingGaussianFilter);
            this.Controls.Add(this.buttonImpaintShadowEdges);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonShadowRemovalBasicLightModelMethod);
            this.Controls.Add(this.buttonShadowRemovalCombinedMethod);
            this.Controls.Add(this.buttonShadowRemovalLabMethod);
            this.Controls.Add(this.buttonShadowRemovalConstantMethod);
            this.Controls.Add(this.buttonShadowRemovalAditiveMethod);
            this.Controls.Add(this.textBoxShadowDetectionLMSThreshold);
            this.Controls.Add(this.buttonShadowDetectionMS);
            this.Controls.Add(this.textBoxShadowDetectionLabThreshold);
            this.Controls.Add(this.buttonShadowDetectionLab);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBoxComputeMetrics.ResumeLayout(false);
            this.groupBoxComputeMetrics.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Button buttonShadowDetectionLab;
        private System.Windows.Forms.TextBox textBoxShadowDetectionLabThreshold;
        private System.Windows.Forms.Button buttonShadowDetectionMS;
        private System.Windows.Forms.TextBox textBoxShadowDetectionLMSThreshold;
        private System.Windows.Forms.Button buttonShadowRemovalAditiveMethod;
        private System.Windows.Forms.Button buttonShadowRemovalConstantMethod;
        private System.Windows.Forms.Button buttonShadowRemovalLabMethod;
        private System.Windows.Forms.Button buttonShadowRemovalCombinedMethod;
        private System.Windows.Forms.Button buttonShadowRemovalBasicLightModelMethod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonImpaintShadowEdges;
        private System.Windows.Forms.Button buttonSmoothShadowEdgesUsingGaussianFilter;
        private System.Windows.Forms.Button buttonSmoothShadowEdgesUsingMedianFilter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonRemoveFogUsingDarkChannelMethod;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxShowOptionalWindows;
        private System.Windows.Forms.Button buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
        private System.Windows.Forms.Button buttonShadowRemovalLabMethod2;
        private System.Windows.Forms.Button buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod;
        private System.Windows.Forms.Button buttonTestEmguCVCudaMeanShift;
        private System.Windows.Forms.Button buttonMSTest;
        private System.Windows.Forms.TextBox textBoxTestMsKernel;
        private System.Windows.Forms.TextBox textBoxTestMsSigma;
        private System.Windows.Forms.Button buttonDetectUsingModifiedRatioOfHueOverIntensityMethod;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_RatioConstancyMethod_sigma;
        private System.Windows.Forms.TextBox textBox_RatioConstancyMethod_kernel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button buttonRobbyTanFogRemovalMethod;
        private System.Windows.Forms.Button buttonRemoveFogUsingMedianChannelPrior;
        private System.Windows.Forms.Button buttonRemoveFogUsingIdcpWithClahe;
        private System.Windows.Forms.Button buttonApplyAGC;
        private System.Windows.Forms.Button buttonEnhaceVisibilityUsingRobbyTanMethodForRoads;
        private System.Windows.Forms.Button buttonRemoveFogUsingDCPAndDFT;
        private System.Windows.Forms.Button buttonRemoveFogUsingCustomMethod;
        private System.Windows.Forms.Button buttonRemoveFogUsingLocalExtremaMethod;
        private System.Windows.Forms.Button buttonRemoveFogUsingPhysicsBasedMethod;
        private System.Windows.Forms.TextBox textBoxFvmMetric;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button buttonRemoveFogUsingCustomMethodWithDepthEstimation;
        private System.Windows.Forms.Button buttonRemoveFogUsingMultiCoreDSPMethod;
        private System.Windows.Forms.ToolStripMenuItem exportMetricsToCSVToolStripMenuItem;
        private System.Windows.Forms.Button buttonRunAllMethods;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxMseMetric;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxNaeMetric;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxScMetric;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBoxPsnrMEtric;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBoxAdMetric;
        private System.Windows.Forms.ToolStripMenuItem resetMethodsStatisticsToolStripMenuItem;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBoxRMSMetricDiff;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBoxShannonEntropyDiff;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBoxRMSMetric;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox textBoxShannonEntropy;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox textBoxMEthodExecTime;
      private System.Windows.Forms.Label label24;
      private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox checkBoxUpdateStats;
        private System.Windows.Forms.CheckBox checkBoxRunAllMethodsFog;
        private System.Windows.Forms.CheckBox checkBoxRunAllMethodsDust;
        private System.Windows.Forms.CheckBox checkBoxRunAllMethodsSaveAllImages;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox textBoxSSIMMetric;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox textBox_RatioConstancyMethod_Imax;
        private System.Windows.Forms.TextBox textBox_RatioConstancyMethod_Imin;
        private System.Windows.Forms.Button buttonComputeMetrics;
        private System.Windows.Forms.Button buttonTestVideo;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.ToolStripMenuItem saveDetectionResultToolStripMenuItem;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox textBoxShadowEdgeInpaint_KernelRadius;
        private System.Windows.Forms.TextBox textBoxShadowEdgeInpaint_DilationKernelSize;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.TextBox textBoxShadowEdgeGaussian_KernelRadius;
        private System.Windows.Forms.TextBox textBoxShadowEdgeGaussian_DilationKernelSize;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox textBoxShadowEdgeMedian_KernelRadius;
        private System.Windows.Forms.TextBox textBoxShadowEdgeMedian_DilationKernelSIze;
        private System.Windows.Forms.CheckBox checkBoxMinifyLargeImages;
        private System.Windows.Forms.GroupBox groupBoxComputeMetrics;
        private System.Windows.Forms.CheckBox checkBoxCloudyFog;
        private System.Windows.Forms.CheckBox checkBoxCloudyHeterogeneousFog;
        private System.Windows.Forms.CheckBox checkBoxHeterogeneousFog;
        private System.Windows.Forms.CheckBox checkBoxUniformFog;
        private System.Windows.Forms.CheckBox checkBoxTestDustMethods;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

