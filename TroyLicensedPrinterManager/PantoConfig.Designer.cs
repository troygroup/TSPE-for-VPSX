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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.cboModels = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pantoControl4 = new TroyLicensedPrinterManager.PantoControl();
            this.pantoControl3 = new TroyLicensedPrinterManager.PantoControl();
            this.pantoControl2 = new TroyLicensedPrinterManager.PantoControl();
            this.pantoControl1 = new TroyLicensedPrinterManager.PantoControl();
            this.txtMicroPrint = new System.Windows.Forms.TextBox();
            this.numIntfPtn = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.btnIntPattrn = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numDensityPtn2 = new System.Windows.Forms.NumericUpDown();
            this.numDensityPtn1 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chkUseFullPage = new System.Windows.Forms.CheckBox();
            this.numDarknessFactor = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkTM = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIntfPtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensityPtn2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensityPtn1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDarknessFactor)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(688, 307);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(590, 307);
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
            this.label13.Location = new System.Drawing.Point(163, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 13);
            this.label13.TabIndex = 18;
            this.label13.Text = "Printer Model:";
            // 
            // cboModels
            // 
            this.cboModels.FormattingEnabled = true;
            this.cboModels.Location = new System.Drawing.Point(255, 18);
            this.cboModels.Name = "cboModels";
            this.cboModels.Size = new System.Drawing.Size(227, 21);
            this.cboModels.TabIndex = 19;
            this.cboModels.SelectedIndexChanged += new System.EventHandler(this.cboModels_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.pantoControl4);
            this.groupBox1.Controls.Add(this.pantoControl3);
            this.groupBox1.Controls.Add(this.pantoControl2);
            this.groupBox1.Controls.Add(this.pantoControl1);
            this.groupBox1.Location = new System.Drawing.Point(34, 142);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(707, 148);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(95, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "Lower Left Pantograph";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(437, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "Lower Right Pantograph";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(437, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "Upper Right Pantograph";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(95, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Upper Left Pantograph";
            // 
            // pantoControl4
            // 
            this.pantoControl4.Location = new System.Drawing.Point(357, 94);
            this.pantoControl4.Name = "pantoControl4";
            this.pantoControl4.Size = new System.Drawing.Size(318, 46);
            this.pantoControl4.TabIndex = 34;
            // 
            // pantoControl3
            // 
            this.pantoControl3.Location = new System.Drawing.Point(17, 95);
            this.pantoControl3.Name = "pantoControl3";
            this.pantoControl3.Size = new System.Drawing.Size(315, 45);
            this.pantoControl3.TabIndex = 33;
            // 
            // pantoControl2
            // 
            this.pantoControl2.Location = new System.Drawing.Point(357, 33);
            this.pantoControl2.Name = "pantoControl2";
            this.pantoControl2.Size = new System.Drawing.Size(318, 46);
            this.pantoControl2.TabIndex = 32;
            // 
            // pantoControl1
            // 
            this.pantoControl1.Location = new System.Drawing.Point(17, 33);
            this.pantoControl1.Name = "pantoControl1";
            this.pantoControl1.Size = new System.Drawing.Size(323, 46);
            this.pantoControl1.TabIndex = 31;
            // 
            // txtMicroPrint
            // 
            this.txtMicroPrint.Location = new System.Drawing.Point(380, 82);
            this.txtMicroPrint.Name = "txtMicroPrint";
            this.txtMicroPrint.Size = new System.Drawing.Size(383, 20);
            this.txtMicroPrint.TabIndex = 10;
            // 
            // numIntfPtn
            // 
            this.numIntfPtn.Location = new System.Drawing.Point(142, 81);
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
            this.numIntfPtn.Size = new System.Drawing.Size(43, 20);
            this.numIntfPtn.TabIndex = 11;
            this.numIntfPtn.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(236, 83);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(138, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "MicroPrint Border Text:";
            // 
            // btnIntPattrn
            // 
            this.btnIntPattrn.Location = new System.Drawing.Point(191, 79);
            this.btnIntPattrn.Name = "btnIntPattrn";
            this.btnIntPattrn.Size = new System.Drawing.Size(27, 23);
            this.btnIntPattrn.TabIndex = 12;
            this.btnIntPattrn.Text = "...";
            this.btnIntPattrn.UseVisualStyleBackColor = true;
            this.btnIntPattrn.Click += new System.EventHandler(this.btnIntPattrn_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(12, 83);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(125, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Interference Pattern:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(299, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pantograph Inclusion Regions (Units: 1/600th inch)";
            // 
            // numDensityPtn2
            // 
            this.numDensityPtn2.Location = new System.Drawing.Point(474, 48);
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
            this.numDensityPtn2.Size = new System.Drawing.Size(56, 20);
            this.numDensityPtn2.TabIndex = 7;
            this.numDensityPtn2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numDensityPtn1
            // 
            this.numDensityPtn1.Location = new System.Drawing.Point(265, 48);
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
            this.numDensityPtn1.Size = new System.Drawing.Size(56, 20);
            this.numDensityPtn1.TabIndex = 6;
            this.numDensityPtn1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(338, 51);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(130, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Density Setting Pattern 2: ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(129, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(130, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Density Setting Pattern 1: ";
            // 
            // chkUseFullPage
            // 
            this.chkUseFullPage.AutoSize = true;
            this.chkUseFullPage.Location = new System.Drawing.Point(353, 119);
            this.chkUseFullPage.Name = "chkUseFullPage";
            this.chkUseFullPage.Size = new System.Drawing.Size(150, 17);
            this.chkUseFullPage.TabIndex = 22;
            this.chkUseFullPage.Text = "Use Full Page Pantograph";
            this.chkUseFullPage.UseVisualStyleBackColor = true;
            this.chkUseFullPage.CheckedChanged += new System.EventHandler(this.chkUseFullPage_CheckedChanged);
            // 
            // numDarknessFactor
            // 
            this.numDarknessFactor.Location = new System.Drawing.Point(671, 48);
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
            this.numDarknessFactor.Size = new System.Drawing.Size(38, 20);
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
            this.label14.Location = new System.Drawing.Point(577, 50);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(88, 13);
            this.label14.TabIndex = 20;
            this.label14.Text = "Darkness Factor:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Quality Adjustment:";
            // 
            // chkTM
            // 
            this.chkTM.AutoSize = true;
            this.chkTM.Checked = true;
            this.chkTM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTM.Location = new System.Drawing.Point(509, 119);
            this.chkTM.Name = "chkTM";
            this.chkTM.Size = new System.Drawing.Size(197, 17);
            this.chkTM.TabIndex = 32;
            this.chkTM.Text = "Add TROYmark text to Warning Box";
            this.chkTM.UseVisualStyleBackColor = true;
            // 
            // PantoConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 351);
            this.Controls.Add(this.chkTM);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.numDarknessFactor);
            this.Controls.Add(this.cboModels);
            this.Controls.Add(this.chkUseFullPage);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.numDensityPtn1);
            this.Controls.Add(this.numDensityPtn2);
            this.Controls.Add(this.txtMicroPrint);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numIntfPtn);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnIntPattrn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PantoConfig";
            this.Text = "  ";
            this.Load += new System.EventHandler(this.PantoConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIntfPtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensityPtn2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensityPtn1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDarknessFactor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cboModels;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private PantoControl pantoControl4;
        private PantoControl pantoControl3;
        private PantoControl pantoControl2;
        private PantoControl pantoControl1;
        private System.Windows.Forms.TextBox txtMicroPrint;
        private System.Windows.Forms.NumericUpDown numIntfPtn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnIntPattrn;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numDensityPtn2;
        private System.Windows.Forms.NumericUpDown numDensityPtn1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkUseFullPage;
        private System.Windows.Forms.NumericUpDown numDarknessFactor;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkTM;
    }
}