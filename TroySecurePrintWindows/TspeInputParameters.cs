using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TroySecurePrintWindows
{
    public class Diagnostics
    {
        public string TraceFile = "";
        public string ErrorLogFile = "";
        public string BackupErrorFiles = "";
        public string BackupAllFiles = "";
    }

    public class TspeInputParameters
    {
        public string DefaultPantographLocation = "";
        public string DefaultTroymarkConfigFile = ""; //Located in Configuration folder
        public string InsertAfterFeatures = "";
        public bool TroyCommandsIncluded = false;
        public bool PjlTroymarkData = true;
        public List<string> PjlTmDataTags = new List<string>();
        public Diagnostics DiagSettings;
        public bool IncludeCodeOnPrintout = false;
        public bool RemoveLongShortPageOffset = false;
    }
}
