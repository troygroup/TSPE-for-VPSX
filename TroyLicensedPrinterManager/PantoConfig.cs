using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace TroyLicensedPrinterManager
{
    public partial class PantoConfig : Form
    {
        public string BasePath = "";
        private PantographConfiguration swPantograph = null;

        public PantoConfig()
        {
            InitializeComponent();
        }

        private void PantoConfig_Load(object sender, EventArgs e)
        {
            string ConfigPath = BasePath + "\\Configuration";
            DirectoryInfo configdir = new DirectoryInfo(ConfigPath);
            cboModels.Items.Clear();

            try
            {
                foreach (DirectoryInfo dir in configdir.GetDirectories())
                {
                    cboModels.Items.Add(dir.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading pantograph configuration screen.  Error: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string pgFileName = BasePath + "\\Configuration\\" + cboModels.Text + "\\TroyPantographConfiguration.xml";
                if (!File.Exists(pgFileName))
                {
                    pgFileName = BasePath + "\\Configuration\\TroyPantographConfiguration.xml";
                }

                XmlSerializer dser = new XmlSerializer(typeof(PantographConfiguration));
                FileStream fs = new FileStream(pgFileName, FileMode.Open);
                swPantograph = (PantographConfiguration)dser.Deserialize(fs);

                numDarknessFactor.Value = Convert.ToDecimal(swPantograph.PantographConfigurations[0].BgDarknessFactor);
                numDensityPtn1.Value = Convert.ToDecimal(swPantograph.PantographConfigurations[0].DensityPattern1);
                numDensityPtn2.Value = Convert.ToDecimal(swPantograph.PantographConfigurations[0].DensityPattern2);
                numXAnchor.Value = Convert.ToDecimal(swPantograph.PantographConfigurations[0].InclusionRegion.XAnchor);
                numYAnchor.Value = Convert.ToDecimal(swPantograph.PantographConfigurations[0].InclusionRegion.YAnchor);
                numHeight.Value = Convert.ToDecimal(swPantograph.PantographConfigurations[0].InclusionRegion.Height);
                numWidth.Value = Convert.ToDecimal(swPantograph.PantographConfigurations[0].InclusionRegion.Width);
                numIntfPtn.Value = Convert.ToDecimal(swPantograph.PantographConfigurations[0].InterferencePatternId);
                txtMicroPrint.Text = swPantograph.PantographConfigurations[0].BorderString;
                chkUseFullPage.Checked = swPantograph.PantographConfigurations[0].UseDefaultInclusionForPaperSize;

                fs.Close();

                gbPantoConfig.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening pantograph configuration.  Error: " + ex.Message);
            }
        }

        private void chkUseFullPage_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseFullPage.Checked)
            {
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
            }
            else
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!gbPantoConfig.Enabled)
            {
                return;
            }
            if (ValidateSettings())
            {
                

                SaveConfig sc = new SaveConfig();
                sc.BasePath = BasePath;
                sc.CurrentConfig = cboModels.Text;
                sc.StartPosition = FormStartPosition.CenterParent;
                sc.ShowDialog();

                if (sc.SaveLocation != "")
                {
                    swPantograph.PantographConfigurations[0].BgDarknessFactor = Convert.ToInt32(numDarknessFactor.Value);
                    swPantograph.PantographConfigurations[0].DensityPattern1 = Convert.ToInt32(numDensityPtn1.Value);
                    swPantograph.PantographConfigurations[0].DensityPattern2 = Convert.ToInt32(numDensityPtn2.Value);
                    if (swPantograph.PantographConfigurations[0].InclusionRegion == null)
                    {
                        swPantograph.PantographConfigurations[0].InclusionRegion = new PantographPclBuilder.PantographRegionObjectType();
                    }
                    swPantograph.PantographConfigurations[0].InclusionRegion.XAnchor = Convert.ToInt32(numXAnchor.Value);
                    swPantograph.PantographConfigurations[0].InclusionRegion.YAnchor = Convert.ToInt32(numYAnchor.Value);
                    swPantograph.PantographConfigurations[0].InclusionRegion.Height = Convert.ToInt32(numHeight.Value);
                    swPantograph.PantographConfigurations[0].InclusionRegion.Width = Convert.ToInt32(numWidth.Value);
                    swPantograph.PantographConfigurations[0].InterferencePatternId = Convert.ToInt32(numIntfPtn.Value);
                    swPantograph.PantographConfigurations[0].BorderString = txtMicroPrint.Text;
                    swPantograph.PantographConfigurations[0].UseDefaultInclusionForPaperSize = chkUseFullPage.Checked;

                    string configloc = BasePath + "\\Configuration\\" + sc.SaveLocation;
                    if (!Directory.Exists(configloc))
                    {
                        Directory.CreateDirectory(configloc);
                    }
                    XmlSerializer xser = new XmlSerializer(typeof(PantographConfiguration));
                    string filename = configloc + "\\TroyPantographConfiguration.xml";
                    TextWriter writer = new StreamWriter(filename);
                    xser.Serialize(writer, swPantograph);
                    writer.Close();

                    string pfilename = configloc + "\\PantographProfile1Page1.pcl";
                    PantographPclBuilder.BuildPantograph pBuilder = new PantographPclBuilder.BuildPantograph();
                    pBuilder.CreatePantographPcl(pfilename, swPantograph.PantographConfigurations[0], BasePath + "\\Configuration\\", true);

                    MessageBox.Show(this, "Configuation settings saved.  Pantograph configuration saved to: " + sc.SaveLocation);

                    this.Close();

                }

            }
        }

        private bool ValidateSettings()
        {
            if ((!chkUseFullPage.Checked) && ((numHeight.Value == 0) || (numWidth.Value == 0)))
            {
                MessageBox.Show("Pantograph Height and Width cannot be 0.  Please enter a number greater than zero or select the Use Full Page Pantograph option.");
                return false;
            }


            return true;
        }

        private void btnIntPattrn_Click(object sender, EventArgs e)
        {
            IntPttrn ip = new IntPttrn();
            ip.StartPosition = FormStartPosition.CenterParent;
            ip.ShowDialog();

            if (ip.retValue > 0)
            {
                numIntfPtn.Value = ip.retValue;
            }
        }
    }
}
