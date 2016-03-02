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
    public partial class PatternTest : Form
    {
        public string CommonPath = "";

        public PatternTest()
        {
            InitializeComponent();
        }

        private void PatternTest_Load(object sender, EventArgs e)
        {
            cboDefaultPrinter.Items.Clear();

            foreach (string strPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                cboDefaultPrinter.Items.Add(strPrinter);
            }



        }

        private void btnPrintPage_Click(object sender, EventArgs e)
        {
            if (cboDefaultPrinter.Text == "")
            {
                MessageBox.Show("Please select a printer.");
                return;
            }
            string PrinterName = cboDefaultPrinter.Text;

            string fileName = CommonPath + @"\Data\";
            string ptnFileName;
            string results = "";
            bool patternChecked = false;
            int darkLevel = Convert.ToInt32(numDarknessFactor.Value);

            if (chkPattern1.Checked)
            {
                fileName = fileName + "DensityPattern1";
                if (darkLevel > 1)
                {
                    fileName += "Dark" + darkLevel;
                }
                ptnFileName = fileName + ".pcl";
                if (!File.Exists(ptnFileName))
                {
                    MessageBox.Show("Error.  Can not find PCL file for pattern 1.  File name: " + ptnFileName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    PrintToSpooler.SendFileToPrinter(PrinterName, ptnFileName, "Troy Port Monitor Density Pattern 1");
                    results += " Pattern 1 Test; ";
                }
                patternChecked = true;
            }
            if (chkPattern2.Checked)
            {
                fileName = fileName + "DensityPattern2";
                if (darkLevel > 1)
                {
                    fileName += "Dark" + darkLevel;
                }
                ptnFileName = fileName + ".pcl";
                if (!File.Exists(ptnFileName))
                {
                    MessageBox.Show("Error.  Can not find PCL file for pattern 2.  File name: " + ptnFileName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    PrintToSpooler.SendFileToPrinter(PrinterName, ptnFileName, "Troy Port Monitor Density Pattern 2");
                    results += " Pattern 2 Test; ";
                }
                patternChecked = true;

            }
            if (!patternChecked)
            {
                MessageBox.Show("Please select a pattern to print");

            }
            else if (results != "")
            {
                MessageBox.Show("Printing the following tests: " + results);
            }

        }

    }
}
