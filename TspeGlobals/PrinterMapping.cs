using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TspeGlobals
{
    public class PrinterMap
    {
        public string PrinterName = "";
        public string SiteName = "";
        public string TroymarkConfig = "";
        public string PantographPath = "";
    }

    public class PrinterMapping
    {
        public List<PrinterMap> PrinterMapList = new List<PrinterMap>();

        public void AddNewEntry(PrinterMap pmap)
        {
            foreach (PrinterMap pm in PrinterMapList)
            {
                if (pm.PrinterName == pmap.PrinterName)
                {
                    pm.PantographPath = pmap.PantographPath;
                    pm.SiteName = pmap.SiteName;
                    pm.TroymarkConfig = pmap.TroymarkConfig;
                    return;
                }
            }
            PrinterMapList.Add(pmap);
        }

        public bool EntryExist(string PrinterName)
        {
            foreach (PrinterMap pm in PrinterMapList)
            {
                if (pm.PrinterName == PrinterName)
                {
                    return true;
                }
            }
            return false;
        }

        public bool DeleteEntry(string PrinterName)
        {
            foreach (PrinterMap pm in PrinterMapList)
            {
                if (pm.PrinterName == PrinterName)
                {
                    PrinterMapList.Remove(pm);
                    return true;
                }
            }
            return false;
        }

    }
}
