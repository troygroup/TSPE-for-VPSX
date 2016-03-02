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
using PantographPclBuilder;

namespace TroyLicensedPrinterManager
{
    public partial class PantoConfig : Form
    {
        public string BasePath = "";
        public string pantoFolder = "";
        private PantographConfiguration swPantograph = null;
        private static bool fullPagePantographUsed;


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
                cboModels.SelectedIndex = cboModels.Items.IndexOf(pantoFolder);
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
                fs.Close();

                numDarknessFactor.Value = Convert.ToDecimal(swPantograph.PantographConfigurations[0].BgDarknessFactor);
                numDensityPtn1.Value = Convert.ToDecimal(swPantograph.PantographConfigurations[0].DensityPattern1);
                numDensityPtn2.Value = Convert.ToDecimal(swPantograph.PantographConfigurations[0].DensityPattern2);

                numIntfPtn.Value = Convert.ToDecimal(swPantograph.PantographConfigurations[0].InterferencePatternId);
                txtMicroPrint.Text = swPantograph.PantographConfigurations[0].BorderString;
                chkUseFullPage.Checked = swPantograph.PantographConfigurations[0].UseDefaultInclusionForPaperSize;
                chkTM.Checked = swPantograph.PantographConfigurations[0].TROYmarkOn;

                GetInclusionRegion(pantoControl1, swPantograph.PantographConfigurations[0]);
                GetInclusionRegion(pantoControl2, swPantograph.PantographConfigurations[1]);
                GetInclusionRegion(pantoControl3, swPantograph.PantographConfigurations[2]);
                GetInclusionRegion(pantoControl4, swPantograph.PantographConfigurations[3]);
            }
            catch (Exception ex)
            {
                swPantograph =  new PantographConfiguration();
                var cc = new CustomConfiguration() {TROYmarkOn = true};
                swPantograph.PantographConfigurations.Add(cc);
                chkTM.Checked = swPantograph.PantographConfigurations[0].TROYmarkOn;
            }
        }

        private bool GetInclusionRegion(PantoControl pantoControl, CustomConfiguration item)
        {

            pantoControl.numXAnchor.Value = Convert.ToDecimal(item.InclusionRegion.XAnchor);
            pantoControl.numYAnchor.Value = Convert.ToDecimal(item.InclusionRegion.YAnchor);
            pantoControl.numHeight.Value = Convert.ToDecimal(item.InclusionRegion.Height);
            pantoControl.numWidth.Value = Convert.ToDecimal(item.InclusionRegion.Width);
            return true;
        }

        private void chkUseFullPage_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseFullPage.Checked)
            {
                groupBox1.Enabled = false;
            }
            else
            {
                groupBox1.Enabled = true;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateSettings())
            {
                SaveConfig sc = new SaveConfig();
                sc.BasePath = BasePath;
                sc.CurrentConfig = cboModels.Text;
                sc.StartPosition = FormStartPosition.CenterParent;
                sc.ShowDialog();

                if (sc.SaveLocation != "")
                {
                    swPantograph.PantographConfigurations.Clear();
                    fullPagePantographUsed = false;

                    AddPantoConfig(pantoControl1, ref swPantograph.PantographConfigurations);
                    AddPantoConfig(pantoControl2, ref swPantograph.PantographConfigurations);
                    AddPantoConfig(pantoControl3, ref swPantograph.PantographConfigurations);
                    AddPantoConfig(pantoControl4, ref swPantograph.PantographConfigurations);

                    string configloc = BasePath + "\\Configuration\\" + sc.SaveLocation;
                    if (!Directory.Exists(configloc))
                    {
                        Directory.CreateDirectory(configloc);
                    }
                    XmlSerializer xser = new XmlSerializer(typeof (PantographConfiguration));
                    string filename = configloc + "\\TroyPantographConfiguration.xml";
                    TextWriter writer = new StreamWriter(filename);
                    xser.Serialize(writer, swPantograph);
                    writer.Close();

                    var dir = new DirectoryInfo(configloc);
                    foreach (var file in dir.EnumerateFiles("PantographProfile*.pcl"))
                    {
                        file.Delete();
                    }

                    var end = swPantograph.PantographConfigurations.Count()-1;
                    for (int cntr = 0; cntr <= end; cntr++)
                    {
                        string pfilename = String.Format(@"{0}\PantographProfile{1}Page1.pcl", configloc, cntr+1);
                        var pBuilder = new BuildPantograph();
                        pBuilder.CreatePantographPcl(pfilename, swPantograph.PantographConfigurations[cntr], BasePath + "\\Configuration\\", true);
                    }
                    MessageBox.Show(this, "Configuation settings saved.  Pantograph configuration saved to: " + sc.SaveLocation);
                    this.Close();
                }
            }
        }

        private bool AddPantoConfig(PantoControl pantoControl, ref List<CustomConfiguration> PantographConfigurations)
        {
            if (fullPagePantographUsed) return false;

            var item = new CustomConfiguration();
            item.BgDarknessFactor = Convert.ToInt32(numDarknessFactor.Value);
            item.DensityPattern1 = Convert.ToInt32(numDensityPtn1.Value);
            item.DensityPattern2 = Convert.ToInt32(numDensityPtn2.Value);
            item.InterferencePatternId = Convert.ToInt32(numIntfPtn.Value);
            item.BorderString = txtMicroPrint.Text;
            item.UseDefaultInclusionForPaperSize = chkUseFullPage.Checked;
            item.TROYmarkOn = chkTM.Checked;
            AddInclusionRegion(pantoControl, ref item);
            PantographConfigurations.Add(item);
            if (item.UseDefaultInclusionForPaperSize)
            {
                fullPagePantographUsed = true;
            }
            return true;
        }

        private bool AddInclusionRegion(PantoControl pantoControl, ref CustomConfiguration item)
        {
            item.InclusionRegion = new PantographRegionObjectType();
            if (pantoControl1.numHeight.Value > 0 && pantoControl1.numWidth.Value > 0)
            {
                item.InclusionRegion.XAnchor = Convert.ToInt32(pantoControl.numXAnchor.Value);
                item.InclusionRegion.YAnchor = Convert.ToInt32(pantoControl.numYAnchor.Value);
                item.InclusionRegion.Height = Convert.ToInt32(pantoControl.numHeight.Value);
                item.InclusionRegion.Width = Convert.ToInt32(pantoControl.numWidth.Value);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidateSettings()
        {
            if ((!chkUseFullPage.Checked) && ((pantoControl1.numHeight.Value == 0) || (pantoControl1.numWidth.Value == 0)))
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
