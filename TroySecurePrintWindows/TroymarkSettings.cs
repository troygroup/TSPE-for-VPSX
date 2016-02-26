using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TroySecurePrintWindows
{
    public class TroymarkSettings
    {
        public bool TroymarkEnabled = false;
        public string TroymarkPattern = "Medium";
        public string TroymarkInclusion = "0,0,4800,6400";
        public string TroymarkExclusion = "";
        public string TroymarkSpacing = "300";
        public string TroymarkCharsPerLine = "70";
        public bool TroymarkOnBack = false;
        public string TroymarkStaticText = "";
        public string TroymarkSymbolSet = "298";
        public string TroymarkFontSpacing = "0";
        public string TroymarkPitch = "9";
        public string TroymarkHeight = "11.5";
        public string TroymarkPosture = "0";
        public string TroymarkStrokeWeight = "0";
        public string TroymarkTypeface = "48";
        public List<string> PjlTmDataTags = new List<string>();

    }
}
