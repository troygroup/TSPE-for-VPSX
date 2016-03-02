using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TroyLicensedPrinterManager
{
    public partial class SaveConfig : Form
    {
        public string CurrentConfig = "";
        public string SaveLocation = "";
        public string CommonPath = "";

        public SaveConfig()
        {
            InitializeComponent();
        }

        

        private void SaveConfig_Load(object sender, EventArgs e)
        {
            lblMain.Text = "Do you want to overwrite the existing pantograph configuration for: " + CurrentConfig + "?  Or do you want to save this configuration as a new printer model?";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOverwrite_Click(object sender, EventArgs e)
        {
            SaveLocation = CurrentConfig;
            this.Close();
        }

        private void btnSaveAsNew_Click(object sender, EventArgs e)
        {
            SaveAsNew san = new SaveAsNew();
            san.CommonPath = CommonPath;
            san.StartPosition = FormStartPosition.CenterParent; 
            san.ShowDialog();
            if (san.ReturnName == "")
            {
                return;
            }

            SaveLocation = san.ReturnName;
            this.Close();

        }

       

       
    }
}
