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
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.buttonSaveInFile = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.buttonShadowDetectionLab = new System.Windows.Forms.Button();
            this.textBoxShadowDetectionLabThreshold = new System.Windows.Forms.TextBox();
            this.buttonShadowDetectionMS = new System.Windows.Forms.Button();
            this.textBoxShadowDetectionLMSThreshold = new System.Windows.Forms.TextBox();
            this.checkBoxShadowDetectionLMSShowWindows = new System.Windows.Forms.CheckBox();
            this.checkBoxShadowDetectionLabShowWindows = new System.Windows.Forms.CheckBox();
            this.buttonCloseAllWindows = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 417);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(6, 53);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(403, 358);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.BackColor = System.Drawing.Color.LightBlue;
            this.buttonOpenFile.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.buttonOpenFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOpenFile.Font = new System.Drawing.Font("Georgia", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOpenFile.ForeColor = System.Drawing.Color.Indigo;
            this.buttonOpenFile.Location = new System.Drawing.Point(6, 12);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(104, 35);
            this.buttonOpenFile.TabIndex = 2;
            this.buttonOpenFile.Text = "Open File";
            this.buttonOpenFile.UseVisualStyleBackColor = false;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // buttonSaveInFile
            // 
            this.buttonSaveInFile.BackColor = System.Drawing.Color.LightBlue;
            this.buttonSaveInFile.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.buttonSaveInFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveInFile.Font = new System.Drawing.Font("Georgia", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSaveInFile.ForeColor = System.Drawing.Color.Indigo;
            this.buttonSaveInFile.Location = new System.Drawing.Point(116, 12);
            this.buttonSaveInFile.Name = "buttonSaveInFile";
            this.buttonSaveInFile.Size = new System.Drawing.Size(104, 35);
            this.buttonSaveInFile.TabIndex = 4;
            this.buttonSaveInFile.Text = "Save In File";
            this.buttonSaveInFile.UseVisualStyleBackColor = false;
            this.buttonSaveInFile.Click += new System.EventHandler(this.buttonSaveInFile_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(129)))));
            this.buttonExit.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.buttonExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExit.Font = new System.Drawing.Font("Georgia", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExit.ForeColor = System.Drawing.Color.Indigo;
            this.buttonExit.Location = new System.Drawing.Point(226, 12);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(104, 35);
            this.buttonExit.TabIndex = 5;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = false;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Gray;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(415, 53);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(403, 358);
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Gray;
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Location = new System.Drawing.Point(824, 53);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(403, 358);
            this.pictureBox3.TabIndex = 7;
            this.pictureBox3.TabStop = false;
            // 
            // buttonShadowDetectionLab
            // 
            this.buttonShadowDetectionLab.Location = new System.Drawing.Point(116, 437);
            this.buttonShadowDetectionLab.Name = "buttonShadowDetectionLab";
            this.buttonShadowDetectionLab.Size = new System.Drawing.Size(75, 27);
            this.buttonShadowDetectionLab.TabIndex = 8;
            this.buttonShadowDetectionLab.Text = "LAB";
            this.buttonShadowDetectionLab.UseVisualStyleBackColor = true;
            this.buttonShadowDetectionLab.Click += new System.EventHandler(this.buttonShadowDetectionLab_Click);
            // 
            // textBoxShadowDetectionLabThreshold
            // 
            this.textBoxShadowDetectionLabThreshold.Location = new System.Drawing.Point(197, 444);
            this.textBoxShadowDetectionLabThreshold.Name = "textBoxShadowDetectionLabThreshold";
            this.textBoxShadowDetectionLabThreshold.Size = new System.Drawing.Size(85, 20);
            this.textBoxShadowDetectionLabThreshold.TabIndex = 9;
            // 
            // buttonShadowDetectionMS
            // 
            this.buttonShadowDetectionMS.Location = new System.Drawing.Point(116, 470);
            this.buttonShadowDetectionMS.Name = "buttonShadowDetectionMS";
            this.buttonShadowDetectionMS.Size = new System.Drawing.Size(75, 27);
            this.buttonShadowDetectionMS.TabIndex = 10;
            this.buttonShadowDetectionMS.Text = "MS";
            this.buttonShadowDetectionMS.UseVisualStyleBackColor = true;
            this.buttonShadowDetectionMS.Click += new System.EventHandler(this.buttonShadowDetectionMS_Click);
            // 
            // textBoxShadowDetectionLMSThreshold
            // 
            this.textBoxShadowDetectionLMSThreshold.Location = new System.Drawing.Point(197, 477);
            this.textBoxShadowDetectionLMSThreshold.Name = "textBoxShadowDetectionLMSThreshold";
            this.textBoxShadowDetectionLMSThreshold.Size = new System.Drawing.Size(85, 20);
            this.textBoxShadowDetectionLMSThreshold.TabIndex = 11;
            // 
            // checkBoxShadowDetectionLMSShowWindows
            // 
            this.checkBoxShadowDetectionLMSShowWindows.AutoSize = true;
            this.checkBoxShadowDetectionLMSShowWindows.Location = new System.Drawing.Point(288, 480);
            this.checkBoxShadowDetectionLMSShowWindows.Name = "checkBoxShadowDetectionLMSShowWindows";
            this.checkBoxShadowDetectionLMSShowWindows.Size = new System.Drawing.Size(76, 17);
            this.checkBoxShadowDetectionLMSShowWindows.TabIndex = 12;
            this.checkBoxShadowDetectionLMSShowWindows.Text = "Windows?";
            this.checkBoxShadowDetectionLMSShowWindows.UseVisualStyleBackColor = true;
            // 
            // checkBoxShadowDetectionLabShowWindows
            // 
            this.checkBoxShadowDetectionLabShowWindows.AutoSize = true;
            this.checkBoxShadowDetectionLabShowWindows.Location = new System.Drawing.Point(288, 446);
            this.checkBoxShadowDetectionLabShowWindows.Name = "checkBoxShadowDetectionLabShowWindows";
            this.checkBoxShadowDetectionLabShowWindows.Size = new System.Drawing.Size(76, 17);
            this.checkBoxShadowDetectionLabShowWindows.TabIndex = 13;
            this.checkBoxShadowDetectionLabShowWindows.Text = "Windows?";
            this.checkBoxShadowDetectionLabShowWindows.UseVisualStyleBackColor = true;
            // 
            // buttonCloseAllWindows
            // 
            this.buttonCloseAllWindows.BackColor = System.Drawing.Color.IndianRed;
            this.buttonCloseAllWindows.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.buttonCloseAllWindows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCloseAllWindows.Font = new System.Drawing.Font("Georgia", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCloseAllWindows.ForeColor = System.Drawing.Color.Indigo;
            this.buttonCloseAllWindows.Location = new System.Drawing.Point(336, 12);
            this.buttonCloseAllWindows.Name = "buttonCloseAllWindows";
            this.buttonCloseAllWindows.Size = new System.Drawing.Size(128, 35);
            this.buttonCloseAllWindows.TabIndex = 14;
            this.buttonCloseAllWindows.Text = "Close all windows";
            this.buttonCloseAllWindows.UseVisualStyleBackColor = false;
            this.buttonCloseAllWindows.Click += new System.EventHandler(this.buttonCloseAllWindows_Click);
            // 
            // buttonShadowRemovalAditiveMethod
            // 
            this.buttonShadowRemovalAditiveMethod.Location = new System.Drawing.Point(415, 413);
            this.buttonShadowRemovalAditiveMethod.Name = "buttonShadowRemovalAditiveMethod";
            this.buttonShadowRemovalAditiveMethod.Size = new System.Drawing.Size(75, 27);
            this.buttonShadowRemovalAditiveMethod.TabIndex = 15;
            this.buttonShadowRemovalAditiveMethod.Text = "Aditive";
            this.buttonShadowRemovalAditiveMethod.UseVisualStyleBackColor = true;
            this.buttonShadowRemovalAditiveMethod.Click += new System.EventHandler(this.buttonShadowRemovalAditiveMethod_Click);
            // 
            // buttonShadowRemovalConstantMethod
            // 
            this.buttonShadowRemovalConstantMethod.Location = new System.Drawing.Point(415, 545);
            this.buttonShadowRemovalConstantMethod.Name = "buttonShadowRemovalConstantMethod";
            this.buttonShadowRemovalConstantMethod.Size = new System.Drawing.Size(75, 27);
            this.buttonShadowRemovalConstantMethod.TabIndex = 16;
            this.buttonShadowRemovalConstantMethod.Text = "Constan";
            this.buttonShadowRemovalConstantMethod.UseVisualStyleBackColor = true;
            this.buttonShadowRemovalConstantMethod.Click += new System.EventHandler(this.buttonShadowRemovalConstantMethod_Click);
            // 
            // buttonShadowRemovalLabMethod
            // 
            this.buttonShadowRemovalLabMethod.Location = new System.Drawing.Point(415, 512);
            this.buttonShadowRemovalLabMethod.Name = "buttonShadowRemovalLabMethod";
            this.buttonShadowRemovalLabMethod.Size = new System.Drawing.Size(75, 27);
            this.buttonShadowRemovalLabMethod.TabIndex = 17;
            this.buttonShadowRemovalLabMethod.Text = "LAB";
            this.buttonShadowRemovalLabMethod.UseVisualStyleBackColor = true;
            this.buttonShadowRemovalLabMethod.Click += new System.EventHandler(this.buttonShadowRemovalLabMethod_Click);
            // 
            // buttonShadowRemovalCombinedMethod
            // 
            this.buttonShadowRemovalCombinedMethod.Location = new System.Drawing.Point(415, 479);
            this.buttonShadowRemovalCombinedMethod.Name = "buttonShadowRemovalCombinedMethod";
            this.buttonShadowRemovalCombinedMethod.Size = new System.Drawing.Size(75, 27);
            this.buttonShadowRemovalCombinedMethod.TabIndex = 18;
            this.buttonShadowRemovalCombinedMethod.Text = "Combined";
            this.buttonShadowRemovalCombinedMethod.UseVisualStyleBackColor = true;
            this.buttonShadowRemovalCombinedMethod.Click += new System.EventHandler(this.buttonShadowRemovalCombinedMethod_Click);
            // 
            // buttonShadowRemovalBasicLightModelMethod
            // 
            this.buttonShadowRemovalBasicLightModelMethod.Location = new System.Drawing.Point(415, 446);
            this.buttonShadowRemovalBasicLightModelMethod.Name = "buttonShadowRemovalBasicLightModelMethod";
            this.buttonShadowRemovalBasicLightModelMethod.Size = new System.Drawing.Size(75, 27);
            this.buttonShadowRemovalBasicLightModelMethod.TabIndex = 19;
            this.buttonShadowRemovalBasicLightModelMethod.Text = "BasicLightModel";
            this.buttonShadowRemovalBasicLightModelMethod.UseVisualStyleBackColor = true;
            this.buttonShadowRemovalBasicLightModelMethod.Click += new System.EventHandler(this.buttonShadowRemovalBasicLightModelMethod_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(113, 417);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Shadow detection";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(496, 417);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Shadow removal";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(636, 417);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Edges processing";
            // 
            // buttonImpaintShadowEdges
            // 
            this.buttonImpaintShadowEdges.Location = new System.Drawing.Point(721, 440);
            this.buttonImpaintShadowEdges.Name = "buttonImpaintShadowEdges";
            this.buttonImpaintShadowEdges.Size = new System.Drawing.Size(97, 27);
            this.buttonImpaintShadowEdges.TabIndex = 23;
            this.buttonImpaintShadowEdges.Text = "Inpaint";
            this.buttonImpaintShadowEdges.UseVisualStyleBackColor = true;
            this.buttonImpaintShadowEdges.Click += new System.EventHandler(this.buttonImpaintShadowEdges_Click);
            // 
            // buttonSmoothShadowEdgesUsingGaussianFilter
            // 
            this.buttonSmoothShadowEdgesUsingGaussianFilter.Location = new System.Drawing.Point(721, 470);
            this.buttonSmoothShadowEdgesUsingGaussianFilter.Name = "buttonSmoothShadowEdgesUsingGaussianFilter";
            this.buttonSmoothShadowEdgesUsingGaussianFilter.Size = new System.Drawing.Size(97, 27);
            this.buttonSmoothShadowEdgesUsingGaussianFilter.TabIndex = 24;
            this.buttonSmoothShadowEdgesUsingGaussianFilter.Text = "Gaussian filter";
            this.buttonSmoothShadowEdgesUsingGaussianFilter.UseVisualStyleBackColor = true;
            this.buttonSmoothShadowEdgesUsingGaussianFilter.Click += new System.EventHandler(this.buttonSmoothShadowEdgesUsingGaussianFilter_Click);
            // 
            // buttonSmoothShadowEdgesUsingMedianFilter
            // 
            this.buttonSmoothShadowEdgesUsingMedianFilter.Location = new System.Drawing.Point(721, 503);
            this.buttonSmoothShadowEdgesUsingMedianFilter.Name = "buttonSmoothShadowEdgesUsingMedianFilter";
            this.buttonSmoothShadowEdgesUsingMedianFilter.Size = new System.Drawing.Size(97, 27);
            this.buttonSmoothShadowEdgesUsingMedianFilter.TabIndex = 25;
            this.buttonSmoothShadowEdgesUsingMedianFilter.Text = "Median filter";
            this.buttonSmoothShadowEdgesUsingMedianFilter.UseVisualStyleBackColor = true;
            this.buttonSmoothShadowEdgesUsingMedianFilter.Click += new System.EventHandler(this.buttonSmoothShadowEdgesUsingMedianFilter_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(879, 420);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Fog removal";
            // 
            // buttonRemoveFogUsingDarkChannelMethod
            // 
            this.buttonRemoveFogUsingDarkChannelMethod.Location = new System.Drawing.Point(882, 444);
            this.buttonRemoveFogUsingDarkChannelMethod.Name = "buttonRemoveFogUsingDarkChannelMethod";
            this.buttonRemoveFogUsingDarkChannelMethod.Size = new System.Drawing.Size(97, 27);
            this.buttonRemoveFogUsingDarkChannelMethod.TabIndex = 27;
            this.buttonRemoveFogUsingDarkChannelMethod.Text = "Dark channel";
            this.buttonRemoveFogUsingDarkChannelMethod.UseVisualStyleBackColor = true;
            this.buttonRemoveFogUsingDarkChannelMethod.Click += new System.EventHandler(this.buttonRemoveFogUsingDarkChannelMethod_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1235, 578);
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
            this.Controls.Add(this.buttonCloseAllWindows);
            this.Controls.Add(this.checkBoxShadowDetectionLabShowWindows);
            this.Controls.Add(this.checkBoxShadowDetectionLMSShowWindows);
            this.Controls.Add(this.textBoxShadowDetectionLMSThreshold);
            this.Controls.Add(this.buttonShadowDetectionMS);
            this.Controls.Add(this.textBoxShadowDetectionLabThreshold);
            this.Controls.Add(this.buttonShadowDetectionLab);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonSaveInFile);
            this.Controls.Add(this.buttonOpenFile);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.Button buttonSaveInFile;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Button buttonShadowDetectionLab;
        private System.Windows.Forms.TextBox textBoxShadowDetectionLabThreshold;
        private System.Windows.Forms.Button buttonShadowDetectionMS;
        private System.Windows.Forms.TextBox textBoxShadowDetectionLMSThreshold;
        private System.Windows.Forms.CheckBox checkBoxShadowDetectionLMSShowWindows;
        private System.Windows.Forms.CheckBox checkBoxShadowDetectionLabShowWindows;
        private System.Windows.Forms.Button buttonCloseAllWindows;
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
    }
}

