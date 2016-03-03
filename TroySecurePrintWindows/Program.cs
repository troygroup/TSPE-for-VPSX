using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Troy.Core.Licensing;
using System.IO;
using TspeGlobals;
using System.Xml.Serialization;
using System.Diagnostics;
namespace TroySecurePrintWindows
{
    class Program
    {
        private static Object thisLock = new Object();
        
        //COnfiguration file
        static string DefaultInputParamConfigName = "TspConfiguration.xml";
        static TspeInputParameters tip = null;
        static byte[] inputBuffer;
        static int bufferSize = 0;
        static int enterPclLocation;

        //Pathes and licenses
        static string BasePath = "";
        static string LicensePath = "";
        static string LicensedPrinterFile = @"LicensedPrinterList.txt";
        static bool InvalidPrinterLicense = false;
        static string InvalidLicenseMessage = "";

        //The input arguments
        static string inputFileName = "";
        static string outputFileName = "";
        static string inputParamFileName = "";
        static string destPrinterName = "";
        static bool showConsoleMsgs = false;
        static bool TraceOnCmdLine = false;
        static bool ErrOnPrintCmdLine = false;
        static bool ErrLogOnCmdLine = false;
        static bool FakeAFault = false;

        //Trace and error
        static string DefaultTraceFileName = "";
        static string DefaultErrorFileName = "";
        static FileInfo traceFile;
        static StreamWriter traceWrite;
        static bool traceEnabled = false;
        static FileInfo errorFile;
        static StreamWriter errorWrite;
        static bool errorEnabled = false;
        static string TroymarkErrorMsg = "";

        //Pantograph/Troymark
        static bool SearchForTroyCmds = false;
        static int PantographValue = 0;
        static Dictionary<int, string> TmPerPage = new Dictionary<int, string>();
        static int TmPattern = 1;
        static int TmInclusionXAnchor = 0;
        static int TmInclusionYAnchor = 0;
        static int TmInclusionWidth = 4800;
        static int TmInclusionHeight = 6400;
        static int TmExclusionXAnchor = -1;
        static int TmExclusionYAnchor = -1;
        static int TmExclusionWidth = -1;
        static int TmExclusionHeight = -1;
        static TroymarkSettings TmSettings;// = new TroymarkSettings();
        static string TroymarkData = "";

        static Dictionary<int, string> TroymarkDataPerPage = new Dictionary<int, string>();

        //Licensing
        static bool UnlicensedPrinter = true;
        static string TroymarkLicenseErrorText = "";

        static PrinterMapping prtMap = null;
        private enum EventPointType
        {
            epInsert = 0,
            epRemove = 1,
            epSubstitute = 2,
            epInsertPoint = 3,
            epPageEnd = 4,
            epUELLocation = 5,
            epEndOfJob = 6,
        }
        private struct EventPoints
        {
            public int PageNumber;
            public int Location;
            public EventPointType EventType;
            public int EventLength;
            public EventPoints(int pageNumber, int location, EventPointType eventType, int eventLength)
            {
                PageNumber = pageNumber;
                Location = location;
                EventType = eventType;
                EventLength = eventLength;
            }
        }
        static List<EventPoints> fileEventPoints = new List<EventPoints>();

        static byte[] InsertAfterPg = null;


