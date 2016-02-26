using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using Troy.Core.Licensing;
using TspeGlobals;

namespace TroyLrfUtility
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Must include the Company name.");
                Console.WriteLine("Valid arguments are (<> respresent data that must be included with argument):");
                Console.WriteLine("   FIRST:<first name> - optional, first name of contact for license file.");
                Console.WriteLine("   LAST:<last name> - optional, last name of contact for license file.");
                Console.WriteLine("   COMPANY:<company name> - company name of contact for license file.");
                Console.WriteLine("   EMAIL:<email address> - optional, email address of contact for license file.");
                Console.WriteLine("   TITLE:<title> - optional, title of contact for license file.");
                Console.WriteLine("   PHONE:<phone number> - optional, phone number of contact for license file.");
                Console.WriteLine("   FILENAME:<license request file name> - optional, File name of the license request file if not using the default.");
                return;
            }

            string tempLoc = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string BasePath = System.IO.Path.GetDirectoryName(tempLoc);


            string FirstName = "", LastName = "", Company = "", Email = "", Phone = "", Title = "";

            string FileName = "TroyLicenseReq.lrf";

            for (int cntr = 0; cntr < args.Length; cntr++)
            {
                if (args[cntr].StartsWith("FIRST:"))
                {
                    FirstName = args[cntr].Substring(5);
                }
                else if (args[cntr].StartsWith("LAST:"))
                {
                    LastName = args[cntr].Substring(4);
                }
                else if (args[cntr].StartsWith("COMPANY:"))
                {
                    Company = args[cntr].Substring(8);
                }
                else if (args[cntr].StartsWith("EMAIL:"))
                {
                    Email = args[cntr].Substring(6);
                }
                else if (args[cntr].StartsWith("PHONE:"))
                {
                    Phone = args[cntr].Substring(6);
                }
                else if (args[cntr].StartsWith("TITLE:"))
                {
                    Title = args[cntr].Substring(6);
                }
                else if (args[cntr].StartsWith("FILENAME:"))
                {
                    FileName = args[cntr].Substring(9);
                }
            }

            Troy.Core.Licensing.LicenseRequestInfo lri = new Troy.Core.Licensing.LicenseRequestInfo();
            lri.CompanyName = Company;
            lri.Domain = Environment.UserDomainName;
            lri.EmailAddress = Email;
            lri.FirstName = FirstName;
            lri.LastName = LastName;
            lri.MachineName = Environment.MachineName.ToLower();
            lri.OSPlatform = Environment.OSVersion.Platform.ToString();
            lri.OSVersion = Environment.OSVersion.VersionString;
            lri.PhoneNumber = Phone;
            lri.Product = "TSPE";
            lri.SPlevel = Environment.OSVersion.ServicePack;
            lri.Title = Title;

            byte[] encryptedBytes = Troy.Core.Licensing.LicensingCore.Encrypt(lri, Troy.Core.Licensing.LicenseType.Unified);
            BinaryWriter bw = new BinaryWriter(File.Open(BasePath + @"/" + FileName, FileMode.Create));
            bw.Write(encryptedBytes);
            bw.Flush();
            bw.Close();

            Console.WriteLine("License file created. " + BasePath + @"/" + FileName);

        }
    }
}
