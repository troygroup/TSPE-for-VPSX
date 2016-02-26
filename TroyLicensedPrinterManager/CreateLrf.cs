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
    public partial class CreateLrf : Form
    {
        public CreateLrf()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtFirstName.Text == "")
            {
                MessageBox.Show("First Name is a required field");
                return;
            }
            else if (txtLastName.Text == "")
            {
                MessageBox.Show("Last Name is a required field");
                return;
            }
            else if (txtEmailAddress.Text == "")
            {
                MessageBox.Show("Email Address is a required field");
                return;
            }
            else if (txtCompanyName.Text == "")
            {
                MessageBox.Show("Company Name is a required field");
                return;
            }

            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string SavePath = folderBrowserDialog1.SelectedPath;

                string FileName = SavePath + "\\LicenseInfo.lrf";

                Troy.Core.Licensing.LicenseRequestInfo lri = new Troy.Core.Licensing.LicenseRequestInfo();
                lri.CompanyName = txtCompanyName.Text;
                lri.Domain = Environment.UserDomainName;
                lri.EmailAddress = txtEmailAddress.Text;
                lri.FirstName = txtFirstName.Text;
                lri.LastName = txtLastName.Text;
                lri.MachineName = Environment.MachineName.ToLower();
                lri.OSPlatform = Environment.OSVersion.Platform.ToString();
                lri.OSVersion = Environment.OSVersion.VersionString;
                lri.PhoneNumber = txtPhoneNumber.Text;
                lri.Product = "TSPE";
                lri.SPlevel = Environment.OSVersion.ServicePack;
                lri.Title = txtTitle.Text;

                byte[] encryptedBytes = Troy.Core.Licensing.LicensingCore.Encrypt(lri, Troy.Core.Licensing.LicenseType.Unified);
                BinaryWriter bw = new BinaryWriter(File.Open(FileName, FileMode.Create));
                bw.Write(encryptedBytes);
                bw.Flush();
                bw.Close();
                MessageBox.Show("License file was created.  Please send this file to licensing@troygroup.com");
                this.Close();
            }
        }
    }
}