        public static int Main(string[] args)
        {
            int processID = Process.GetCurrentProcess().Id;
            
            //Verify number of arguments is at least 3
            if (args.Length < 3)
            {
                Console.WriteLine("Invalid arguments.  Arguments must include an input file (IN:<filename>), an output file (OUT:<filename>) and a printer name (PRT:<printername)");
                return 1;
            }

            //Set the base path, license file path and licensed printer file name
            //BasePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            BasePath = Directory.GetCurrentDirectory();
            LicensedPrinterFile = BasePath + @"\LicenseFiles\" + LicensedPrinterFile;
            LicensePath = BasePath + @"\LicenseFiles\";

            //Create an instance of the licensing core in order to determine if the license is active
            LicensingCore lcore = new Troy.Core.Licensing.LicensingCore();
            int PrinterCnt = -1;

            lock (thisLock)
            {
                LicensingStatus lstatus = lcore.GetLicenseStatus(out PrinterCnt, LicensePath);
                if (!((lstatus == LicensingStatus.FullyLicensed) || (lstatus == LicensingStatus.TrialMode)))
                {
                    InvalidPrinterLicense = true;
                    InvalidLicenseMessage = "License Status: " + lstatus.ToString();
                }
            }

            ParseTheArgs(args);

            //Validate the Arguments are valid
            int val = ValidateTheArgs();
            if (val != 0)
            {
                return val;
            }

            //Deserialize the XML input parameters file	
            try
            {
                XmlSerializer dser = new XmlSerializer(typeof(TspeInputParameters));
                FileStream fs = new FileStream(inputParamFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                tip = (TspeInputParameters)dser.Deserialize(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                LogAnError("Error reading in configuraiton file. Error: " + ex.Message);
                return 7;
            }

            //Trace and error 
            if (TraceOnCmdLine)
            {
                tip.DiagSettings.TraceFile = BasePath + @"\" + DefaultTraceFileName;
            }
            if (ErrLogOnCmdLine)
            {
                tip.DiagSettings.ErrorLogFile = BasePath + @"\" + DefaultErrorFileName;
            }
            if (ErrOnPrintCmdLine)
            {
                tip.IncludeCodeOnPrintout = true;
            }
            if (!tip.TroyCommandsIncluded)
            {
                PantographValue = 1;  //Quick way to make this work.		
            }
            EnableTraceLog();
            EnableErrorLog();

            //Is license file found
            if (!File.Exists(LicensedPrinterFile))
            {
                UnlicensedPrinter = true;
                TroymarkLicenseErrorText = "*** UNLICENSED PRINTER *** Printer: " + destPrinterName;
                tip.DefaultPantographLocation = BasePath + @"\Configuration\" + tip.DefaultPantographLocation;
            }
            else
            {
                DeterminePrinterLicensed();
            }
            if (TmSettings == null)
            {
                SetupDefaultTroymark();
            }
            if (TroymarkLicenseErrorText != "")
            {
                TmSettings.TroymarkStaticText = TroymarkLicenseErrorText;
            }
            else if (TroymarkErrorMsg != "")
            {
                TmSettings.TroymarkStaticText = TroymarkErrorMsg;
            }

            //Open the input file and load it into a byte array
            BinaryReader binReader = new BinaryReader(File.Open(inputFileName, FileMode.Open, FileAccess.Read, FileShare.Read));
            bufferSize = (int)binReader.BaseStream.Length;
            inputBuffer = new Byte[bufferSize];
            inputBuffer = binReader.ReadBytes(Convert.ToInt32(bufferSize));
            binReader.Close();

            //Find the Enter PCL string in the PJL header
            enterPclLocation = FindPjlEnterPcl();

            //Look for the Troymark in the PJL
            if ((tip.PjlTroymarkData) && (enterPclLocation > -1))
            {
                if (tip.PjlTmDataTags.Count < 1)
                {
                    LogAnError("PJL Troymark Data flag is enabled but PJL tags do not exists in the configuration.");
                }
                else
                {
                    if (traceEnabled) WriteToTrace("Search PJL Header for Troymark data....");
                    SearchPjlForTm();
                    if (TroymarkData != "")
                    {
                        if (traceEnabled) WriteToTrace("Troymark data found:  " + TroymarkData);
                    }
                    else
                    {
                        if (traceEnabled) WriteToTrace("Troymark data not found in PJL header");
                    }
                }
            }

            //Call ReadInputFile function to parse through the data and find the insert points and the end of page points
            if (enterPclLocation < 0)
            {
                LogAnError("ENTER PCL String not found in input file.");
            }
            else
            {
                if (!ReadInputFile(enterPclLocation))
                {
                    LogAnError("READ INPUT FILE FAILED");
                    return 7;
                }
            }

            //TEMP FAKE AN ERROR
            if (FakeAFault)
            {
                LogAnError("THIS IS A TEST ERROR");
            }

            //Call WriteOutPcl to write the data out the output file
            if (!WriteOutPcl(outputFileName))
            {
                LogAnError("WRITE TO OUTPUT FILE FAILED\n\r");
                return 8;
            }

            //Close the Trace Log
            if (traceEnabled) WriteToTrace("Process complete.");
            if (traceWrite != null)
            {
                traceWrite.Flush();
                traceWrite.Close();
            }

            if (errorWrite != null)
            {
                errorWrite.Flush();
                errorWrite.Close();
            }

            if (showConsoleMsgs)
            {
                Console.WriteLine("Process Complete.");
            }            


            return 0;

        }

        static void SearchPjlForTm()
        {
            TroymarkData = "";
            string PjlString = "";
            PjlString = new UTF8Encoding(true).GetString(inputBuffer, 0, enterPclLocation - 26);

            int index, index2;
            string capString = "";

			foreach (string str in tip.PjlTmDataTags)
			{
				if (traceEnabled) WriteToTrace("In PJL header, looking for " + str);
	            index = PjlString.IndexOf(str);
	            while (index > -1)
	            {
	                // look for trailing "
                    index2 = PjlString.IndexOf("\"", index + str.Length);
	                if (index2 > index)
	                {
                        int index3 = PjlString.IndexOf(",", index + 1);
                        if (index3 > -1)
                        {
                            // JLD changed to tryparse

                            int pagenum = -1;
                            bool result= Int32.TryParse(PjlString.Substring(index + str.Length,index3 - (index+str.Length)), out pagenum);
                            if (result == true)
                            {
                                string tmdata = PjlString.Substring(index3 + 1, index2 - (index3 + 1));
                                if (TroymarkDataPerPage.ContainsKey(pagenum))
                                {
                                    TroymarkDataPerPage[pagenum] += tmdata;
                                }
                                else
                                {
                                    TroymarkDataPerPage.Add(pagenum, tmdata);
                                }
                            }
                            else
                            {
                                capString = PjlString.Substring(index + str.Length, index2 - (index + str.Length));
                                TroymarkData += capString;
                            }
                        }
                        else
                        {
                            capString = PjlString.Substring(index + str.Length, index2 - (index + str.Length));
                            TroymarkData += capString;
                        }
	                }
                    index = PjlString.IndexOf(str,index+1);
	            }
			}
			
			if (traceEnabled) WriteToTrace("Troymark data found in PJL: "  + TroymarkData);
        }


        static int FindPjlEnterPcl()
        {
            const int pjlEnterLangLength = 23;
            //@PJL ENTER LANGUAGE=PCL
            byte[] pjlEnterLang = new byte[pjlEnterLangLength] { 0x40, 0x50, 0x4A, 0x4C, 0x20, 0x45, 0x4E, 0x54, 0x45, 0x52, 0x20, 0x4C, 0x41, 0x4E, 0x47, 0x55, 0x41, 0x47, 0x45, 0x3D, 0x50, 0x43, 0x4C };

            bool continueLoop = true;
            int matchCntr = 0, cntr = 0;

            int returnValue = -1;

            while ((cntr < bufferSize) && (continueLoop))
            {
                if (inputBuffer[cntr] == pjlEnterLang[matchCntr])
                {
                    matchCntr++;
                    if (matchCntr == pjlEnterLangLength)
                    {
                        continueLoop = false;
                        cntr = cntr + 4;
                        returnValue = cntr;
                    }
                }
                else
                {
                    matchCntr = 0;
                }
                cntr++;
            }
            //Quick way to do this for now.  
            // Need to also check for //@PJL ENTER LANGUAGE = PCL
            if (returnValue < 0)
            {
                const int pjlEnterLangLengthSpaces = 25;
                //@PJL ENTER LANGUAGE = PCL
                byte[] pjlEnterLangSpaces = new byte[pjlEnterLangLengthSpaces] { 0x40, 0x50, 0x4A, 0x4C, 0x20, 0x45, 0x4E, 0x54, 0x45, 0x52, 0x20, 0x4C, 0x41, 0x4E, 0x47, 0x55, 0x41, 0x47, 0x45, 0x20, 0x3D, 0x20, 0x50, 0x43, 0x4C };
                continueLoop = true;
                matchCntr = 0;
                cntr = 0;
                bufferSize = inputBuffer.Length;

                returnValue = -1;

                while ((cntr < bufferSize) && (continueLoop))
                {
                    if (inputBuffer[cntr] == pjlEnterLangSpaces[matchCntr])
                    {
                        matchCntr++;
                        if (matchCntr == pjlEnterLangLengthSpaces)
                        {
                            continueLoop = false;
                            cntr = cntr + 4;
                            returnValue = cntr;
                        }
                    }
                    else
                    {
                        matchCntr = 0;
                    }
                    cntr++;
                }


            }


            return returnValue;
        }
        

        private static void SetupDefaultTroymark()
        {
            string tmFile = BasePath + @"\Configuration\" + tip.DefaultTroymarkConfigFile;
            if ((tip.DefaultTroymarkConfigFile != "") &&
                (File.Exists(tmFile)))
            {
                try
                {
                    XmlSerializer dsertm = new XmlSerializer(typeof(TroymarkSettings));
                    FileStream fstm = new FileStream(tmFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                    TmSettings = (TroymarkSettings)dsertm.Deserialize(fstm);
                    fstm.Close();
                }
                catch (Exception ex)
                {
                    LogAnError("Error reading default Troymark configuraiton file. Error: " + ex.Message);
                    TroymarkErrorMsg = "ERROR READING DEFAULT TROYMARK CONFIGURATION";
                    TmSettings = new TroymarkSettings();
                }
            }
            else
            {
                TmSettings = new TroymarkSettings();
                TroymarkErrorMsg = "ERROR FINDING DEFAULT TROYMARK CONFIGURATION";
            }

        }

        private static void DeterminePrinterLicensed()
        {
            try
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
                prtMap = (PrinterMapping)pmdser.Deserialize(ms);

                if (prtMap == null)
                {
                    LogAnError("Error reading Licensed Printer Mapping file. Name: " + LicensedPrinterFile);
                    UnlicensedPrinter = true;
                    TroymarkLicenseErrorText = "*** UNLICENSED PRINTER, ERROR READING LICENSE PRINTER FILE *** Name: " + LicensedPrinterFile;
                    tip.DefaultPantographLocation = BasePath + @"\Configuration\" + tip.DefaultPantographLocation;
                    //return 12;	
                }
            }
            catch (Exception ex)
            {
                LogAnError("Exception reading Licensed Printer Mapping file. Error: " + ex.Message);
                UnlicensedPrinter = true;
                TroymarkLicenseErrorText = "*** UNLICENSED PRINTER, ERROR READING LICENSE PRINTER FILE *** Name: " + LicensedPrinterFile;
                tip.DefaultPantographLocation = BasePath + @"\Configuration\" + tip.DefaultPantographLocation;
                //return 12;

            }

            PrinterMap pm = new PrinterMap();
            if (PrinterInList(ref pm))
            {
                UnlicensedPrinter = false;
                tip.DefaultPantographLocation = BasePath + @"\Configuration\" + pm.PantographPath;
                string tmFile = BasePath + @"\Configuration\" + pm.TroymarkConfig;
                if (!File.Exists(tmFile))
                {
                    LogAnError("Troymark Configuration file not found. Printer: " + destPrinterName + " File: " + tmFile);
                    TroymarkLicenseErrorText = "*** ERROR: TROYMARK CONFIGURATION NOT FOUND ***";
                }
                else
                {
                    try
                    {
                        XmlSerializer dsertm = new XmlSerializer(typeof(TroymarkSettings));
                        FileStream fstm = new FileStream(tmFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                        TmSettings = (TroymarkSettings)dsertm.Deserialize(fstm);
                        fstm.Close();
                    }
                    catch (Exception ex)
                    {
                        LogAnError("Error reading in Troymark configuraiton file. Error: " + ex.Message);
                        TroymarkErrorMsg = "ERROR READING TROYMARK CONFIGURATION";
                    }
                }
            }
            else
            {
                UnlicensedPrinter = true;
                TroymarkLicenseErrorText = "*** UNLICENSED PRINTER *** Printer: " + destPrinterName;
                tip.DefaultPantographLocation = BasePath + @"\Configuration\" + tip.DefaultPantographLocation;
            }


        }

        static private bool PrinterInList(ref PrinterMap pm)
        {
            foreach (PrinterMap pmap in prtMap.PrinterMapList)
            {
                if (pmap.PrinterName == destPrinterName)
                {
                    pm = pmap;
                    return true;
                }
            }

            return false;
        }


        //Parse the Arguments
        private static void ParseTheArgs(string[] args)
        {
            //Parse the Args
            for (int argcntr = 0; argcntr < args.Length; argcntr++)
            {
                if (args[argcntr].StartsWith("IN:"))
                {
                    inputFileName = args[argcntr].Substring(3);
                }
                else if (args[argcntr].StartsWith("OUT:"))
                {
                    outputFileName = args[argcntr].Substring(4);
                }
                else if (args[argcntr].StartsWith("CFG:"))
                {
                    inputParamFileName = BasePath + @"\" + args[argcntr].Substring(4);
                }
                else if (args[argcntr].StartsWith("PRT:"))
                {
                    destPrinterName = args[argcntr].Substring(4);
                }
                else if (args[argcntr].StartsWith(@"/v"))
                {
                    showConsoleMsgs = true;
                }
                else if (args[argcntr].StartsWith(@"/p"))
                {
                    ErrOnPrintCmdLine = true;
                }
                else if (args[argcntr].StartsWith(@"/t"))
                {
                    TraceOnCmdLine = true;
                    DefaultTraceFileName = "Trace" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";
                }
                else if (args[argcntr].StartsWith(@"/e"))
                {
                    ErrLogOnCmdLine = true;
                    DefaultErrorFileName = "Error" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";
                }
                else if (args[argcntr].StartsWith(@"/f"))
                {
                    FakeAFault = true;
                }
            }
        }

        //Verify the args are valid
        private static int ValidateTheArgs()
        {
            //VERIFY THE ARGS ARE VALID
            if (inputFileName == "")
            {
                LogAnError("Input File name not found in argument list. Format - IN:<filename>");
                return 2;
            }
            else if (outputFileName == "")
            {
                LogAnError("Output File name not found in argument list. Format - OUT:<filename>");
                return 3;
            }
            else if (inputParamFileName == "")
            {
                //This argument is optional
            }
            else if (destPrinterName == "")
            {
                LogAnError("Destination Printer name not found in argument list. Format - PRT:<printername>");
                return 4;
            }
            else if (!File.Exists(inputFileName))
            {
                LogAnError("Input File not found.  Name: " + inputFileName);
                return 5;
            }

            if (inputParamFileName == "")
            {
                inputParamFileName = BasePath + @"\" + DefaultInputParamConfigName;
            }

            if (!File.Exists(inputParamFileName))
            {
                LogAnError("Input Parameters File not found.  Name: " + inputParamFileName);
                return 6;
            }
            return 0;
        }

        static void LogAnError(string ErrorString)
        {
            if (showConsoleMsgs)
            {
                Console.WriteLine(ErrorString);
            }
            if (traceEnabled)
            {
                WriteToTrace("ERROR: " + ErrorString);
            }
            if (errorEnabled)
            {
                errorWrite.WriteLine("IN FILE: " + inputFileName + " PRT: " + destPrinterName + "[" + DateTime.Now.ToString() + "]  " + ErrorString);
            }
            if ((tip != null) && (tip.IncludeCodeOnPrintout))
            {
                TroymarkErrorMsg = " " + ErrorString + " ";
            }
        }

        static private void WriteToTrace(string str)
        {
            traceWrite.WriteLine("IN FILE: " + inputFileName + " PRT: " + destPrinterName + " [" + DateTime.Now.ToString() + "]  " + str);
        }

        static private void EnableTraceLog()
        {
            try
            {
                if (!string.IsNullOrEmpty(tip.DiagSettings.TraceFile))
                {
                    traceFile = new FileInfo(tip.DiagSettings.TraceFile);
                    if (traceFile.Exists)
                    {
                        traceWrite = traceFile.AppendText();
                    }
                    else
                    {
                        traceWrite = traceFile.CreateText();
                    }
                    //traceWrite = new StreamWriter(traceFile.OpenWrite());
                    traceEnabled = true;
                    WriteToTrace("Trace log enabled. " + DateTime.Now.ToString());
                    traceWrite.Flush();
                }
            }
            catch (Exception ex)
            {
                traceEnabled = false;
                LogAnError("Error creating Trace Log. Error: " + ex.Message);
            }

        }

        static private void EnableErrorLog()
        {
            try
            {
                if (!string.IsNullOrEmpty(tip.DiagSettings.ErrorLogFile))
                {

                    errorFile = new FileInfo(tip.DiagSettings.ErrorLogFile);
                    if (errorFile.Exists)
                    {
                        errorWrite = errorFile.AppendText();
                    }
                    else
                    {
                        errorWrite = errorFile.CreateText();
                    }
                    //errorWrite = new StreamWriter(errorFile.OpenWrite());
                    errorEnabled = true;
                    errorWrite.WriteLine("Error log enabled. " + DateTime.Now.ToString());
                    errorWrite.Flush();
                }
            }
            catch (Exception ex)
            {
                errorEnabled = false;
                LogAnError("Error creating Error Log. Error: " + ex.Message);
            }
        }

        static bool ReadInputFile(int StartLocation)
        {
            int cntr = StartLocation;
            try
            {
                if (StartLocation < 0)
                {
                    StartLocation = 0;
                }
                bool continueLoop = true;
                bool lookingForPageStart = true;
                int PageNum = 1;
                while ((cntr < bufferSize) && (continueLoop))
                {
                    if (inputBuffer[cntr] == 0x1B)
                    {
                        //Look for insert point, <ESC>*p0x0Y
                        if ((cntr + 7 < bufferSize) &&
                            ((inputBuffer[cntr + 1] == 0x2A) && (inputBuffer[cntr + 2] == 0x70) &&
                             (inputBuffer[cntr + 3] == 0x30) && (inputBuffer[cntr + 4] == 0x78) &&
                             (inputBuffer[cntr + 5] == 0x30) && (inputBuffer[cntr + 6] == 0x59)))
                        {
                            //Insert point found
                            if (lookingForPageStart)
                            {
                                lookingForPageStart = false;
                                EventPoints NewEvent = new EventPoints();
                                NewEvent.EventType = EventPointType.epInsertPoint;
                                NewEvent.PageNumber = PageNum;
                                NewEvent.Location = cntr;
                                NewEvent.EventLength = 7;
                                fileEventPoints.Add(NewEvent);
                                if (traceEnabled) WriteToTrace("Insert point found.  Location: " + cntr.ToString());
                            }
                            cntr += 7;
                        }
                        //TEMPORARY  looking for <ESC>&l0E
                        else if ((cntr + 5 < bufferSize) &&
                                  ((inputBuffer[cntr + 1] == 0x26) && (inputBuffer[cntr + 2] == 0x6C) &&
                                   (inputBuffer[cntr + 3] == 0x30) && (inputBuffer[cntr + 4] == 0x45)))
                        {
                            //Insert point found
                            if (lookingForPageStart)
                            {
                                lookingForPageStart = false;
                                EventPoints NewEvent = new EventPoints();
                                NewEvent.EventType = EventPointType.epInsertPoint;
                                NewEvent.PageNumber = PageNum;
                                NewEvent.Location = cntr;
                                NewEvent.EventLength = 5;
                                fileEventPoints.Add(NewEvent);
                                if (traceEnabled) WriteToTrace("Insert point found.  Location: " + cntr.ToString());
                            }
                            cntr += 5;

                        }
                        //Look for the end of page, <ESC>*p6400Y<FF>
                        else if ((cntr + 9 < bufferSize) &&
                                 ((inputBuffer[cntr + 1] == 0x2A) && (inputBuffer[cntr + 2] == 0x70) &&
                                  (inputBuffer[cntr + 3] == 0x36) && (inputBuffer[cntr + 4] == 0x34) &&
                                  (inputBuffer[cntr + 5] == 0x30) && (inputBuffer[cntr + 6] == 0x30) &&
                                  (inputBuffer[cntr + 7] == 0x59) && (inputBuffer[cntr + 8] == 0x0C)))
                        {
                            if (!lookingForPageStart)
                            {
                                lookingForPageStart = true;
                                EventPoints NewEvent = new EventPoints();
                                NewEvent.EventType = EventPointType.epPageEnd;
                                NewEvent.PageNumber = PageNum;
                                NewEvent.Location = cntr;
                                NewEvent.EventLength = 9;
                                fileEventPoints.Add(NewEvent);
                                PageNum++;
                                if (traceEnabled) WriteToTrace("Page break found.  Location: " + cntr.ToString());
                            }
                            cntr += 9;
                        }
                        //Look for the end of page, <ESC>*rB<FF>
                        else if ((cntr + 5 < bufferSize) &&
                                 ((inputBuffer[cntr + 1] == 0x2A) && (inputBuffer[cntr + 2] == 0x72) &&
                                  (inputBuffer[cntr + 3] == 0x42) && (inputBuffer[cntr + 4] == 0x0C)))
                        {
                            if (!lookingForPageStart)
                            {
                                lookingForPageStart = true;
                                EventPoints NewEvent = new EventPoints();
                                NewEvent.EventType = EventPointType.epPageEnd;
                                NewEvent.PageNumber = PageNum;
                                NewEvent.Location = cntr;
                                NewEvent.EventLength = 5;
                                fileEventPoints.Add(NewEvent);
                                PageNum++;
                                if (traceEnabled) WriteToTrace("Page break found.  Location: " + cntr.ToString());
                            }
                            cntr += 5;
                        }
                        //Look for the end of page, <ESC>*c0P<FF>
                        else if ((cntr + 5 < bufferSize) &&
                                 ((inputBuffer[cntr + 1] == 0x2A) && (inputBuffer[cntr + 2] == 0x63) &&
                                  (inputBuffer[cntr + 3] == 0x30) && (inputBuffer[cntr + 4] == 0x50) && 
                                  (inputBuffer[cntr + 5] == 0x0C)))
                        {
                            if (!lookingForPageStart)
                            {
                                lookingForPageStart = true;
                                EventPoints NewEvent = new EventPoints();
                                NewEvent.EventType = EventPointType.epPageEnd;
                                NewEvent.PageNumber = PageNum;
                                NewEvent.Location = cntr;
                                NewEvent.EventLength = 6;
                                fileEventPoints.Add(NewEvent);
                                PageNum++;
                                if (traceEnabled) WriteToTrace("Page break found.  Location: " + cntr.ToString());
                            }
                            cntr += 5;
                        }
                        //Look for the end of page, <ESC>*c2P<FF>
                        else if ((cntr + 5 < bufferSize) &&
                                 ((inputBuffer[cntr + 1] == 0x2A) && (inputBuffer[cntr + 2] == 0x63) &&
                                  (inputBuffer[cntr + 3] == 0x32) && (inputBuffer[cntr + 4] == 0x50) &&
                                  (inputBuffer[cntr + 5] == 0x0C)))
                        {
                            if (!lookingForPageStart)
                            {
                                lookingForPageStart = true;
                                EventPoints NewEvent = new EventPoints();
                                NewEvent.EventType = EventPointType.epPageEnd;
                                NewEvent.PageNumber = PageNum;
                                NewEvent.Location = cntr;
                                NewEvent.EventLength = 6;
                                fileEventPoints.Add(NewEvent);
                                PageNum++;
                                if (traceEnabled) WriteToTrace("Page break found.  Location: " + cntr.ToString());
                            }
                            cntr += 5;
                        }
                        //UEL &-12345X
                        else if ((cntr + 8 <= bufferSize) &&
                                     ((inputBuffer[cntr + 1] == 0x25) && (inputBuffer[cntr + 2] == 0x2D) &&
                                      (inputBuffer[cntr + 3] == 0x31) && (inputBuffer[cntr + 4] == 0x32) &&
                                      (inputBuffer[cntr + 5] == 0x33) && (inputBuffer[cntr + 6] == 0x34) &&
                                      (inputBuffer[cntr + 7] == 0x35) && (inputBuffer[cntr + 8] == 0x58)))
                        {
                            EventPoints newEventPoint = new EventPoints(PageNum, cntr, EventPointType.epUELLocation, 9);
                            fileEventPoints.Add(newEventPoint);
                            cntr += 9;
                            //Look for @PJL EOL so that its not considered plain text
                            if ((cntr + 7 <= bufferSize) &&
                                ((inputBuffer[cntr] == 0x40) && (inputBuffer[cntr + 1] == 0x50) &&
                                 (inputBuffer[cntr + 2] == 0x4A) && (inputBuffer[cntr + 3] == 0x4C) &&
                                 (inputBuffer[cntr + 4] == 0x20) && (inputBuffer[cntr + 5] == 0x45) &&
                                 (inputBuffer[cntr + 6] == 0x4F) && (inputBuffer[cntr + 7] == 0x4A)))
                            {
                                EventPoints ep = new EventPoints(PageNum, cntr, EventPointType.epEndOfJob, 8);
                                fileEventPoints.Add(ep);
                                cntr += 8;
                            }
                        }
                        //Start raster data *r1A
                        else if ((cntr < cntr + 5) &&
                                 ((inputBuffer[cntr + 1] == 0x2A) && (inputBuffer[cntr + 2] == 0x72) &&
                                  (inputBuffer[cntr + 3] == 0x31) && (inputBuffer[cntr + 4] == 0x41)))
                        {
                            cntr += 5;
                            bool foundEndRaster = false;
                            //loop until end of raster is found
                            while ((cntr < bufferSize) && (!foundEndRaster))
                            {
                                if (inputBuffer[cntr] == 0x1B)
                                {

                                    if ((cntr < cntr + 4) &&
                                        ((inputBuffer[cntr + 1] == 0x2A) && (inputBuffer[cntr + 2] == 0x72) &&
                                         (inputBuffer[cntr + 3] == 0x42)))
                                    {
                                        foundEndRaster = true;
                                        //IMPORTANT: NEEDS TO EXIT WITH CNTR POINTING TO ESCAPE SO DO NOT INCREMENT CNTR IN CASE THIS IS ALSO THE END OF PAGE
                                    }
                                    else if ((cntr < cntr + 4) &&
                                        ((inputBuffer[cntr + 1] == 0x2A) && (inputBuffer[cntr + 2] == 0x72) &&
                                         (inputBuffer[cntr + 3] == 0x43)))
                                    {
                                        foundEndRaster = true;
                                        //IMPORTANT: NEEDS TO EXIT WITH CNTR POINTING TO ESCAPE SO DO NOT INCREMENT CNTR IN CASE THIS IS ALSO THE END OF PAGE
                                    }
                                    else
                                    {
                                        cntr++;
                                    }
                                }
                                else
                                {
                                    cntr++;
                                }
                            }
                        }
                        //Look for ESC strings that have data (that end with W)
                        //)s,(s,(f,&n,*o,*b,*v,*m,*l,*i, *c
                        else if (((cntr + 2) < bufferSize) &&
                                 (((inputBuffer[cntr + 1] == 0x29) && (inputBuffer[cntr + 2] == 0x73)) ||
                                  ((inputBuffer[cntr + 1] == 0x28) && (inputBuffer[cntr + 2] == 0x73)) ||
                                  ((inputBuffer[cntr + 1] == 0x28) && (inputBuffer[cntr + 2] == 0x66)) ||
                                  ((inputBuffer[cntr + 1] == 0x26) && (inputBuffer[cntr + 2] == 0x6E)) ||
                                  ((inputBuffer[cntr + 1] == 0x2A) && (inputBuffer[cntr + 2] == 0x6F)) ||
                                  ((inputBuffer[cntr + 1] == 0x2A) && (inputBuffer[cntr + 2] == 0x62)) ||
                                  ((inputBuffer[cntr + 1] == 0x2A) && (inputBuffer[cntr + 2] == 0x63)) ||
                                  ((inputBuffer[cntr + 1] == 0x2A) && (inputBuffer[cntr + 2] == 0x76)) ||
                                  ((inputBuffer[cntr + 1] == 0x2A) && (inputBuffer[cntr + 2] == 0x6D)) ||
                                  ((inputBuffer[cntr + 1] == 0x2A) && (inputBuffer[cntr + 2] == 0x6C)) ||
                                  ((inputBuffer[cntr + 1] == 0x2A) && (inputBuffer[cntr + 2] == 0x69))))
                        {
                            int HoldStartPos = cntr + 2;
                            int LengthStartPos = HoldStartPos + 1;
                            bool inEscSeq = true;
                            cntr += 2;
                            while ((inEscSeq) && (cntr < bufferSize))
                            {
                                //Capital letter and @ mark the end of an Escape sequence
                                if ((inputBuffer[cntr] > 0x3F) && (inputBuffer[cntr] < 0x5B))
                                {
                                    inEscSeq = false;
                                    cntr++;
                                }
                                //check for a none numeric character (lower case letter)
                                else if (!((inputBuffer[cntr] > 0x29) && (inputBuffer[cntr] < 0x40)))
                                {
                                    cntr++;
                                    LengthStartPos = cntr;
                                }
                                else
                                {
                                    cntr++;
                                }
                            }

                            //W
                            if (inputBuffer[cntr - 1] == 0x57)
                            {
                                int JumpCntr = Convert.ToInt32(Encoding.ASCII.GetString(inputBuffer, LengthStartPos, cntr - LengthStartPos - 1));
                                cntr += JumpCntr;
                            }
                            //look for the *b ending with a V
                            else if ((inputBuffer[HoldStartPos - 1] == 0x2A) && (inputBuffer[HoldStartPos] == 0x62) && (inputBuffer[cntr - 1] == 0x56))
                            {
                                int JumpCntr = Convert.ToInt32(Encoding.ASCII.GetString(inputBuffer, LengthStartPos, cntr - LengthStartPos - 1));
                                cntr += JumpCntr;
                            }
                        }
                        //Another PCL string that could have data
                        else if (((cntr + 3) < bufferSize) &&
                                 ((inputBuffer[cntr + 1] == 0x26) && (inputBuffer[cntr + 2] == 0x70)))
                        {

                            int HoldStartPos = cntr + 2;
                            int LengthStartPos = HoldStartPos + 1;
                            bool inEscSeq = true;
                            cntr += 2;
                            while ((inEscSeq) && (cntr < bufferSize))
                            {
                                //Capital letter and @ mark the end of an Escape sequence
                                if ((inputBuffer[cntr] > 0x3F) && (inputBuffer[cntr] < 0x5B))
                                {
                                    inEscSeq = false;
                                    cntr++;
                                }
                                //check for a none numeric character (lower case letter)
                                else if (!((inputBuffer[cntr] > 0x29) && (inputBuffer[cntr] < 0x40)))
                                {
                                    cntr++;
                                    LengthStartPos = cntr;
                                }
                                else
                                {
                                    cntr++;
                                }
                            }

                            //X
                            if (inputBuffer[cntr - 1] == 0x58)
                            {
                                int JumpCntr = Convert.ToInt32(Encoding.ASCII.GetString(inputBuffer, LengthStartPos, cntr - LengthStartPos - 1));
                                cntr += JumpCntr;
                            }
                        }
                        //Look for the <ESC>9 string
                        else if (inputBuffer[cntr + 1] == 0x39)
                        {
                            cntr = cntr + 2;
                        }
                        //Removing &l###u###Z (long edge/short edge offset)
                        //Looking for &l
                        else if (((cntr + 2) < bufferSize) &&
                                 ((inputBuffer[cntr + 1] == 0x26) &&
                                  (inputBuffer[cntr + 2] == 0x6C)))
                        {
                            int HoldStartPos = cntr;
                            bool inEscSeq = true;
                            cntr += 2;
                            while ((inEscSeq) && (cntr < bufferSize))
                            {
                                //Capital letter and @ mark the end of an Escape sequence
                                if ((inputBuffer[cntr] > 0x3F) && (inputBuffer[cntr] < 0x5B))
                                {
                                    inEscSeq = false;
                                    //cntr++;
                                }
                                else
                                {
                                    cntr++;
                                }
                            }
                            if ((inputBuffer[cntr] == 0x5A) || (inputBuffer[cntr] == 0x55))
                            {
                                if (tip.RemoveLongShortPageOffset)
                                {
                                    EventPoints ep = new EventPoints(PageNum, HoldStartPos, EventPointType.epRemove, cntr - HoldStartPos + 1);
                                    fileEventPoints.Add(ep);
                                    InsertAfterPg = new byte[cntr - HoldStartPos + 1];
                                    Array.Copy(inputBuffer, HoldStartPos, InsertAfterPg, 0, cntr - HoldStartPos + 1);
                                }
                            }
                            cntr++;
                        }
                        //Troy command %p
                        else if (((cntr + 2) < bufferSize) &&
                                 ((inputBuffer[cntr + 1] == 0x25) &&
                                  (inputBuffer[cntr + 2] == 0x70)))
                        {
                            int HoldStartPos = cntr + 2;
                            bool inEscSeq = true;
                            cntr += 2;
                            while ((inEscSeq) && (cntr < bufferSize))
                            {
                                //Capital letter and @ mark the end of an Escape sequence
                                if ((inputBuffer[cntr] > 0x3F) && (inputBuffer[cntr] < 0x5B))
                                {
                                    inEscSeq = false;
                                }
                                else
                                {
                                    cntr++;
                                }
                            }
                            //Looking for %p M
                            if (inputBuffer[cntr] == 0x4D)
                            {
                                try
                                {
                                    int PgValue = Convert.ToInt32(Encoding.ASCII.GetString(inputBuffer, HoldStartPos + 1, cntr - HoldStartPos - 1));
                                    PantographValue = PgValue;
                                }
                                catch (Exception ex)
                                {
                                    if (traceEnabled) WriteToTrace("ERROR: ReadInputFile.  Error converting to number in Pantograph number. Location: " + cntr.ToString() + " Exception: " + ex.Message);

                                    //Error in converting string to int
                                }
                            }
                            cntr++;

                        }
                        //Troy command %w
                        else if (((cntr + 2) < bufferSize) &&
                                 ((inputBuffer[cntr + 1] == 0x25) &&
                                  (inputBuffer[cntr + 2] == 0x77)))
                        {
                            int HoldStartPos = cntr + 2;
                            bool inEscSeq = true;
                            cntr += 2;
                            while ((inEscSeq) && (cntr < bufferSize))
                            {
                                //Capital letter and @ mark the end of an Escape sequence
                                if ((inputBuffer[cntr] > 0x3F) && (inputBuffer[cntr] < 0x5B))
                                {
                                    inEscSeq = false;
                                }
                                else
                                {
                                    cntr++;
                                }
                            }
                            //Looking for %w M
                            if (inputBuffer[cntr] == 0x4D)
                            {
                                try
                                {
                                    int TmValue = Convert.ToInt32(Encoding.ASCII.GetString(inputBuffer, HoldStartPos + 1, cntr - HoldStartPos - 1));
                                    TmPattern = TmValue;
                                }
                                catch (Exception ex)
                                {
                                    if (traceEnabled) WriteToTrace("ERROR: ReadInputFile. Error converting to number in TM Pattern.  Location: " + cntr.ToString() + " Exception: " + ex.Message);

                                    //Error in converting string to int
                                }
                            }
                            //Looking for %w B
                            else if (inputBuffer[cntr] == 0x42)
                            {
                                try
                                {
                                    int TmValue = Convert.ToInt32(Encoding.ASCII.GetString(inputBuffer, HoldStartPos + 1, cntr - HoldStartPos - 1));
                                    if (TmValue == 1)
                                    {
                                        ExtractTmInclusionRegion(ref cntr);
                                    }
                                    else if (TmValue == 2)
                                    {
                                        ExtractTmExclusionRegion(ref cntr);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (traceEnabled) WriteToTrace("ERROR: ReadInputFile.  Error converting to number in TM Inclusion region. Location: " + cntr.ToString() + " Exception: " + ex.Message);

                                }
                            }
                            cntr++;

                        }
                        //Troy command %m
                        else if (((cntr + 2) < bufferSize) &&
                                 ((inputBuffer[cntr + 1] == 0x25) &&
                                  (inputBuffer[cntr + 2] == 0x6D)))
                        {
                            int HoldCntr = cntr;
                            bool inEscSeq = true;
                            cntr += 2;
                            while ((inEscSeq) && (cntr < bufferSize))
                            {
                                //Capital letter and @ mark the end of an Escape sequence
                                if ((inputBuffer[cntr] > 0x3F) && (inputBuffer[cntr] < 0x5B))
                                {
                                    inEscSeq = false;
                                }
                                else
                                {
                                    cntr++;
                                }
                            }
                            //%m T
                            if (inputBuffer[cntr] == 0x54)
                            {
                                ExtractTmDataFromTroyCmds(ref cntr, PageNum);
                                EventPoints ep = new EventPoints(PageNum, HoldCntr, EventPointType.epRemove, cntr - HoldCntr);
                                fileEventPoints.Add(ep);
                            }
                            cntr++;
                        }
                        else
                        {
                            bool inEscSeq = true;
                            while ((inEscSeq) && (cntr < bufferSize))
                            {
                                //Capital letter and @ mark the end of an Escape sequence
                                if ((inputBuffer[cntr] > 0x3F) && (inputBuffer[cntr] < 0x5B))
                                {
                                    inEscSeq = false;
                                    cntr++;
                                }
                                else
                                {
                                    cntr++;
                                }
                            }
                        }
                    }
                    //Form feed
                    else if (inputBuffer[cntr] == 0x0C)
                    {
                        if (!lookingForPageStart)
                        {
                            lookingForPageStart = true;
                            EventPoints NewEvent = new EventPoints();
                            NewEvent.EventType = EventPointType.epPageEnd;
                            NewEvent.PageNumber = PageNum;
                            NewEvent.Location = cntr;
                            NewEvent.EventLength = 1;
                            fileEventPoints.Add(NewEvent);
                            PageNum++;
                            if (traceEnabled) WriteToTrace("Page break found.  Location: " + cntr.ToString());
                        }
                        cntr++;
                    }
                    else
                    {
                        cntr++;
                    }
                    //DO NOT INCREMENT CNTR HERE!!!
                }
                return true;
            }
            catch (Exception ex)
            {
                if (traceEnabled) WriteToTrace("ERROR: ReadInputFile.  Location: " + cntr.ToString() + " Exception: " + ex.Message);
                return false;
            }
        }

        static void ExtractTmDataFromTroyCmds(ref int cntr, int pagenum)
        {
            byte[] tmstring = new byte[255];
            int tmcntr = 0;
            int tempcntr = cntr + 1;
            bool ContinueLoop = true;

            while ((ContinueLoop) && (tempcntr < bufferSize))
            {
                if (inputBuffer[tempcntr] == 0x1B)
                {
                    cntr = tempcntr;
                    //If Troymark data was found, store it in the tm data per page
                    if (tmcntr > 0)
                    {
                        string temp = Encoding.ASCII.GetString(tmstring, 0, tmcntr);
                        Array.Clear(tmstring, 0, tmstring.Length);
                        tmcntr = 0;
                        if (TmPerPage.ContainsKey(pagenum))
                        {
                            TmPerPage[pagenum] += temp;
                        }
                        else
                        {
                            TmPerPage.Add(pagenum, temp);
                        }
                    }
                    //Look for %m0T
                    if (((tempcntr + 4) < bufferSize) &&
                        ((inputBuffer[tempcntr + 1] == 0x25) &&
                         (inputBuffer[tempcntr + 2] == 0x6D) &&
                         (inputBuffer[tempcntr + 3] == 0x30) &&
                         (inputBuffer[tempcntr + 4] == 0x54)))
                    {
                        tempcntr += 5;
                        //If next ESC string is another Troymark string
                        if (((tempcntr + 3) < bufferSize) &&
                            ((inputBuffer[tempcntr] == 0x1B) &&
                             (inputBuffer[tempcntr + 1] == 0x25) &&
                             (inputBuffer[tempcntr + 2] == 0x6D)))
                        {
                            //Loop to the T
                            bool inEscSeq = true;
                            tempcntr += 3;
                            while ((inEscSeq) && (tempcntr < bufferSize))
                            {
                                //Capital letter and @ mark the end of an Escape sequence
                                if ((inputBuffer[tempcntr] > 0x3F) && (inputBuffer[tempcntr] < 0x5B))
                                {
                                    //tempcntr++;
                                    inEscSeq = false;
                                }
                                else
                                {
                                    tempcntr++;
                                }
                            }
                        }
                        else
                        {
                            cntr = tempcntr;
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {
                    if ((inputBuffer[tempcntr] > 0x1F) &&
                        (inputBuffer[tempcntr] < 0x7F))
                    {
                        if (tmcntr < 255)
                        {
                            tmstring[tmcntr] = inputBuffer[tempcntr];
                            tmcntr++;
                        }
                    }
                }
                tempcntr++;
            }

        }


        static void ExtractTmInclusionRegion(ref int cntr)
        {
            bool ContinueLoop = true;
            int tempcntr = cntr;
            while ((ContinueLoop) && (tempcntr < bufferSize))
            {
                //if <ESC>%w
                if (((tempcntr + 3) < bufferSize) &&
                    ((inputBuffer[tempcntr + 1] == 0x1B) &&
                     (inputBuffer[tempcntr + 2] == 0x25) &&
                     (inputBuffer[tempcntr + 3] == 0x77)))
                {
                    int HoldStartPos = cntr + 3;
                    bool inEscSeq = true;
                    tempcntr += 4;
                    while ((inEscSeq) && (tempcntr < bufferSize))
                    {
                        //Capital letter and @ mark the end of an Escape sequence
                        if ((inputBuffer[tempcntr] > 0x3F) && (inputBuffer[tempcntr] < 0x5B))
                        {
                            inEscSeq = false;
                        }
                        else
                        {
                            tempcntr++;
                        }
                    }
                    //X
                    if (inputBuffer[tempcntr] == 0x58)
                    {
                        try
                        {
                            int TmValue = Convert.ToInt32(Encoding.ASCII.GetString(inputBuffer, HoldStartPos + 1, tempcntr - HoldStartPos - 1));
                            TmInclusionXAnchor = TmValue;
                        }
                        catch (Exception ex)
                        {
                            if (traceEnabled) WriteToTrace("ERROR: ExtractTmInclusionRegion.  Error converting to number XAnchor. Location: " + cntr.ToString() + " Exception: " + ex.Message);
                            //Error in converting string to int
                        }
                        cntr = tempcntr;
                    }
                    //Y
                    else if (inputBuffer[tempcntr] == 0x59)
                    {
                        try
                        {
                            int TmValue = Convert.ToInt32(Encoding.ASCII.GetString(inputBuffer, HoldStartPos + 1, tempcntr - HoldStartPos - 1));
                            TmInclusionYAnchor = TmValue;
                        }
                        catch (Exception ex)
                        {
                            if (traceEnabled) WriteToTrace("ERROR: ExtractTmInclusionRegion. Error converting to number YAnchor. Location: " + cntr.ToString() + " Exception: " + ex.Message);
                            //Error in converting string to int
                        }
                        cntr = tempcntr;

                    }
                    //V - Height
                    else if (inputBuffer[tempcntr] == 0x56)
                    {
                        try
                        {
                            int TmValue = Convert.ToInt32(Encoding.ASCII.GetString(inputBuffer, HoldStartPos + 1, tempcntr - HoldStartPos - 1));
                            TmInclusionHeight = TmValue;
                        }
                        catch (Exception ex)
                        {
                            if (traceEnabled) WriteToTrace("ERROR: ExtractTmInclusionRegions. Error converting to number Height Location: " + cntr.ToString() + " Exception: " + ex.Message);
                            //Error in converting string to int
                        }
                        cntr = tempcntr;

                    }
                    //H - Width
                    else if (inputBuffer[tempcntr] == 0x48)
                    {
                        try
                        {
                            int TmValue = Convert.ToInt32(Encoding.ASCII.GetString(inputBuffer, HoldStartPos + 1, tempcntr - HoldStartPos - 1));
                            TmInclusionWidth = TmValue;
                        }
                        catch (Exception ex)
                        {
                            if (traceEnabled) WriteToTrace("ERROR: ExtractTmInclusionRegion. Error converting to number Width Location: " + cntr.ToString() + " Exception: " + ex.Message);
                            //Error in converting string to int
                        }
                        cntr = tempcntr;
                    }
                    else
                    {
                        //Leave cntr pointing to the character before the ESC
                        ContinueLoop = false;
                    }
                }
                else
                {
                    ContinueLoop = false;
                }
            }
        }

        static void ExtractTmExclusionRegion(ref int cntr)
        {
            bool ContinueLoop = true;
            int tempcntr = cntr;
            while ((ContinueLoop) && (tempcntr < bufferSize))
            {
                //if <ESC>%w
                if (((tempcntr + 3) < bufferSize) &&
                    ((inputBuffer[tempcntr + 1] == 0x1B) &&
                     (inputBuffer[tempcntr + 2] == 0x25) &&
                     (inputBuffer[tempcntr + 3] == 0x77)))
                {
                    int HoldStartPos = cntr + 3;
                    bool inEscSeq = true;
                    tempcntr += 4;
                    while ((inEscSeq) && (tempcntr < bufferSize))
                    {
                        //Capital letter and @ mark the end of an Escape sequence
                        if ((inputBuffer[tempcntr] > 0x3F) && (inputBuffer[tempcntr] < 0x5B))
                        {
                            inEscSeq = false;
                        }
                        else
                        {
                            tempcntr++;
                        }
                    }
                    //X
                    if (inputBuffer[tempcntr] == 0x58)
                    {
                        try
                        {
                            int TmValue = Convert.ToInt32(Encoding.ASCII.GetString(inputBuffer, HoldStartPos + 1, tempcntr - HoldStartPos - 1));
                            TmExclusionXAnchor = TmValue;
                        }
                        catch (Exception ex)
                        {
                            if (traceEnabled) WriteToTrace("ERROR: ExtractTmExclusionREgion. Error converting to number XAnchor Location: " + cntr.ToString() + " Exception: " + ex.Message);
                            //Error in converting string to int
                        }
                        cntr = tempcntr;
                    }
                    //Y
                    else if (inputBuffer[tempcntr] == 0x59)
                    {
                        try
                        {
                            int TmValue = Convert.ToInt32(Encoding.ASCII.GetString(inputBuffer, HoldStartPos + 1, tempcntr - HoldStartPos - 1));
                            TmExclusionYAnchor = TmValue;
                        }
                        catch (Exception ex)
                        {
                            if (traceEnabled) WriteToTrace("ERROR: ExtractTmExclusionRegion. Error converting to number YAnchor Location: " + cntr.ToString() + " Exception: " + ex.Message);
                            //Error in converting string to int
                        }
                        cntr = tempcntr;

                    }
                    //V - Height
                    else if (inputBuffer[tempcntr] == 0x56)
                    {
                        try
                        {
                            int TmValue = Convert.ToInt32(Encoding.ASCII.GetString(inputBuffer, HoldStartPos + 1, tempcntr - HoldStartPos - 1));
                            TmExclusionHeight = TmValue;
                        }
                        catch (Exception ex)
                        {
                            if (traceEnabled) WriteToTrace("ERROR: ExtractTmExclusionREgion.  Error converting to number HeightLocation: " + cntr.ToString() + " Exception: " + ex.Message);
                            //Error in converting string to int
                        }
                        cntr = tempcntr;

                    }
                    //H - Width
                    else if (inputBuffer[tempcntr] == 0x48)
                    {
                        try
                        {
                            int TmValue = Convert.ToInt32(Encoding.ASCII.GetString(inputBuffer, HoldStartPos + 1, tempcntr - HoldStartPos - 1));
                            TmExclusionWidth = TmValue;
                        }
                        catch (Exception ex)
                        {
                            if (traceEnabled) WriteToTrace("ERROR: ExtractTmExclsuionRegion. Error converting to number Width Location: " + cntr.ToString() + " Exception: " + ex.Message);
                            //Error in converting string to int
                        }
                        cntr = tempcntr;
                    }
                    else
                    {
                        //Leave cntr pointing to the character before the ESC
                        ContinueLoop = false;
                    }
                }
                else
                {
                    ContinueLoop = false;
                }
            }
        }


        /// <summary>
        /// Write the PCL out to the output file
        /// </summary>
        /// <param name="filename">name of the output file</param>
        /// <returns>false if an error occurs</returns>
        static bool WriteOutPcl(string filename)
        {
            try
            {
                BinaryWriter outbuf = new BinaryWriter(File.Open(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None));

                int currPage = 1;
                int currCntr = 0;
                foreach (EventPoints evpt in fileEventPoints)
                {
                    if (evpt.PageNumber == currPage)
                    {

                        if (evpt.Location > currCntr)
                        {
                            outbuf.Write(inputBuffer, currCntr, evpt.Location - currCntr);
                            currCntr = evpt.Location;
                        }

                        switch (evpt.EventType)
                        {
                            case EventPointType.epInsertPoint:
                                //Force to one page/no copies
                                if (currPage == 1)
                                {
                                    byte[] NumberOfCopiesOne = new byte[5] { 0x1B, 0x26, 0x6C, 0x31, 0x58 };
                                    outbuf.Write(NumberOfCopiesOne, 0, NumberOfCopiesOne.Length);
                                }

                                //Output the Insert PCL
                                outbuf.Write(inputBuffer, evpt.Location, evpt.EventLength);
                                currCntr += evpt.EventLength;

                                //Output the duplex command if necessary
                                if ((TmSettings.TroymarkOnBack) && (currPage == 1))
                                {
                                    string enDup = "\u001B&l1S";
                                    byte[] enDupBytes = new UTF8Encoding(true).GetBytes(enDup);
                                    outbuf.Write(enDupBytes, 0, enDupBytes.Length);
                                }
                                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                                for (int cntr = 1; cntr <= 4; cntr++)
                                {
                                    string pgPath = tip.DefaultPantographLocation;
                                    string pgFilename = String.Format(@"\PantographProfile{0}Page1.pcl", cntr);
                                    string pgFile = pgPath + pgFilename;
                                    if (Directory.Exists(pgPath) && File.Exists(pgFile))
                                    {
                                        if (PantographValue > 0)
                                        {
                                            if (InsertPantograph(pgFile, ref outbuf))
                                            {
                                                if (traceEnabled) WriteToTrace("Pantograph inserted.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (traceEnabled) WriteToTrace("Pantograph not inserted. Data file not found. Path: " + tip.DefaultPantographLocation);
                                    }
                                }
                            
                            //Check for Troymark
                                if ((TmSettings != null) && (TmSettings.TroymarkEnabled) && (!TmSettings.TroymarkOnBack))
                                {
                                    if (InsertSoftwareTroymark(currPage, false, ref outbuf))
                                    {
                                        if (traceEnabled) WriteToTrace("Troymark inserted on front.");
                                    }
                                }
                                if ((UnlicensedPrinter) || (InvalidPrinterLicense) || (TroymarkErrorMsg != ""))
                                {
                                    InsertDemoTroyMark(ref outbuf);
                                }
                                if (tip.InsertAfterFeatures != "")
                                {
                                    string tmp = tip.InsertAfterFeatures.Replace("/e", "\u001B");
                                    tmp = tmp.Replace("/r", "\u000D");
                                    tmp = tmp.Replace("/n", "\u000A");
                                    tmp = tmp.Replace("/a", "&");
                                    byte[] bc = new UTF8Encoding(true).GetBytes(tmp);
                                    outbuf.Write(bc, 0, bc.Length);

                                }
                                if ((InsertAfterPg != null) && (InsertAfterPg.Length > 0))
                                {
                                    outbuf.Write(InsertAfterPg, 0, InsertAfterPg.Length);
                                }
                                break;
                            case EventPointType.epPageEnd:
                                outbuf.Write(inputBuffer, evpt.Location, evpt.EventLength);
                                currCntr += evpt.EventLength;
                                //Check for Troymark
                                if ((TmSettings != null) && (TmSettings.TroymarkEnabled) && (TmSettings.TroymarkOnBack))
                                {
                                    if (InsertSoftwareTroymark(currPage, true, ref outbuf))
                                    {
                                        if (traceEnabled) WriteToTrace("Troymark inserted on back.");
                                    }
                                }
                                currPage++;

                                break;
                            case EventPointType.epRemove:
                                currCntr += evpt.EventLength;
                                break;
                            case EventPointType.epUELLocation:
                                outbuf.Write(inputBuffer, evpt.Location, evpt.EventLength);
                                currCntr += evpt.EventLength;
                                break;

                            case EventPointType.epEndOfJob:
                                outbuf.Write(inputBuffer, evpt.Location, evpt.EventLength);
                                currCntr += evpt.EventLength;
                                break;

                        }
                    }

                }
                outbuf.Flush();
                outbuf.Close();
                return true;
            }
            catch (Exception ex)
            {
                if (traceEnabled) WriteToTrace("ERROR: WriteOutPcl.  Exception: " + ex.Message);
                return false;
            }
        }

        static bool InsertPantograph(string filename, ref BinaryWriter outbuf)
        {
            try
            {
                FileStream pgFile;
                pgFile = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader binReader = new BinaryReader(pgFile);
                byte[] inBuffer = new Byte[pgFile.Length];
                inBuffer = binReader.ReadBytes(Convert.ToInt32(pgFile.Length));
                outbuf.Write(inBuffer, 0, inBuffer.Length);
                binReader.Close();
                return true;
            }
            catch (Exception ex)
            {
                if (traceEnabled) WriteToTrace("ERROR: InsertPantograph.  Exception: " + ex.Message);
                return false;
            }
        }

        static bool InsertDemoTroyMark(ref BinaryWriter outbuf)
        {
            Interface.TROYmarkConfiguration tmConfig = new Interface.TROYmarkConfiguration();
            //tmConfig.Pattern = Troy.TROYmarkPclBuilder.Interface.TROYmarkPatternType.tmMedium;
            tmConfig.Pattern = Interface.TROYmarkPatternType.tmDark;
            tmConfig.LineSpacing = 800;
            tmConfig.InclusionRegion = new Interface.TroyMarkRegionType();
            tmConfig.InclusionRegion.XAnchor = 0;
            tmConfig.InclusionRegion.YAnchor = 0;
            tmConfig.InclusionRegion.Width = 4800;
            tmConfig.InclusionRegion.Height = 6400;

            string TMString = "";

            if (UnlicensedPrinter)
            {
                if (TroymarkLicenseErrorText == "")
                {
                    TMString = " *** TROY SECURE PRINT ENTERPRISE - ";
                    TMString += "UNLICENSED COPY *** ";
                }
                else
                {
                    TMString = TroymarkLicenseErrorText;
                }
            }
            if (InvalidPrinterLicense)
            {
                TMString += InvalidLicenseMessage + " ";
            }

            if (TroymarkErrorMsg != "")
            {
                TMString += TroymarkErrorMsg;
            }

            tmConfig.TROYmarkText.Add(TMString);

            byte[] tmData;
            if (Interface.GetTroyMarkPcl(tmConfig, out tmData, true))
            {
                outbuf.Write(tmData, 0, tmData.Length);
            }
            return true;
        }



        static bool InsertSoftwareTroymark(int currPage, bool insertFF, ref BinaryWriter outbuf)
        {
            try
            {

                string TPattern = "";
                if (tip.TroyCommandsIncluded)
                {
                    if (TmPattern == 1)
                    {
                        TPattern = "LIGHT";
                    }
                    else if (TmPattern == 2)
                    {
                        TPattern = "MEDIUM";
                    }
                    else if (TmPattern == 3)
                    {
                        TPattern = "DARK";
                    }
                }
                else
                {
                    TPattern = TmSettings.TroymarkPattern;
                }


                if ((TPattern.ToUpper() != "NONE") && (TPattern != ""))
                {
                    Interface.TROYmarkConfiguration tmConfig = new Interface.TROYmarkConfiguration();
                    if (TPattern.ToUpper() == "DARK")
                    {
                        tmConfig.Pattern = Interface.TROYmarkPatternType.tmDark;
                    }
                    else if (TPattern.ToUpper() == "MEDIUM")
                    {
                        tmConfig.Pattern = Interface.TROYmarkPatternType.tmMedium;
                    }
                    else if (TPattern.ToUpper() == "LIGHT")
                    {
                        tmConfig.Pattern = Interface.TROYmarkPatternType.tmLight;
                    }

                    if (TmSettings.TroymarkSpacing != "")
                    {
                        tmConfig.LineSpacing = Convert.ToInt32(TmSettings.TroymarkSpacing);
                    }


                    if (tip.TroyCommandsIncluded)
                    {
                        if (tmConfig.InclusionRegion == null)
                        {
                            tmConfig.InclusionRegion = new Interface.TroyMarkRegionType();
                        }
                        tmConfig.InclusionRegion.XAnchor = TmInclusionXAnchor;
                        tmConfig.InclusionRegion.YAnchor = TmInclusionYAnchor;
                        tmConfig.InclusionRegion.Height = TmInclusionHeight;
                        tmConfig.InclusionRegion.Width = TmInclusionWidth;
                        if (TmExclusionXAnchor > -1)
                        {
                            /*TroyMarkRegionType tmrt = new TroyMarkRegionType();
                            tmrt.XAnchor = TmExclusionXAnchor;
                            tmrt.YAnchor = TmExclusionYAnchor;
                            tmrt.Height = TmExclusionHeight;
                            tmrt.Width = TmExclusionWidth;
                            tmConfig.ExclusionRegion.Add(tmrt);*/
                        }
                    }
                    else
                    {
                        if (TmSettings.TroymarkInclusion != "")
                        {
                            if (tmConfig.InclusionRegion == null)
                            {
                                tmConfig.InclusionRegion = new Interface.TroyMarkRegionType();
                            }
                            int cntr1 = 1;
                            foreach (string subStr1 in TmSettings.TroymarkInclusion.Split(','))
                            {
                                if (cntr1 == 1)
                                {
                                    tmConfig.InclusionRegion.XAnchor = Convert.ToInt32(subStr1);
                                }
                                else if (cntr1 == 2)
                                {
                                    tmConfig.InclusionRegion.YAnchor = Convert.ToInt32(subStr1);
                                }
                                else if (cntr1 == 3)
                                {
                                    tmConfig.InclusionRegion.Width = Convert.ToInt32(subStr1);
                                }
                                else if (cntr1 == 4)
                                {
                                    tmConfig.InclusionRegion.Height = Convert.ToInt32(subStr1);
                                }
                                cntr1++;
                            }
                        }

                        if (TmSettings.TroymarkExclusion != "")
                        {
                            Interface.TroyMarkRegionType tmRegion = new Interface.TroyMarkRegionType();
                            int cntr1 = 1;
                            foreach (string subStr1 in TmSettings.TroymarkExclusion.Split(','))
                            {
                                if (cntr1 == 1)
                                {
                                    tmRegion.XAnchor = Convert.ToInt32(subStr1);
                                }
                                else if (cntr1 == 2)
                                {
                                    tmRegion.YAnchor = Convert.ToInt32(subStr1);
                                }
                                else if (cntr1 == 3)
                                {
                                    tmRegion.Width = Convert.ToInt32(subStr1);
                                }
                                else if (cntr1 == 4)
                                {
                                    tmRegion.Height = Convert.ToInt32(subStr1);
                                }
                                cntr1++;
                            }
                            tmConfig.ExclusionRegion.Add(tmRegion);
                        }
                    }

                    string TMString = "";
                    //if (!UnlicensedPrinter)
                    //{
                    if (tip.TroyCommandsIncluded)
                    {
                        if (TmPerPage.ContainsKey(currPage))
                        {
                            TMString = TmPerPage[currPage];
                        }
                    }
                    else
                    {
                        if (TroymarkDataPerPage.Count > 0)
                        {
                            if (TroymarkDataPerPage.ContainsKey(currPage))
                            {
                                TMString += TroymarkDataPerPage[currPage];
                            }
                        }
                        else if (TroymarkData != "")
                        {
                            TMString += TroymarkData;
                        }
                    }
                    //}

                    if (TmSettings.TroymarkStaticText != "")
                    {
                        TMString += TmSettings.TroymarkStaticText;
                    }

                    //if (TroymarkErrorMsg != "")
                    //{
                    //	TMString += TroymarkErrorMsg;	
                    //}


                    if (TMString != "")
                    {
                        if (Convert.ToInt32(TmSettings.TroymarkCharsPerLine) > 0)
                        {
                            int strCntr = 1, currIndex = 0;
                            int CharsPerLine = Convert.ToInt32(TmSettings.TroymarkCharsPerLine);
                            while (TMString.Length > (strCntr * CharsPerLine))
                            {
                                tmConfig.TROYmarkText.Add(TMString.Substring(CharsPerLine * (strCntr - 1), CharsPerLine));
                                currIndex += CharsPerLine;
                                strCntr++;
                            }
                            string nextString = TMString.Substring(CharsPerLine * (strCntr - 1), TMString.Length - currIndex);
                            if (currIndex > 0)
                            {
                                nextString += TMString.Substring(0, CharsPerLine - (TMString.Length - currIndex - 1));  //Add some 'fill in' characters at the end.
                                nextString += " ";
                            }
                            tmConfig.TROYmarkText.Add(nextString);
                        }
                        else
                        {
                            tmConfig.TROYmarkText.Add(TMString);
                        }
                    }


                    //1.0.8
                    tmConfig.StandardFontDefinition.SymbolSet = Convert.ToInt32(TmSettings.TroymarkSymbolSet);
                    tmConfig.StandardFontDefinition.FontSpacing = Convert.ToInt32(TmSettings.TroymarkFontSpacing);
                    tmConfig.StandardFontDefinition.Pitch = Convert.ToInt32(TmSettings.TroymarkPitch);
                    tmConfig.StandardFontDefinition.Posture = Convert.ToInt32(TmSettings.TroymarkPosture);
                    tmConfig.StandardFontDefinition.Height = (float)Convert.ToDecimal(TmSettings.TroymarkHeight);
                    tmConfig.StandardFontDefinition.StrokeWeight = Convert.ToInt32(TmSettings.TroymarkStrokeWeight);
                    tmConfig.StandardFontDefinition.Typeface = Convert.ToInt32(TmSettings.TroymarkTypeface);


                    byte[] tmData;
                    if (Interface.GetTroyMarkPcl(tmConfig, out tmData, false))
                    {
                        if (tmData != null)
                        {
                            outbuf.Write(tmData, 0, tmData.Length);
                            if (insertFF)
                            {
                                outbuf.Write(0x1B);
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                if (traceEnabled) WriteToTrace("ERROR: InsertSoftwareTroymark.  Exception: " + ex.Message);
                return false;
            }
        }


    }
}
