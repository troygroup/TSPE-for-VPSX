namespace TroyLicensedPrinterManager
{
    partial class PatternTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatternTest));
            this.label1 = new System.Windows.Forms.Label();
            this.cboDefaultPrinter = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkPattern2 = new System.Windows.Forms.CheckBox();
            this.chkPattern1 = new System.Windows.Forms.CheckBox();
            this.btnPrintPage = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.numDarknessFactor = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDarknessFactor)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Printer:";
            // 
            // cboDefaultPrinter
            // 
            this.cboDefaultPrinter.FormattingEnabled = true;
            this.cboDefaultPrinter.Location = new System.Drawing.Point(68, 17);
            this.cboDefaultPrinter.Name = "cboDefaultPrinter";
            this.cboDefaultPrinter.Size = new System.Drawing.Size(309, 21);
            this.cboDefaultPrinter.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numDarknessFactor);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.chkPattern2);
            this.groupBox1.Controls.Add(this.chkPattern1);
            this.groupBox1.Location = new System.Drawing.Point(25, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(385, 50);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Print Pattern Density Page For...";
            // 
            // chkPattern2
            // 
            this.chkPattern2.AutoSize = true;
            this.chkPattern2.Location = new System.Drawing.Point(111, 19);
            this.chkPattern2.Name = "chkPattern2";
            this.chkPattern2.Size = new System.Drawing.Size(69, 17);
            this.chkPattern2.TabIndex = 1;
            this.chkPattern2.Text = "Pattern 2";
            this.chkPattern2.UseVisualStyleBackColor = true;
            // 
            // chkPattern1
            // 
            this.chkPattern1.AutoSize = true;
            this.chkPattern1.Location = new System.Drawing.Point(16, 19);
            this.chkPattern1.Name = "chkPattern1";
            this.chkPattern1.Size = new System.Drawing.Size(69, 17);
            this.chkPattern1.TabIndex = 0;
            this.chkPattern1.Text = "Pattern 1";
            this.chkPattern1.UseVisualStyleBackColor = true;
            // 
            // btnPrintPage
            // 
            this.btnPrintPage.Location = new System.Drawing.Point(25, 122);
            this.btnPrintPage.Name = "btnPrintPage";
            this.btnPrintPage.Size = new System.Drawing.Size(110, 28);
            this.btnPrintPage.TabIndex = 8;
            this.btnPrintPage.Text = "Print Density Page";
            this.btnPrintPage.UseVisualStyleBackColor = true;
            this.btnPrintPage.Click += new System.EventHandler(this.btnPrintPage_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(203, 20);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(88, 13);
            this.label14.TabIndex = 22;
            this.label14.Text = "Darkness Factor:";
            // 
            // numDarknessFactor
            // 
            this.numDarknessFactor.Location = new System.Drawing.Point(302, 18);
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
            this.numDarknessFactor.TabIndex = 23;
            this.numDarknessFactor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // PatternTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 173);
            this.Controls.Add(this.btnPrintPage);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cboDefaultPrinter);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PatternTest";
            this.Text = "Pattern Test";
            this.Load += new System.EventHandler(this.PatternTest_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDarknessFactor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboDefaultPrinter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkPattern2;
        private System.Windows.Forms.CheckBox chkPattern1;
        private System.Windows.Forms.Button btnPrintPage;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown numDarknessFactor;
    }
}