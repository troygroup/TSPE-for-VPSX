namespace TroyLicensedPrinterManager
{
    partial class frmLicensedPrinter
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLicensedPrinter));
            this.dgvPrinterList = new System.Windows.Forms.DataGridView();
            this.PrinterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TroymarkConfig = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PantographConfig = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLicenseStatus = new System.Windows.Forms.TextBox();
            this.btnCheckLicenseStatus = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPrinterCount = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCreateLrf = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnImportCsv = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.btnPantograph = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrinterList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPrinterList
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPrinterList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPrinterList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrinterList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PrinterName,
            this.TroymarkConfig,
            this.PantographConfig});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPrinterList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPrinterList.Enabled = false;
            this.dgvPrinterList.Location = new System.Drawing.Point(12, 50);
            this.dgvPrinterList.Name = "dgvPrinterList";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPrinterList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvPrinterList.RowHeadersWidth = 25;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPrinterList.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvPrinterList.Size = new System.Drawing.Size(778, 280);
            this.dgvPrinterList.TabIndex = 0;
            this.dgvPrinterList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dgvPrinterList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dgvPrinterList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvPrinterList_RowsAdded);
            this.dgvPrinterList.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvPrinterList_RowsRemoved);
            this.dgvPrinterList.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPrinterList_RowValidated);
            this.dgvPrinterList.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvPrinterList_UserAddedRow);
            this.dgvPrinterList.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvPrinterList_UserDeletedRow);
            this.dgvPrinterList.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvPrinterList_UserDeletingRow);
            // 
            // PrinterName
            // 
            this.PrinterName.HeaderText = "Printer Name";
            this.PrinterName.Name = "PrinterName";
            this.PrinterName.Width = 250;
            // 
            // TroymarkConfig
            // 
            this.TroymarkConfig.HeaderText = "TROYmark Configuration";
            this.TroymarkConfig.Name = "TroymarkConfig";
            this.TroymarkConfig.Width = 250;
            // 
            // PantographConfig
            // 
            this.PantographConfig.HeaderText = "Pantograph Configuration";
            this.PantographConfig.Name = "PantographConfig";
            this.PantographConfig.Width = 250;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "License Status:";
            // 
            // txtLicenseStatus
            // 
            this.txtLicenseStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLicenseStatus.Location = new System.Drawing.Point(143, 12);
            this.txtLicenseStatus.Name = "txtLicenseStatus";
            this.txtLicenseStatus.ReadOnly = true;
            this.txtLicenseStatus.Size = new System.Drawing.Size(230, 24);
            this.txtLicenseStatus.TabIndex = 2;
            // 
            // btnCheckLicenseStatus
            // 
            this.btnCheckLicenseStatus.Location = new System.Drawing.Point(693, 11);
            this.btnCheckLicenseStatus.Name = "btnCheckLicenseStatus";
            this.btnCheckLicenseStatus.Size = new System.Drawing.Size(100, 25);
            this.btnCheckLicenseStatus.TabIndex = 3;
            this.btnCheckLicenseStatus.Text = "Re-Read License";
            this.btnCheckLicenseStatus.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(379, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(193, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Used/Max Printer Count:";
            // 
            // txtPrinterCount
            // 
            this.txtPrinterCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrinterCount.Location = new System.Drawing.Point(570, 13);
            this.txtPrinterCount.Name = "txtPrinterCount";
            this.txtPrinterCount.ReadOnly = true;
            this.txtPrinterCount.Size = new System.Drawing.Size(79, 24);
            this.txtPrinterCount.TabIndex = 5;
            // 
            // btnOk
            // 
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(612, 379);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(715, 379);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCreateLrf
            // 
            this.btnCreateLrf.Location = new System.Drawing.Point(16, 382);
            this.btnCreateLrf.Name = "btnCreateLrf";
            this.btnCreateLrf.Size = new System.Drawing.Size(84, 23);
            this.btnCreateLrf.TabIndex = 8;
            this.btnCreateLrf.Text = "Create LRF";
            this.btnCreateLrf.UseVisualStyleBackColor = true;
            this.btnCreateLrf.Click += new System.EventHandler(this.btnCreateLrf_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(12, 336);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(58, 21);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnImportCsv
            // 
            this.btnImportCsv.Location = new System.Drawing.Point(117, 382);
            this.btnImportCsv.Name = "btnImportCsv";
            this.btnImportCsv.Size = new System.Drawing.Size(84, 23);
            this.btnImportCsv.TabIndex = 10;
            this.btnImportCsv.Text = "Import CSV";
            this.btnImportCsv.UseVisualStyleBackColor = true;
            this.btnImportCsv.Click += new System.EventHandler(this.btnImportCsv_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(218, 382);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Pattern Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnPantograph
            // 
            this.btnPantograph.Location = new System.Drawing.Point(319, 382);
            this.btnPantograph.Name = "btnPantograph";
            this.btnPantograph.Size = new System.Drawing.Size(84, 23);
            this.btnPantograph.TabIndex = 12;
            this.btnPantograph.Text = "Pantograph";
            this.btnPantograph.UseVisualStyleBackColor = true;
            this.btnPantograph.Click += new System.EventHandler(this.btnPantograph_Click);
            // 
            // frmLicensedPrinter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 417);
            this.Controls.Add(this.btnPantograph);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnImportCsv);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnCreateLrf);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtPrinterCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCheckLicenseStatus);
            this.Controls.Add(this.txtLicenseStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvPrinterList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLicensedPrinter";
            this.Text = "TROY Licensed Printer Manager";
            this.Load += new System.EventHandler(this.frmLicensedPrinter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrinterList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPrinterList;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrinterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TroymarkConfig;
        private System.Windows.Forms.DataGridViewTextBoxColumn PantographConfig;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLicenseStatus;
        private System.Windows.Forms.Button btnCheckLicenseStatus;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPrinterCount;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCreateLrf;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnImportCsv;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnPantograph;
    }
}

