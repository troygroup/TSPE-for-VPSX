using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using TspeGlobals;
using Troy.Core.Licensing;
using System.Threading;

namespace TroyLicensedPrinterManager
{
    public partial class frmLicensedPrinter : Form
    {
        string LicensedPrinterFile = "";
        string LicensePath = "";
        bool SaveLicenseFile = false;
        PrinterMapping pm;
        int MaxPrinterCount = -1;
        int rowcnt = 0;
        string BasePath = "";

        public frmLicensedPrinter()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {

                }
                else if (e.ColumnIndex == 1)
                {
                    openFileDialog1.DefaultExt = "XML|xml";
                    openFileDialog1.InitialDirectory = BasePath + @"\Configuration";
                    if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        dgvPrinterList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = openFileDialog1.SafeFileName;
                        dgvPrinterList.Refresh();
                    }
                }
                else if (e.ColumnIndex == 2)
                {
                    folderBrowserDialog1.SelectedPath = BasePath + @"\Configuration";
                    if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        dgvPrinterList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = folderBrowserDialog1.SelectedPath.Remove(0,folderBrowserDialog1.SelectedPath.LastIndexOf("\\") + 1);
                    }
                }
                Thread.Sleep(200);
                dgvPrinterList.Refresh();
            }
        }

        private bool CheckLicenseStatus()
        {
            try
            {
                //BasePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                BasePath = Directory.GetCurrentDirectory();
                LicensePath = BasePath + @"\LicenseFiles";
                LicensedPrinterFile = BasePath + @"\LicenseFiles\LicensedPrinterList.txt";
                LicensingCore lcore = new Troy.Core.Licensing.LicensingCore();
                LicensingStatus lstatus = lcore.GetLicenseStatus(out MaxPrinterCount, LicensePath);

                if (lstatus == LicensingStatus.LicenseExpired)
                {
                    txtLicenseStatus.Text = "License Expired";
                    return false;
                }
                else if (lstatus == LicensingStatus.LicenseNotFound)
                {
                    txtLicenseStatus.Text = "License Not Found";
                    return false;
                }
                else if (lstatus == LicensingStatus.InvalidLicense)
                {
                    txtLicenseStatus.Text = "License File is Invalid";
                    return false;
                }
                else if (lstatus == LicensingStatus.TrialMode)
                {
                    dgvPrinterList.Enabled = true;
                    btnOk.Enabled = true;
                    txtLicenseStatus.Text = "Trial Mode";
                }
                else
                {
                    dgvPrinterList.Enabled = true;
                    btnOk.Enabled = true;
                    txtLicenseStatus.Text = lstatus.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in CheckLicenseStatus.  Error: " + ex.Message);
                txtLicenseStatus.Text = "Error Opening License";
                return false;
            }
        }

        private void LoadDataFromFile()
        {
            try
            {
                if (!File.Exists(LicensedPrinterFile))
                {
                    pm = new PrinterMapping();
                }
                else
                {
                    BinaryReader br = new BinaryReader(File.Open(LicensedPrinterFile, FileMode.Open, FileAccess.Read, FileShare.Read));
                    byte[] data = null;
                    int len = Convert.ToInt32(br.BaseStream.Length);
                    data = br.ReadBytes(len);
                    br.Close();
                    string dc = EncryptionClass.decryptStringFromBytes_AES(data);
                    byte[] bc = new UTF8Encoding(true).GetBytes(dc);
                    MemoryStream ms = new MemoryStream(bc);
                    XmlSerializer pmdser = new XmlSerializer(typeof(PrinterMapping));
                    pm = (PrinterMapping)pmdser.Deserialize(ms);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in LoadDataFromFile(). " + ex.Message);
            }

        }

        private void frmLicensedPrinter_Load(object sender, EventArgs e)
        {
            try
            {
                if (CheckLicenseStatus())
                {
                    LoadDataFromFile();
                }
                if (pm != null)
                {
                    foreach (PrinterMap pmap in pm.PrinterMapList)
                    {
                        string[] newRow = new string[] { pmap.PrinterName, pmap.TroymarkConfig, pmap.PantographPath };
                        dgvPrinterList.Rows.Add(newRow);
                    }
                    if (dgvPrinterList.Rows.Count > MaxPrinterCount)
                    {
                        dgvPrinterList.AllowUserToAddRows = false;
                        btnImportCsv.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error. " + ex.Message);
            }
        }

        private void dgvPrinterList_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            //if (dgvPrinterList.Rows.Count > MaxPrinterCount) //Disallow user to add new rows when reached row limit
            //{
            //    MessageBox.Show("You have reached the maximum allowed printer count for your license. Cannot add another printer", "Maximum Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    dgvPrinterList.AllowUserToAddRows = false;
            //}

        }

        private void dgvPrinterList_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            dgvPrinterList.AllowUserToAddRows = true;
            btnImportCsv.Enabled = true;

        }

        private void dgvPrinterList_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this entry?", "Are you sure?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                e.Cancel = true;
            }

        }

        private bool ValidateData()
        {
            List<string> prtnamelist = new List<string>();
            for (int cntr = 0; cntr < dgvPrinterList.Rows.Count; cntr++)
            {
                if (dgvPrinterList.Rows[cntr].IsNewRow)
                {
                    break;
                }

                int realcntr = cntr + 1;
                if ((dgvPrinterList.Rows[cntr].Cells.Count > 0) &&
                    (dgvPrinterList.Rows[cntr].Cells[0].Value.ToString() == ""))
                {
                    MessageBox.Show("Printer Name can not be blank.  Row: " + realcntr.ToString());
                    return false;
                }
                else if ((dgvPrinterList.Rows[cntr].Cells[1].Value == null) ||
                         (dgvPrinterList.Rows[cntr].Cells[1].Value.ToString() == ""))
                {
                    MessageBox.Show("Pantograph Configuration can not be blank.  Row: " + realcntr.ToString());
                    return false;
                }
                else if ((dgvPrinterList.Rows[cntr].Cells[2].Value == null) ||
                         (dgvPrinterList.Rows[cntr].Cells[2].Value.ToString() == ""))
                {
                    MessageBox.Show("TROYmark Configuration can not be blank.  Row: " + realcntr.ToString());
                    return false;
                }
                else if (Directory.Exists(BasePath + "\\" + dgvPrinterList.Rows[cntr].Cells[1].Value.ToString()))
                {
                    MessageBox.Show("Pantograph path not found.  Path: " + BasePath + "\\" + dgvPrinterList.Rows[cntr].Cells[1].Value.ToString());
                    return false;
                }
                else if (File.Exists(BasePath + "\\Configuration\\" + dgvPrinterList.Rows[cntr].Cells[2].Value.ToString()))
                {
                    MessageBox.Show("Troymark configuration file not found.  File: " + BasePath + "\\Configuration\\" + dgvPrinterList.Rows[cntr].Cells[2].Value.ToString());
                    return false;
                }
                else if (prtnamelist.Contains(dgvPrinterList.Rows[cntr].Cells[0].Value.ToString()))
                {
                    MessageBox.Show("Repeated printer name.  List can contain only one entry per printer name.  Row: " + cntr.ToString());
                    return false;
                }
                prtnamelist.Add(dgvPrinterList.Rows[cntr].Cells[0].Value.ToString());
            }
            return true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult userresp = MessageBox.Show("Save changes to the Licensed Printer Table.", "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (userresp == System.Windows.Forms.DialogResult.Yes)
                {
                    if (ValidateData())
                    {
                        pm.PrinterMapList.Clear();
                        for (int cntr = 0; cntr < dgvPrinterList.Rows.Count; cntr++)
                        {
                            if (dgvPrinterList.Rows[cntr].IsNewRow) break;
                            PrinterMap pmap = new PrinterMap();
                            pmap.PrinterName = dgvPrinterList.Rows[cntr].Cells[0].Value.ToString();
                            pmap.TroymarkConfig = dgvPrinterList.Rows[cntr].Cells[1].Value.ToString();
                            pmap.PantographPath = dgvPrinterList.Rows[cntr].Cells[2].Value.ToString();
                            pm.PrinterMapList.Add(pmap);
                        }
                        XmlSerializer xser = new XmlSerializer(typeof(PrinterMapping));
                        MemoryStream ms = new MemoryStream();
                        xser.Serialize(ms, pm);
                        string es = Encoding.ASCII.GetString(ms.GetBuffer());
                        es = es.TrimStart('?');
                        while ((es.Length % 256) != 0)
                        {
                            es += "\0";
                        }
                        byte[] eb = EncryptionClass.encryptStringToBytes_AES(es);
                        BinaryWriter bw = new BinaryWriter(File.Open(LicensedPrinterFile, FileMode.Create));
                        bw.Write(eb, 0, eb.Length);
                        bw.Close();
                        MessageBox.Show("Changes were saved.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Changes can not be saved. Correct the errors then attempt to save the changes again. ", "Changes not saved.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (userresp == System.Windows.Forms.DialogResult.No)
                {
                    MessageBox.Show("Changes were not saved.");
                    this.Close();
                }
                else
                {
                   
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit without saving?", "Are you sure?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnCreateLrf_Click(object sender, EventArgs e)
        {
            CreateLrf clrf = new CreateLrf();
            clrf.Show();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear the table?", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                dgvPrinterList.Rows.Clear();
                pm.PrinterMapList.Clear();
                dgvPrinterList.AllowUserToAddRows = true;
                btnImportCsv.Enabled = true;
            }
        }

        private void dgvPrinterList_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPrinterList.Rows.Count > MaxPrinterCount) //Disallow user to add new rows when reached row limit
            {
                MessageBox.Show("You have reached the maximum allowed printer count for your license. Cannot add another printer", "Maximum Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgvPrinterList.AllowUserToAddRows = false;
                btnImportCsv.Enabled = false;
            }

        }

        private void btnImportCsv_Click(object sender, EventArgs e)
        {
            openFileDialog2.DefaultExt = "csv";
            openFileDialog2.Filter = "Comma Delimited Files (*.csv) | *.csv";
            openFileDialog2.CheckFileExists = true;
            openFileDialog2.Multiselect = false;
            openFileDialog2.Title = "Licensed Printer List (CSV)";
            openFileDialog2.FileName = "";
            openFileDialog2.InitialDirectory = BasePath;
            if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TextReader tr = new StreamReader(openFileDialog2.FileName);
                string inline = tr.ReadLine();
                List<string> ErrorList = new List<string>();
                List<PrinterMap> ImportString = new List<PrinterMap>();
                int fieldcntr = 0;
                int rowcntr = 0;

                while (inline != null)
                {
                    if (inline.StartsWith(@"//"))
                    {
                        //Ignore this.  its a comment
                    }
                    else if (inline == "")
                    {
                        //Ignore this. Blank
                    }
                    else
                    {
                        string pName = "", sName = "", tmConfig = "", pPath = "";
                        fieldcntr = 0;
                        foreach (string str in inline.Split(','))
                        {
                            if (fieldcntr == 0)
                            {
                                pName = str;
                            }
                            else if (fieldcntr == 1)
                            {
                                sName = str;
                            }
                            else if (fieldcntr == 2)
                            {
                                tmConfig = str;
                            }
                            else if (fieldcntr == 3)
                            {
                                pPath = str;
                            }
                            fieldcntr++;

                        }
                        if ((pName == "") && (sName == "") && (tmConfig == "") && (pPath == ""))
                        {
                            //Ignore, blank line
                        }
                        else
                        {
                            if (pName == "")
                            {
                                ErrorList.Add("Error in line " + fieldcntr.ToString() + ".  Printer name is blank.");
                            }
                            else if (tmConfig == "")
                            {
                                ErrorList.Add("Error in line " + fieldcntr.ToString() + ".  Troymark Configuration is blank.");
                            }
                            else if (pPath == "")
                            {
                                ErrorList.Add("Error in line " + fieldcntr.ToString() + ".  Pantograph Configuration is blank.");
                            }
                            else
                            {
                                PrinterMap pmap = new PrinterMap();
                                pmap.PrinterName = pName;
                                pmap.SiteName = sName;
                                pmap.TroymarkConfig = tmConfig;
                                pmap.PantographPath = pPath;
                                ImportString.Add(pmap);
                                rowcntr++;
                            }
                        }


                    }
                    inline = tr.ReadLine();
                }
                tr.Close();
                int totalcnt = rowcntr + dgvPrinterList.Rows.Count - 1;  //Subtract 1 to account for the add row.
                if (totalcnt > MaxPrinterCount)
                {
                    ErrorList.Insert(0, "Number of entries in CSV plus number of entries already in the list (" + totalcnt.ToString() + ") exceeds maximum allowed amount (" + MaxPrinterCount.ToString() + ").");
                }
                bool Cont = true;
                if (ErrorList.Count > 0)
                {
                    Errors err = new Errors();
                    err.ErrorList = ErrorList;
                    err.ShowDialog();
                    Cont = !err.CancelImport;
                }
                if (rowcntr < 1)
                {
                    Cont = false;
                }
                if (Cont)
                {
                    int importedcntr = 0;
                    //foreach (PrinterMap pmap in pm.PrinterMapList)
                    foreach (PrinterMap pmap in ImportString)
                    {
                        string[] newRow = new string[] { pmap.PrinterName, pmap.TroymarkConfig, pmap.PantographPath };
                        dgvPrinterList.Rows.Add(newRow);
                        importedcntr++;
                        if (dgvPrinterList.Rows.Count > MaxPrinterCount)
                        {
                            dgvPrinterList.AllowUserToAddRows = false;
                            btnImportCsv.Enabled = false;
                            if (importedcntr < rowcntr)
                            {
                                int diff = rowcntr - importedcntr;
                                MessageBox.Show("Maximum number of printers reached.  " + diff.ToString() + " entries were not imported.");
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PatternTest pt = new PatternTest();
            pt.StartPosition = FormStartPosition.CenterParent;
            pt.BaseFilePath = BasePath;
            pt.Show();
        }

        private void btnPantograph_Click(object sender, EventArgs e)
        {
            PantoConfig pc = new PantoConfig();
            pc.BasePath = BasePath;
            pc.StartPosition = FormStartPosition.CenterParent;
            pc.ShowDialog();
        }

        private void dgvPrinterList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            rowcnt = dgvPrinterList.AllowUserToAddRows ? dgvPrinterList.Rows.Count - 1 : dgvPrinterList.Rows.Count;
            txtPrinterCount.Text = String.Format("{0}/{1}", rowcnt, MaxPrinterCount);
        }

        private void dgvPrinterList_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            rowcnt = dgvPrinterList.AllowUserToAddRows ? dgvPrinterList.Rows.Count - 1 : dgvPrinterList.Rows.Count;
            txtPrinterCount.Text = String.Format("{0}/{1}", rowcnt, MaxPrinterCount);
        }
    }
}
