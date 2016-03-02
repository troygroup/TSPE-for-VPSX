namespace TroyLicensedPrinterManager
{
    partial class PantoControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.numYAnchor = new System.Windows.Forms.NumericUpDown();
            this.numXAnchor = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numYAnchor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numXAnchor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // numYAnchor
            // 
            this.numYAnchor.Location = new System.Drawing.Point(108, 21);
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
            this.numXAnchor.Location = new System.Drawing.Point(108, 3);
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
            this.label3.Location = new System.Drawing.Point(3, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "From Top Edge (Y):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "From Left Edge (X):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(201, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Width:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(201, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Height:";
            // 
            // numWidth
            // 
            this.numWidth.Location = new System.Drawing.Point(248, 3);
            this.numWidth.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(73, 20);
            this.numWidth.TabIndex = 2;
            // 
            // numHeight
            // 
            this.numHeight.Location = new System.Drawing.Point(248, 21);
            this.numHeight.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(73, 20);
            this.numHeight.TabIndex = 3;
            // 
            // PantoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numYAnchor);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numXAnchor);
            this.Controls.Add(this.numHeight);
            this.Controls.Add(this.numWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Name = "PantoControl";
            this.Size = new System.Drawing.Size(333, 55);
            ((System.ComponentModel.ISupportInitialize)(this.numYAnchor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numXAnchor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.NumericUpDown numYAnchor;
        public System.Windows.Forms.NumericUpDown numXAnchor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.NumericUpDown numWidth;
        public System.Windows.Forms.NumericUpDown numHeight;
    }
}
