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
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.buttonRobbyTanFogRemovalMethod = new System.Windows.Forms.Button();
            this.buttonRemoveFogUsingMedianChannelPrior = new System.Windows.Forms.Button();
            this.buttonRemoveFogUsingIdcpWithClahe = new System.Windows.Forms.Button();
            this.buttonApplyAGC = new System.Windows.Forms.Button();
            this.buttonTestFilters = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(6, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(403, 391);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(415, 27);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(403, 391);
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Location = new System.Drawing.Point(824, 27);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(403, 391);
            this.pictureBox3.TabIndex = 7;
            this.pictureBox3.TabStop = false;
            // 
            // buttonShadowDetectionLab
            // 
            this.buttonShadowDetectionLab.BackColor = System.Drawing.SystemColors.Control;
            this.buttonShadowDetectionLab.Location = new System.Drawing.Point(237, 456);
            this.buttonShadowDetectionLab.Name = "buttonShadowDetectionLab";
            this.buttonShadowDetectionLab.Size = new System.Drawing.Size(75, 34);
            this.buttonShadowDetectionLab.TabIndex = 8;
            this.buttonShadowDetectionLab.Text = "LAB";
            this.buttonShadowDetectionLab.UseVisualStyleBackColor = false;
            this.buttonShadowDetectionLab.Click += new System.EventHandler(this.buttonShadowDetectionLab_Click);
            // 
            // textBoxShadowDetectionLabThreshold
            // 
            this.textBoxShadowDetectionLabThreshold.Location = new System.Drawing.Point(318, 469);
            this.textBoxShadowDetectionLabThreshold.Name = "textBoxShadowDetectionLabThreshold";
            this.textBoxShadowDetectionLabThreshold.Size = new System.Drawing.Size(50, 20);
            this.textBoxShadowDetectionLabThreshold.TabIndex = 9;
            // 
            // buttonShadowDetectionMS
            // 
            this.buttonShadowDetectionMS.BackColor = System.Drawing.SystemColors.Control;
            this.buttonShadowDetectionMS.Location = new System.Drawing.Point(237, 493);
            this.buttonShadowDetectionMS.Name = "buttonShadowDetectionMS";
            this.buttonShadowDetectionMS.Size = new System.Drawing.Size(75, 34);
            this.buttonShadowDetectionMS.TabIndex = 10;
            this.buttonShadowDetectionMS.Text = "MS";
            this.buttonShadowDetectionMS.UseVisualStyleBackColor = false;
            this.buttonShadowDetectionMS.Click += new System.EventHandler(this.buttonShadowDetectionMS_Click);
            // 
            // textBoxShadowDetectionLMSThreshold
            // 
            this.textBoxShadowDetectionLMSThreshold.Location = new System.Drawing.Point(318, 506);
            this.textBoxShadowDetectionLMSThreshold.Name = "textBoxShadowDetectionLMSThreshold";
            this.textBoxShadowDetectionLMSThreshold.Size = new System.Drawing.Size(50, 20);
            this.textBoxShadowDetectionLMSThreshold.TabIndex = 11;
            // 
            // buttonShadowRemovalAditiveMethod
            // 
            this.buttonShadowRemovalAditiveMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonShadowRemovalAditiveMethod.Location = new System.Drawing.Point(391, 448);
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
            this.buttonShadowRemovalConstantMethod.Location = new System.Drawing.Point(391, 577);
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
            this.buttonShadowRemovalLabMethod.Location = new System.Drawing.Point(391, 544);
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
            this.buttonShadowRemovalCombinedMethod.Location = new System.Drawing.Point(391, 511);
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
            this.buttonShadowRemovalBasicLightModelMethod.Location = new System.Drawing.Point(391, 481);
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
            this.label1.Location = new System.Drawing.Point(234, 428);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 15);
            this.label1.TabIndex = 20;
            this.label1.Text = "Shadow detection";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(388, 428);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 15);
            this.label2.TabIndex = 21;
            this.label2.Text = "Shadow removal";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(526, 429);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 15);
            this.label3.TabIndex = 22;
            this.label3.Text = "Shadow edges processing";
            // 
            // buttonImpaintShadowEdges
            // 
            this.buttonImpaintShadowEdges.BackColor = System.Drawing.SystemColors.Control;
            this.buttonImpaintShadowEdges.Location = new System.Drawing.Point(529, 451);
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
            this.buttonSmoothShadowEdgesUsingGaussianFilter.Location = new System.Drawing.Point(529, 488);
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
            this.buttonSmoothShadowEdgesUsingMedianFilter.Location = new System.Drawing.Point(529, 530);
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
            this.label4.Location = new System.Drawing.Point(707, 430);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 15);
            this.label4.TabIndex = 26;
            this.label4.Text = "Fog removal";
            // 
            // buttonRemoveFogUsingDarkChannelMethod
            // 
            this.buttonRemoveFogUsingDarkChannelMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingDarkChannelMethod.Location = new System.Drawing.Point(710, 451);
            this.buttonRemoveFogUsingDarkChannelMethod.Name = "buttonRemoveFogUsingDarkChannelMethod";
            this.buttonRemoveFogUsingDarkChannelMethod.Size = new System.Drawing.Size(121, 32);
            this.buttonRemoveFogUsingDarkChannelMethod.TabIndex = 27;
            this.buttonRemoveFogUsingDarkChannelMethod.Text = "Dark channel";
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
            this.checkBoxShowOptionalWindows.Location = new System.Drawing.Point(6, 430);
            this.checkBoxShowOptionalWindows.Name = "checkBoxShowOptionalWindows";
            this.checkBoxShowOptionalWindows.Size = new System.Drawing.Size(148, 17);
            this.checkBoxShowOptionalWindows.TabIndex = 29;
            this.checkBoxShowOptionalWindows.Text = "Show Optional Windows?";
            this.checkBoxShowOptionalWindows.UseVisualStyleBackColor = true;
            // 
            // buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod
            // 
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.Location = new System.Drawing.Point(844, 449);
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.Name = "buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMet" +
    "hod";
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.Size = new System.Drawing.Size(157, 34);
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.TabIndex = 31;
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.Text = "Dust (Fuzzy Operators)";
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.UseVisualStyleBackColor = false;
            this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod.Click += new System.EventHandler(this.buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(841, 429);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 15);
            this.label5.TabIndex = 30;
            this.label5.Text = "Dust removal";
            // 
            // buttonShadowRemovalLabMethod2
            // 
            this.buttonShadowRemovalLabMethod2.Location = new System.Drawing.Point(160, 733);
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
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod.Location = new System.Drawing.Point(844, 489);
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod.Name = "buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod";
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod.Size = new System.Drawing.Size(157, 38);
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod.TabIndex = 33;
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod.Text = "Dust (ratio constansy method)";
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod.UseVisualStyleBackColor = false;
            this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod.Click += new System.EventHandler(this.buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod_Click);
            // 
            // buttonTestEmguCVCudaMeanShift
            // 
            this.buttonTestEmguCVCudaMeanShift.Location = new System.Drawing.Point(13, 733);
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
            this.buttonMSTest.Location = new System.Drawing.Point(13, 705);
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
            this.textBoxTestMsKernel.Location = new System.Drawing.Point(94, 707);
            this.textBoxTestMsKernel.Name = "textBoxTestMsKernel";
            this.textBoxTestMsKernel.Size = new System.Drawing.Size(28, 20);
            this.textBoxTestMsKernel.TabIndex = 36;
            this.textBoxTestMsKernel.Visible = false;
            // 
            // textBoxTestMsSigma
            // 
            this.textBoxTestMsSigma.Location = new System.Drawing.Point(128, 708);
            this.textBoxTestMsSigma.Name = "textBoxTestMsSigma";
            this.textBoxTestMsSigma.Size = new System.Drawing.Size(28, 20);
            this.textBoxTestMsSigma.TabIndex = 37;
            this.textBoxTestMsSigma.Visible = false;
            // 
            // buttonDetectUsingModifiedRatioOfHueOverIntensityMethod
            // 
            this.buttonDetectUsingModifiedRatioOfHueOverIntensityMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonDetectUsingModifiedRatioOfHueOverIntensityMethod.Location = new System.Drawing.Point(237, 530);
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
            this.label6.Location = new System.Drawing.Point(91, 691);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 39;
            this.label6.Text = "kernel";
            this.label6.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(125, 692);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 40;
            this.label7.Text = "sigma";
            this.label7.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1040, 491);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 44;
            this.label8.Text = "sigma";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1006, 491);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 43;
            this.label9.Text = "kernel";
            // 
            // textBox_RatioConstancyMethod_sigma
            // 
            this.textBox_RatioConstancyMethod_sigma.Location = new System.Drawing.Point(1043, 507);
            this.textBox_RatioConstancyMethod_sigma.Name = "textBox_RatioConstancyMethod_sigma";
            this.textBox_RatioConstancyMethod_sigma.Size = new System.Drawing.Size(28, 20);
            this.textBox_RatioConstancyMethod_sigma.TabIndex = 42;
            // 
            // textBox_RatioConstancyMethod_kernel
            // 
            this.textBox_RatioConstancyMethod_kernel.Location = new System.Drawing.Point(1009, 507);
            this.textBox_RatioConstancyMethod_kernel.Name = "textBox_RatioConstancyMethod_kernel";
            this.textBox_RatioConstancyMethod_kernel.Size = new System.Drawing.Size(28, 20);
            this.textBox_RatioConstancyMethod_kernel.TabIndex = 41;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1004, 448);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 46;
            this.label10.Text = "dzeta (ζ)";
            // 
            // textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta
            // 
            this.textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta.Location = new System.Drawing.Point(1007, 462);
            this.textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta.Name = "textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta";
            this.textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta.Size = new System.Drawing.Size(44, 20);
            this.textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta.TabIndex = 45;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(318, 456);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 13);
            this.label11.TabIndex = 47;
            this.label11.Text = "threshold";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(318, 492);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(50, 13);
            this.label12.TabIndex = 48;
            this.label12.Text = "threshold";
            // 
            // buttonRobbyTanFogRemovalMethod
            // 
            this.buttonRobbyTanFogRemovalMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRobbyTanFogRemovalMethod.Location = new System.Drawing.Point(248, 721);
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
            this.buttonRemoveFogUsingMedianChannelPrior.Location = new System.Drawing.Point(710, 489);
            this.buttonRemoveFogUsingMedianChannelPrior.Name = "buttonRemoveFogUsingMedianChannelPrior";
            this.buttonRemoveFogUsingMedianChannelPrior.Size = new System.Drawing.Size(121, 32);
            this.buttonRemoveFogUsingMedianChannelPrior.TabIndex = 50;
            this.buttonRemoveFogUsingMedianChannelPrior.Text = "MedianChannelPrior";
            this.buttonRemoveFogUsingMedianChannelPrior.UseVisualStyleBackColor = false;
            this.buttonRemoveFogUsingMedianChannelPrior.Click += new System.EventHandler(this.buttonRemoveFogUsingMedianChannelPrior_Click);
            // 
            // buttonRemoveFogUsingIdcpWithClahe
            // 
            this.buttonRemoveFogUsingIdcpWithClahe.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingIdcpWithClahe.Location = new System.Drawing.Point(710, 527);
            this.buttonRemoveFogUsingIdcpWithClahe.Name = "buttonRemoveFogUsingIdcpWithClahe";
            this.buttonRemoveFogUsingIdcpWithClahe.Size = new System.Drawing.Size(121, 32);
            this.buttonRemoveFogUsingIdcpWithClahe.TabIndex = 51;
            this.buttonRemoveFogUsingIdcpWithClahe.Text = "IDCP with CLAHE";
            this.buttonRemoveFogUsingIdcpWithClahe.UseVisualStyleBackColor = false;
            this.buttonRemoveFogUsingIdcpWithClahe.Click += new System.EventHandler(this.buttonRemoveFogUsingIdcpWithClahe_Click);
            // 
            // buttonApplyAGC
            // 
            this.buttonApplyAGC.BackColor = System.Drawing.SystemColors.Control;
            this.buttonApplyAGC.Location = new System.Drawing.Point(1096, 450);
            this.buttonApplyAGC.Name = "buttonApplyAGC";
            this.buttonApplyAGC.Size = new System.Drawing.Size(97, 34);
            this.buttonApplyAGC.TabIndex = 52;
            this.buttonApplyAGC.Text = "AGC";
            this.buttonApplyAGC.UseVisualStyleBackColor = false;
            this.buttonApplyAGC.Click += new System.EventHandler(this.buttonApplyAGC_Click);
            // 
            // buttonTestFilters
            // 
            this.buttonTestFilters.Location = new System.Drawing.Point(13, 676);
            this.buttonTestFilters.Name = "buttonTestFilters";
            this.buttonTestFilters.Size = new System.Drawing.Size(75, 23);
            this.buttonTestFilters.TabIndex = 53;
            this.buttonTestFilters.Text = "Test filters";
            this.buttonTestFilters.UseVisualStyleBackColor = true;
            this.buttonTestFilters.Visible = false;
            this.buttonTestFilters.Click += new System.EventHandler(this.buttonTestFilters_Click);
            // 
            // buttonEnhaceVisibilityUsingRobbyTanMethodForRoads
            // 
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.BackColor = System.Drawing.SystemColors.Control;
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.Location = new System.Drawing.Point(844, 533);
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.Name = "buttonEnhaceVisibilityUsingRobbyTanMethodForRoads";
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.Size = new System.Drawing.Size(157, 37);
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.TabIndex = 54;
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.Text = "Robby Tan VE For Roads";
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.UseVisualStyleBackColor = false;
            this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads.Click += new System.EventHandler(this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads_Click);
            // 
            // buttonRemoveFogUsingDCPAndDFT
            // 
            this.buttonRemoveFogUsingDCPAndDFT.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingDCPAndDFT.Location = new System.Drawing.Point(710, 565);
            this.buttonRemoveFogUsingDCPAndDFT.Name = "buttonRemoveFogUsingDCPAndDFT";
            this.buttonRemoveFogUsingDCPAndDFT.Size = new System.Drawing.Size(121, 32);
            this.buttonRemoveFogUsingDCPAndDFT.TabIndex = 55;
            this.buttonRemoveFogUsingDCPAndDFT.Text = "DCP with DFT";
            this.buttonRemoveFogUsingDCPAndDFT.UseVisualStyleBackColor = false;
            this.buttonRemoveFogUsingDCPAndDFT.Click += new System.EventHandler(this.buttonRemoveFogUsingDCPAndDFT_Click);
            // 
            // buttonRemoveFogUsingCustomMethod
            // 
            this.buttonRemoveFogUsingCustomMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingCustomMethod.Location = new System.Drawing.Point(710, 641);
            this.buttonRemoveFogUsingCustomMethod.Name = "buttonRemoveFogUsingCustomMethod";
            this.buttonRemoveFogUsingCustomMethod.Size = new System.Drawing.Size(121, 32);
            this.buttonRemoveFogUsingCustomMethod.TabIndex = 56;
            this.buttonRemoveFogUsingCustomMethod.Text = "Custom";
            this.buttonRemoveFogUsingCustomMethod.UseVisualStyleBackColor = false;
            this.buttonRemoveFogUsingCustomMethod.Click += new System.EventHandler(this.buttonRemoveFogUsingCustomMethod_Click);
            // 
            // buttonRemoveFogUsingLocalExtremaMethod
            // 
            this.buttonRemoveFogUsingLocalExtremaMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingLocalExtremaMethod.Location = new System.Drawing.Point(502, 721);
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
            this.buttonRemoveFogUsingPhysicsBasedMethod.Location = new System.Drawing.Point(375, 721);
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
            this.textBoxFvmMetric.Location = new System.Drawing.Point(901, 620);
            this.textBoxFvmMetric.Name = "textBoxFvmMetric";
            this.textBoxFvmMetric.Size = new System.Drawing.Size(60, 20);
            this.textBoxFvmMetric.TabIndex = 59;
            this.textBoxFvmMetric.Visible = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(848, 623);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 13);
            this.label13.TabIndex = 60;
            this.label13.Text = "FVM";
            this.label13.Visible = false;
            // 
            // buttonRemoveFogUsingCustomMethodWithDepthEstimation
            // 
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.Location = new System.Drawing.Point(710, 679);
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.Name = "buttonRemoveFogUsingCustomMethodWithDepthEstimation";
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.Size = new System.Drawing.Size(121, 32);
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.TabIndex = 61;
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.Text = "Custom With Depth Est";
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.UseVisualStyleBackColor = false;
            this.buttonRemoveFogUsingCustomMethodWithDepthEstimation.Click += new System.EventHandler(this.buttonRemoveFogUsingCustomMethodWithDepthEstimation_Click);
            // 
            // buttonRemoveFogUsingMultiCoreDSPMethod
            // 
            this.buttonRemoveFogUsingMultiCoreDSPMethod.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveFogUsingMultiCoreDSPMethod.Location = new System.Drawing.Point(710, 603);
            this.buttonRemoveFogUsingMultiCoreDSPMethod.Name = "buttonRemoveFogUsingMultiCoreDSPMethod";
            this.buttonRemoveFogUsingMultiCoreDSPMethod.Size = new System.Drawing.Size(121, 32);
            this.buttonRemoveFogUsingMultiCoreDSPMethod.TabIndex = 62;
            this.buttonRemoveFogUsingMultiCoreDSPMethod.Text = "Multi-core DSP";
            this.buttonRemoveFogUsingMultiCoreDSPMethod.UseVisualStyleBackColor = false;
            this.buttonRemoveFogUsingMultiCoreDSPMethod.Click += new System.EventHandler(this.buttonRemoveFogUsingMultiCoreDSPMethod_Click);
            // 
            // buttonRunAllMethods
            // 
            this.buttonRunAllMethods.Location = new System.Drawing.Point(6, 456);
            this.buttonRunAllMethods.Name = "buttonRunAllMethods";
            this.buttonRunAllMethods.Size = new System.Drawing.Size(114, 27);
            this.buttonRunAllMethods.TabIndex = 63;
            this.buttonRunAllMethods.Text = "Run All Methods";
            this.buttonRunAllMethods.UseVisualStyleBackColor = true;
            this.buttonRunAllMethods.Click += new System.EventHandler(this.buttonRunAllMethods_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(848, 647);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(30, 13);
            this.label14.TabIndex = 65;
            this.label14.Text = "MSE";
            this.label14.Visible = false;
            // 
            // textBoxMseMetric
            // 
            this.textBoxMseMetric.Location = new System.Drawing.Point(901, 644);
            this.textBoxMseMetric.Name = "textBoxMseMetric";
            this.textBoxMseMetric.Size = new System.Drawing.Size(60, 20);
            this.textBoxMseMetric.TabIndex = 64;
            this.textBoxMseMetric.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(848, 671);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(29, 13);
            this.label15.TabIndex = 67;
            this.label15.Text = "NAE";
            this.label15.Visible = false;
            // 
            // textBoxNaeMetric
            // 
            this.textBoxNaeMetric.Location = new System.Drawing.Point(901, 668);
            this.textBoxNaeMetric.Name = "textBoxNaeMetric";
            this.textBoxNaeMetric.Size = new System.Drawing.Size(60, 20);
            this.textBoxNaeMetric.TabIndex = 66;
            this.textBoxNaeMetric.Visible = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(983, 619);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(21, 13);
            this.label16.TabIndex = 69;
            this.label16.Text = "SC";
            this.label16.Visible = false;
            // 
            // textBoxScMetric
            // 
            this.textBoxScMetric.Location = new System.Drawing.Point(1035, 615);
            this.textBoxScMetric.Name = "textBoxScMetric";
            this.textBoxScMetric.Size = new System.Drawing.Size(60, 20);
            this.textBoxScMetric.TabIndex = 68;
            this.textBoxScMetric.Visible = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(983, 645);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(37, 13);
            this.label17.TabIndex = 71;
            this.label17.Text = "PSNR";
            this.label17.Visible = false;
            // 
            // textBoxPsnrMEtric
            // 
            this.textBoxPsnrMEtric.Location = new System.Drawing.Point(1035, 642);
            this.textBoxPsnrMEtric.Name = "textBoxPsnrMEtric";
            this.textBoxPsnrMEtric.Size = new System.Drawing.Size(60, 20);
            this.textBoxPsnrMEtric.TabIndex = 70;
            this.textBoxPsnrMEtric.Visible = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(983, 671);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(22, 13);
            this.label18.TabIndex = 73;
            this.label18.Text = "AD";
            this.label18.Visible = false;
            // 
            // textBoxAdMetric
            // 
            this.textBoxAdMetric.Location = new System.Drawing.Point(1035, 668);
            this.textBoxAdMetric.Name = "textBoxAdMetric";
            this.textBoxAdMetric.Size = new System.Drawing.Size(60, 20);
            this.textBoxAdMetric.TabIndex = 72;
            this.textBoxAdMetric.Visible = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(982, 696);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(47, 13);
            this.label19.TabIndex = 75;
            this.label19.Text = "RMSDiff";
            this.label19.Visible = false;
            // 
            // textBoxRMSMetricDiff
            // 
            this.textBoxRMSMetricDiff.Location = new System.Drawing.Point(1035, 693);
            this.textBoxRMSMetricDiff.Name = "textBoxRMSMetricDiff";
            this.textBoxRMSMetricDiff.Size = new System.Drawing.Size(60, 20);
            this.textBoxRMSMetricDiff.TabIndex = 74;
            this.textBoxRMSMetricDiff.Visible = false;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(982, 722);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(50, 39);
            this.label20.TabIndex = 77;
            this.label20.Text = "Shannon\r\nEntropy\r\nDiff";
            this.label20.Visible = false;
            // 
            // textBoxShannonEntropyDiff
            // 
            this.textBoxShannonEntropyDiff.Location = new System.Drawing.Point(1035, 719);
            this.textBoxShannonEntropyDiff.Name = "textBoxShannonEntropyDiff";
            this.textBoxShannonEntropyDiff.Size = new System.Drawing.Size(60, 20);
            this.textBoxShannonEntropyDiff.TabIndex = 76;
            this.textBoxShannonEntropyDiff.Visible = false;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(848, 699);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(31, 13);
            this.label21.TabIndex = 79;
            this.label21.Text = "RMS";
            this.label21.Visible = false;
            // 
            // textBoxRMSMetric
            // 
            this.textBoxRMSMetric.Location = new System.Drawing.Point(901, 696);
            this.textBoxRMSMetric.Name = "textBoxRMSMetric";
            this.textBoxRMSMetric.Size = new System.Drawing.Size(60, 20);
            this.textBoxRMSMetric.TabIndex = 78;
            this.textBoxRMSMetric.Visible = false;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(848, 725);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(50, 26);
            this.label22.TabIndex = 81;
            this.label22.Text = "Shannon\r\nEntropy\r";
            this.label22.Visible = false;
            // 
            // textBoxShannonEntropy
            // 
            this.textBoxShannonEntropy.Location = new System.Drawing.Point(901, 722);
            this.textBoxShannonEntropy.Name = "textBoxShannonEntropy";
            this.textBoxShannonEntropy.Size = new System.Drawing.Size(60, 20);
            this.textBoxShannonEntropy.TabIndex = 80;
            this.textBoxShannonEntropy.Visible = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(847, 597);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(30, 13);
            this.label23.TabIndex = 83;
            this.label23.Text = "Time";
            this.label23.Visible = false;
            // 
            // textBoxMEthodExecTime
            // 
            this.textBoxMEthodExecTime.Location = new System.Drawing.Point(901, 594);
            this.textBoxMEthodExecTime.Name = "textBoxMEthodExecTime";
            this.textBoxMEthodExecTime.Size = new System.Drawing.Size(60, 20);
            this.textBoxMEthodExecTime.TabIndex = 82;
            this.textBoxMEthodExecTime.Visible = false;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label24.Location = new System.Drawing.Point(1093, 432);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(42, 15);
            this.label24.TabIndex = 84;
            this.label24.Text = "Other";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label25.Location = new System.Drawing.Point(847, 576);
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
            this.checkBoxUpdateStats.Location = new System.Drawing.Point(12, 487);
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
            this.checkBoxRunAllMethodsFog.Location = new System.Drawing.Point(12, 532);
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
            this.checkBoxRunAllMethodsDust.Location = new System.Drawing.Point(12, 553);
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
            this.checkBoxRunAllMethodsSaveAllImages.Location = new System.Drawing.Point(12, 509);
            this.checkBoxRunAllMethodsSaveAllImages.Name = "checkBoxRunAllMethodsSaveAllImages";
            this.checkBoxRunAllMethodsSaveAllImages.Size = new System.Drawing.Size(106, 17);
            this.checkBoxRunAllMethodsSaveAllImages.TabIndex = 89;
            this.checkBoxRunAllMethodsSaveAllImages.Text = "Save all images?";
            this.checkBoxRunAllMethodsSaveAllImages.UseVisualStyleBackColor = true;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(983, 594);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(33, 13);
            this.label26.TabIndex = 91;
            this.label26.Text = "SSIM";
            this.label26.Visible = false;
            // 
            // textBoxSSIMMetric
            // 
            this.textBoxSSIMMetric.Location = new System.Drawing.Point(1035, 590);
            this.textBoxSSIMMetric.Name = "textBoxSSIMMetric";
            this.textBoxSSIMMetric.Size = new System.Drawing.Size(60, 20);
            this.textBoxSSIMMetric.TabIndex = 90;
            this.textBoxSSIMMetric.Visible = false;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(1108, 491);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(29, 13);
            this.label27.TabIndex = 95;
            this.label27.Text = "Imax";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(1074, 491);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(26, 13);
            this.label28.TabIndex = 94;
            this.label28.Text = "Imin";
            // 
            // textBox_RatioConstancyMethod_Imax
            // 
            this.textBox_RatioConstancyMethod_Imax.Location = new System.Drawing.Point(1111, 507);
            this.textBox_RatioConstancyMethod_Imax.Name = "textBox_RatioConstancyMethod_Imax";
            this.textBox_RatioConstancyMethod_Imax.Size = new System.Drawing.Size(28, 20);
            this.textBox_RatioConstancyMethod_Imax.TabIndex = 93;
            // 
            // textBox_RatioConstancyMethod_Imin
            // 
            this.textBox_RatioConstancyMethod_Imin.Location = new System.Drawing.Point(1077, 507);
            this.textBox_RatioConstancyMethod_Imin.Name = "textBox_RatioConstancyMethod_Imin";
            this.textBox_RatioConstancyMethod_Imin.Size = new System.Drawing.Size(28, 20);
            this.textBox_RatioConstancyMethod_Imin.TabIndex = 92;
            // 
            // buttonComputeMetrics
            // 
            this.buttonComputeMetrics.Location = new System.Drawing.Point(6, 571);
            this.buttonComputeMetrics.Name = "buttonComputeMetrics";
            this.buttonComputeMetrics.Size = new System.Drawing.Size(114, 27);
            this.buttonComputeMetrics.TabIndex = 96;
            this.buttonComputeMetrics.Text = "Compute metrics";
            this.buttonComputeMetrics.UseVisualStyleBackColor = true;
            this.buttonComputeMetrics.Click += new System.EventHandler(this.buttonComputeMetrics_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1235, 768);
            this.Controls.Add(this.buttonComputeMetrics);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.textBox_RatioConstancyMethod_Imax);
            this.Controls.Add(this.textBox_RatioConstancyMethod_Imin);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.textBoxSSIMMetric);
            this.Controls.Add(this.checkBoxRunAllMethodsSaveAllImages);
            this.Controls.Add(this.checkBoxRunAllMethodsDust);
            this.Controls.Add(this.checkBoxRunAllMethodsFog);
            this.Controls.Add(this.checkBoxUpdateStats);
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
            this.Controls.Add(this.buttonRunAllMethods);
            this.Controls.Add(this.buttonRemoveFogUsingMultiCoreDSPMethod);
            this.Controls.Add(this.buttonRemoveFogUsingCustomMethodWithDepthEstimation);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBoxFvmMetric);
            this.Controls.Add(this.buttonRemoveFogUsingPhysicsBasedMethod);
            this.Controls.Add(this.buttonRemoveFogUsingLocalExtremaMethod);
            this.Controls.Add(this.buttonRemoveFogUsingCustomMethod);
            this.Controls.Add(this.buttonRemoveFogUsingDCPAndDFT);
            this.Controls.Add(this.buttonEnhaceVisibilityUsingRobbyTanMethodForRoads);
            this.Controls.Add(this.buttonTestFilters);
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
            this.Controls.Add(this.checkBoxShowOptionalWindows);
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
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button buttonRobbyTanFogRemovalMethod;
        private System.Windows.Forms.Button buttonRemoveFogUsingMedianChannelPrior;
        private System.Windows.Forms.Button buttonRemoveFogUsingIdcpWithClahe;
        private System.Windows.Forms.Button buttonApplyAGC;
        private System.Windows.Forms.Button buttonTestFilters;
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
    }
}

