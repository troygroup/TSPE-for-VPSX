namespace TroyLicensedPrinterManager
{
    partial class PantoConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PantoConfig));
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numYAnchor = new System.Windows.Forms.NumericUpDown();
            this.numXAnchor = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.numDensityPtn1 = new System.Windows.Forms.NumericUpDown();
            this.numDensityPtn2 = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMicroPrint = new System.Windows.Forms.TextBox();
            this.numIntfPtn = new System.Windows.Forms.NumericUpDown();
            this.btnIntPattrn = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.cboModels = new System.Windows.Forms.ComboBox();
            this.numDarknessFactor = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.chkUseFullPage = new System.Windows.Forms.CheckBox();
            this.gbPantoConfig = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numYAnchor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numXAnchor)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensityPtn1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensityPtn2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIntfPtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDarknessFactor)).BeginInit();
            this.gbPantoConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(293, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pantograph Inclusion Region (Units: 1/600th inch)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numYAnchor);
            this.groupBox1.Controls.Add(this.numXAnchor);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(75, 155);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 77);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Starting Point";
            // 
            // numYAnchor
            // 
            this.numYAnchor.Location = new System.Drawing.Point(169, 46);
            this.numYAnchor.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numYAnchor.Name = "numYAnchor";
            this.numYAnchor.Size = new System.Drawing.Size(73, 20);
            this.numYAnchor.TabIndex = 3;
            // 
            // numXAnchor
            // 
            this.numXAnchor.Location = new System.Drawing.Point(169, 17);
            this.numXAnchor.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numXAnchor.Name = "numXAnchor";
            this.numXAnchor.Size = new System.Drawing.Size(73, 20);
            this.numXAnchor.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "From Top Edge (Y Anchor):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "From Left Edge (X Anchor):";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numHeight);
            this.groupBox2.Controls.Add(this.numWidth);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(344, 154);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(178, 77);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Size";
            // 
            // numHeight
            // 
            this.numHeight.Location = new System.Drawing.Point(80, 46);
            this.numHeight.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(73, 20);
            this.numHeight.TabIndex = 3;
            // 
            // numWidth
            // 
            this.numWidth.Location = new System.Drawing.Point(80, 17);
            this.numWidth.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(73, 20);
            this.numWidth.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Height:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Width:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(31, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Quality Adjustment";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(63, 58);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(130, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Density Setting Pattern 1: ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(292, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(130, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Density Setting Pattern 2: ";
            // 
            // numDensityPtn1
            // 
            this.numDensityPtn1.Location = new System.Drawing.Point(197, 55);
            this.numDensityPtn1.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numDensityPtn1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDensityPtn1.Name = "numDensityPtn1";
            this.numDensityPtn1.Size = new System.Drawing.Size(74, 20);
            this.numDensityPtn1.TabIndex = 6;
            this.numDensityPtn1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numDensityPtn2
            // 
            this.numDensityPtn2.Location = new System.Drawing.Point(426, 55);
            this.numDensityPtn2.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numDensityPtn2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDensityPtn2.Name = "numDensityPtn2";
            this.numDensityPtn2.Size = new System.Drawing.Size(74, 20);
            this.numDensityPtn2.TabIndex = 7;
            this.numDensityPtn2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(31, 252);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(125, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Interference Pattern:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(231, 252);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(138, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "MicroPrint Border Text:";
            // 
            // txtMicroPrint
            // 
            this.txtMicroPrint.Location = new System.Drawing.Point(244, 275);
            this.txtMicroPrint.Name = "txtMicroPrint";
            this.txtMicroPrint.Size = new System.Drawing.Size(429, 20);
            this.txtMicroPrint.TabIndex = 10;
            // 
            // numIntfPtn
            // 
            this.numIntfPtn.Location = new System.Drawing.Point(66, 275);
            this.numIntfPtn.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numIntfPtn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numIntfPtn.Name = "numIntfPtn";
            this.numIntfPtn.Size = new System.Drawing.Size(63, 20);
            this.numIntfPtn.TabIndex = 11;
            this.numIntfPtn.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnIntPattrn
            // 
            this.btnIntPattrn.Location = new System.Drawing.Point(136, 273);
            this.btnIntPattrn.Name = "btnIntPattrn";
            this.btnIntPattrn.Size = new System.Drawing.Size(31, 23);
            this.btnIntPattrn.TabIndex = 12;
            this.btnIntPattrn.Text = "...";
            this.btnIntPattrn.UseVisualStyleBackColor = true;
            this.btnIntPattrn.Click += new System.EventHandler(this.btnIntPattrn_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(678, 387);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(580, 387);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 17;
            this.btnOk.Text = "Save";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(32, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(218, 13);
            this.label13.TabIndex = 18;
            this.label13.Text = "Open Configuration for Printer Model:";
            // 
            // cboModels
            // 
            this.cboModels.FormattingEnabled = true;
            this.cboModels.Location = new System.Drawing.Point(256, 18);
            this.cboModels.Name = "cboModels";
            this.cboModels.Size = new System.Drawing.Size(227, 21);
            this.cboModels.TabIndex = 19;
            this.cboModels.SelectedIndexChanged += new System.EventHandler(this.cboModels_SelectedIndexChanged);
            // 
            // numDarknessFactor
            // 
            this.numDarknessFactor.Location = new System.Drawing.Point(623, 55);
            this.numDarknessFactor.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numDarknessFactor.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDarknessFactor.Name = "numDarknessFactor";
            this.numDarknessFactor.Size = new System.Drawing.Size(50, 20);
            this.numDarknessFactor.TabIndex = 21;
            this.numDarknessFactor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(531, 58);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(88, 13);
            this.label14.TabIndex = 20;
            this.label14.Text = "Darkness Factor:";
            // 
            // chkUseFullPage
            // 
            this.chkUseFullPage.AutoSize = true;
            this.chkUseFullPage.Location = new System.Drawing.Point(54, 132);
            this.chkUseFullPage.Name = "chkUseFullPage";
            this.chkUseFullPage.Size = new System.Drawing.Size(150, 17);
            this.chkUseFullPage.TabIndex = 22;
            this.chkUseFullPage.Text = "Use Full Page Pantograph";
            this.chkUseFullPage.UseVisualStyleBackColor = true;
            this.chkUseFullPage.CheckedChanged += new System.EventHandler(this.chkUseFullPage_CheckedChanged);
            // 
            // gbPantoConfig
            // 
            this.gbPantoConfig.Controls.Add(this.label6);
            this.gbPantoConfig.Controls.Add(this.label14);
            this.gbPantoConfig.Controls.Add(this.numDarknessFactor);
            this.gbPantoConfig.Controls.Add(this.chkUseFullPage);
            this.gbPantoConfig.Controls.Add(this.label7);
            this.gbPantoConfig.Controls.Add(this.label8);
            this.gbPantoConfig.Controls.Add(this.numDensityPtn1);
            this.gbPantoConfig.Controls.Add(this.numDensityPtn2);
            this.gbPantoConfig.Controls.Add(this.label1);
            this.gbPantoConfig.Controls.Add(this.groupBox1);
            this.gbPantoConfig.Controls.Add(this.groupBox2);
            this.gbPantoConfig.Controls.Add(this.label9);
            this.gbPantoConfig.Controls.Add(this.btnIntPattrn);
            this.gbPantoConfig.Controls.Add(this.label10);
            this.gbPantoConfig.Controls.Add(this.numIntfPtn);
            this.gbPantoConfig.Controls.Add(this.txtMicroPrint);
            this.gbPantoConfig.Enabled = false;
            this.gbPantoConfig.Location = new System.Drawing.Point(46, 52);
            this.gbPantoConfig.Name = "gbPantoConfig";
            this.gbPantoConfig.Size = new System.Drawing.Size(707, 319);
            this.gbPantoConfig.TabIndex = 23;
            this.gbPantoConfig.TabStop = false;
            // 
            // PantoConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 431);
            this.Controls.Add(this.gbPantoConfig);
            this.Controls.Add(this.cboModels);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PantoConfig";
            this.Text = "  ";
            this.Load += new System.EventHandler(this.PantoConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numYAnchor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numXAnchor)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensityPtn1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensityPtn2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIntfPtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDarknessFactor)).EndInit();
            this.gbPantoConfig.ResumeLayout(false);
            this.gbPantoConfig.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numXAnchor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numYAnchor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numDensityPtn1;
        private System.Windows.Forms.NumericUpDown numDensityPtn2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMicroPrint;
        private System.Windows.Forms.NumericUpDown numIntfPtn;
        private System.Windows.Forms.Button btnIntPattrn;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cboModels;
        private System.Windows.Forms.NumericUpDown numDarknessFactor;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox chkUseFullPage;
        private System.Windows.Forms.GroupBox gbPantoConfig;
    }
}