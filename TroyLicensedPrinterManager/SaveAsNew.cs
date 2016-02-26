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
    public partial class SaveAsNew : Form
    {
        public string ReturnName = "";
        public string BasePath = "";

        public SaveAsNew()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ReturnName = "";
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtNewName.Text == "")
            {
                MessageBox.Show(this, "Please enter a name before selecting OK.");
                return;
            }

            string pathname = BasePath + "\\Configuration\\" + txtNewName.Text;
            if (Directory.Exists(pathname))
            {
                MessageBox.Show("Configuration name already exists.  Please enter another name.");
                return;
            }

            ReturnName = txtNewName.Text;
            this.Close();
        }



    }
}
