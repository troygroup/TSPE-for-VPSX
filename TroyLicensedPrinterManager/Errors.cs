using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TroyLicensedPrinterManager
{
    public partial class Errors : Form
    {
        public List<string> ErrorList = new List<string>();
        public bool CancelImport = true;

        public Errors()
        {
            InitializeComponent();
        }

        private void Errors_Load(object sender, EventArgs e)
        {
            foreach (string str in ErrorList)
            {
                lstErrors.Items.Add(str);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelImport = true;
            this.Close();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            CancelImport = false;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.OverwritePrompt = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo saveFile = new FileInfo(saveFileDialog1.FileName);
                StreamWriter saveWrite = new StreamWriter(saveFile.OpenWrite());

                foreach (string listString in lstErrors.Items)
                {
                    saveWrite.WriteLine(listString);
                }
                saveWrite.Close();
                MessageBox.Show("Information saved to " + saveFileDialog1.FileName);
            }

        }
    }
}
