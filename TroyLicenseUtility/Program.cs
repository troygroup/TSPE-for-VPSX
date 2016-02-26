using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using TspeGlobals;
using Troy.Core.Licensing;

namespace TroyLicenseUtility
{
    class Program
    {
        static string LicensedPrinterFile = "";
        static string LicensePath = "";
        static bool SaveLicenseFile = false;

        public static void Main(string[] args)
        {
            try
            {
                string tempLoc = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string BasePath = System.IO.Path.GetDirectoryName(tempLoc);
                LicensePath = BasePath + @"/LicenseFiles";
                LicensedPrinterFile = BasePath + @"/LicenseFiles/LicensedPrinterList.txt";
                string CsvFilename = "";
                string PrinterName = "";
                string SiteName = "";
                string TroymarkConfig = "";
                string PantographConfig = "";
                bool ShowList = false;
                //	string DumpToFile = "";
                bool ClearList = false;
                bool DeleteEntry = false;
                string DeletePrinterName = "";
                if (args.Length < 1)
                {
                    Console.WriteLine("Valid Arguments:");
                    Console.WriteLine("  For adding a single printer to the list:");
                    Console.WriteLine("      PRT:<printer name>");
                    Console.WriteLine("      TM:<name of Troymark config file located in Configuration folder>");
                    Console.WriteLine("      PG:<name of pantograph config folder located in Configuration folder>");
                    Console.WriteLine("      SITE:<name of site for mutliple license customers, optional>");
                    Console.WriteLine("  CSV:<csv filename of the file containing the printer list>");
                    Console.WriteLine("  /c - Clears the existing printer list.");
                    Console.WriteLine("  SHOW: - shows the current list of printers.");
                    Console.WriteLine("  DEL:<printer name of printer to be deleted>");
                    return;
                }
                else
                {
                    for (int cntr = 0; cntr < args.Length; cntr++)
                    {
                        if (args[cntr].StartsWith("PRT:"))
                        {
                            PrinterName = args[cntr].Substring(4);
                        }
                        else if (args[cntr].StartsWith("SITE:"))
                        {
                            SiteName = args[cntr].Substring(5);
                        }
                        else if (args[cntr].StartsWith("TM:"))
                        {
                            TroymarkConfig = args[cntr].Substring(3);
                        }
                        else if (args[cntr].StartsWith("PG:"))
                        {
                            PantographConfig = args[cntr].Substring(3);
                        }
                        else if (args[cntr].StartsWith("CSV:"))
                        {
                            CsvFilename = args[cntr].Substring(4);
                        }
                        else if (args[cntr].StartsWith(@"/c"))
                        {
                            ClearList = true;
                        }
                        else if (args[cntr].StartsWith("DEL:"))
                        {
                            DeleteEntry = true;
                            DeletePrinterName = args[cntr].Substring(4);
                        }
                        else if (args[cntr].StartsWith("SHOW:"))
                        {
                            ShowList = true;
                            //if (args[cntr].Length > 5)
                            //{
                            //	DumpToFile = args[cntr].Substring(5);
                            //	DumpToFile = DumpToFile.Trim();
                            //}
                        }
                        else
                        {
                            Console.WriteLine("Invalid argument. Does not begin with PRT:, SITE:, TM: or PG:. Argument: " + args[cntr]);
                            return;
                        }

                    }
                }
                LicensingCore lcore = new Troy.Core.Licensing.LicensingCore();
                int PrinterCnt = -1;
                LicensingStatus lstatus = lcore.GetLicenseStatus(out PrinterCnt, LicensePath);
                //LicensingStatus lstatus = lcore.GetLicenseStatus(BasePath + @"/jan11blicense.licx","petite",out PrinterCnt);

                if (lstatus == LicensingStatus.LicenseExpired)
                {
                    Console.WriteLine("License Expired.  Can not conitinue.");
                    return;
                }
                else if (lstatus == LicensingStatus.LicenseNotFound)
                {
                    Console.WriteLine("License Not Found.  Can not conitinue.");
                    return;
                }
                else if (lstatus == LicensingStatus.InvalidLicense)
                {
                    Console.WriteLine("License File is Invalid.  Can not conitinue.");
                    return;
                }
                PrinterMapping pm;
                PrinterMap pmap = new PrinterMap();
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

                if (ShowList)
                {
                    Console.WriteLine("Showing Licensed Printer List");
                    foreach (PrinterMap pmp in pm.PrinterMapList)
                    {
                        Console.WriteLine(pmp.PrinterName + "   " + pmp.PantographPath + "   " + pmp.TroymarkConfig + "   " + pmp.SiteName);
                    }
                    Console.WriteLine("End of List");

                    return;
                }

                int PrinterOvercount = 0;
                int PrinterAddedCount = 0;
                if (DeleteEntry)
                {
                    if (pm.DeleteEntry(DeletePrinterName))
                    {
                        SaveLicenseFile = true;
                        Console.WriteLine(DeletePrinterName + " removed from the license list.");
                    }
                    else
                    {
                        Console.WriteLine("Printer not deleted.  " + DeletePrinterName + " not found in the license list.");
                    }
                    //return;
                }
                else
                {
                    if (ClearList)
                    {
                        pm = new PrinterMapping();
                        SaveLicenseFile = true;
                    }

                    if (CsvFilename != "")
                    {
                        if (!File.Exists(CsvFilename))
                        {
                            Console.WriteLine("CSV File Not Found.  Filename: " + CsvFilename);
                            return;
                        }
                        TextReader tr = new StreamReader(CsvFilename);
                        string inline = tr.ReadLine();
                        while (inline != null)
                        {
                            if (inline.StartsWith(@"//"))
                            {
                                //Ignore this.  its a comment
                            }
                            else
                            {
                                int fieldcntr = 0;
                                foreach (string str in inline.Split(','))
                                {
                                    if (fieldcntr == 0)
                                    {
                                        pmap.PrinterName = str;
                                    }
                                    else if (fieldcntr == 1)
                                    {
                                        pmap.SiteName = str;
                                    }
                                    else if (fieldcntr == 2)
                                    {
                                        pmap.TroymarkConfig = str;
                                    }
                                    else if (fieldcntr == 3)
                                    {
                                        pmap.PantographPath = str;
                                    }
                                    fieldcntr++;
                                }
                                if (pmap.PrinterName != "")
                                {
                                    if ((PrinterCnt > -1) && (pm.PrinterMapList.Count < PrinterCnt))
                                    {
                                        pm.AddNewEntry(pmap);
                                        PrinterAddedCount++;
                                        pmap = new PrinterMap();
                                    }
                                    else if (pm.EntryExist(pmap.PrinterName))
                                    {
                                        pm.AddNewEntry(pmap);
                                        PrinterAddedCount++;
                                        pmap = new PrinterMap();
                                    }
                                    else
                                    {
                                        PrinterOvercount++;
                                    }
                                }
                            }
                            inline = tr.ReadLine();
                        }
                        tr.Close();
                        SaveLicenseFile = true;
                    }
                    else
                    {
                        pmap.PrinterName = PrinterName;
                        pmap.SiteName = SiteName;
                        pmap.TroymarkConfig = TroymarkConfig;
                        pmap.PantographPath = PantographConfig;
                        if ((PrinterCnt > -1) && (pm.PrinterMapList.Count < PrinterCnt))
                        {
                            pm.AddNewEntry(pmap);
                            PrinterAddedCount++;
                        }
                        else if (pm.EntryExist(PrinterName))
                        {
                            pm.AddNewEntry(pmap);
                            PrinterAddedCount++;
                        }
                        else
                        {
                            PrinterOvercount++;
                        }
                        SaveLicenseFile = true;
                    }
                }
                if (PrinterOvercount > 0)
                {
                    Console.WriteLine("Over the printer count limit. " + PrinterOvercount + " printer(s) not added to licensed printer list.");

                }

                if (SaveLicenseFile == true)
                {
                    if ((pm.PrinterMapList.Count > 0) || (DeleteEntry) || (ClearList))
                    {
                        XmlSerializer xser = new XmlSerializer(typeof(PrinterMapping));

                        //No Encryption
                        //TextWriter writer = new StreamWriter(LicensedPrinterFile);
                        //xser.Serialize(writer,pm);
                        //writer.Close();

                        //Encryption
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
                    }
                    if (ClearList)
                    {
                        Console.WriteLine("Printer List Cleared");
                    }
                    Console.WriteLine("Printers added count: " + PrinterAddedCount.ToString());
                    Console.WriteLine("Current list size: " + pm.PrinterMapList.Count.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
