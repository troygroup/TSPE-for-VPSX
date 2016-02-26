using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TroySecurePrintWindows
{
    public class Interface
    {
        public class TroyMarkRegionType
        {
            public int XAnchor;
            public int YAnchor;
            public int Width;
            public int Height;
        }

        public class StandardFontDefinitionType
        {
            public int SymbolSet = 277;
            public int FontSpacing = 0;
            public int Pitch = 9;
            public float Height = 11.5f;
            public int Posture = 0;  //Upright default 0 - Upright, 1 - Italic, 2 - Alternate Italic
            public int StrokeWeight = 0;
            public int Typeface = 48;
        }


        public enum TROYmarkPatternType
        {
            tmDark,
            tmMedium,
            tmLight
        }

        public class TROYmarkConfiguration
        {
            public List<string> TROYmarkText = new List<string>();
            public TroyMarkRegionType InclusionRegion;
            public List<TroyMarkRegionType> ExclusionRegion = new List<TroyMarkRegionType>();
            public int LineSpacing = 300;
            public float ExtraSpacing = 0.3f;
            public StandardFontDefinitionType StandardFontDefinition = new StandardFontDefinitionType();
            public TROYmarkPatternType Pattern = TROYmarkPatternType.tmDark;
            public int SpacePerCharacter = 75;  //Use this to increase/decrease number of time strings is added to the print job
            public bool InsertEndPageReset = false;  //Set to true is <ESC>E<ESC>%-12345X
        }


        public static bool GetTroyMarkPcl(TROYmarkConfiguration tmConfig, out byte[] PclData, bool ReverseDir)
        {
            string PclPattern;
            string PclString;

            int AdjustedLineSpacing = (int)(((float)tmConfig.LineSpacing / 600) * 1016);

            if (tmConfig.TROYmarkText.Count == 0)
            {
                PclData = null;
                return false;
            }

            //            PclPattern = "\u001BE";
            PclPattern = "\u001B*c4999G";  //Define a Pattern Id
            PclPattern += "\u001B*c44W";   //Define the Pattern

            //THE PATTERN DEFINED FOR THE ORIGINAL TROYMARK (DARK)
            //            byte[] SuperDarkPatternBytes = new byte[] {0x14,0x00,0x01,0x00,0x00,0x10,0x00,0x10,0x02,0x58,0x02,0x58,
            //                                              0x00,0x00,0xAA,0xAA,0x00,0x00,0x88,0x88,0x00,0x00,0xAA,0xAA,0x00,0x00,0x88,0x88,
            //                                              0x00,0x00,0xAA,0xAA,0x00,0x00,0x88,0x88,0x00,0x00,0xAA,0xAA,0x00,0x00,0x88,0x88};
            byte[] SuperDarkPatternBytes = new byte[] {0x14,0x00,0x01,0x00,0x00,0x10,0x00,0x10,0x02,0x58,0x02,0x58,
                                              0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,
                                              0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA};

            byte[] DarkPatternBytes = new byte[] {0x14,0x00,0x01,0x00,0x00,0x10,0x00,0x10,0x02,0x58,0x02,0x58,
                                              0x00,0x00,0x22,0x22,0x00,0x00,0x88,0x88,0x00,0x00,0x22,0x22,0x00,0x00,0x88,0x88,
                                              0x00,0x00,0x22,0x22,0x00,0x00,0x88,0x88,0x00,0x00,0x22,0x22,0x00,0x00,0x88,0x88};

            byte[] LightPatternBytes = new byte[] {0x14,0x00,0x01,0x00,0x00,0x10,0x00,0x10,0x02,0x58,0x02,0x58,
                                              0x00,0x00,0x00,0x00,0x00,0x00,0x88,0x88,0x00,0x00,0x00,0x00,0x00,0x00,0x88,0x88,
                                              0x00,0x00,0x00,0x00,0x00,0x00,0x88,0x88,0x00,0x00,0x00,0x00,0x00,0x00,0x88,0x88};

            byte[] MediumPatternBytes = new byte[] {0x14,0x00,0x01,0x00,0x00,0x10,0x00,0x10,0x02,0x58,0x02,0x58,
                                              0x00,0x00,0x02,0x01,0x00,0x00,0x88,0x88,0x00,0x00,0x20,0x20,0x00,0x00,0x88,0x88,
                                              0x00,0x00,0x02,0x01,0x00,0x00,0x88,0x88,0x00,0x00,0x20,0x20,0x00,0x00,0x88,0x88};


            PclString = "\u001B&f3931Y"; //Set macro id
            PclString += "\u001B&f0X";  //Start macro
            PclString += "\u001B&f0S"; //Push cursor position
            PclString += "\u001B&u600D"; //Set dpi to 600
            PclString += "\u001B*p0x0Y";  //Cursor position
            PclString += "\u001B*p1R"; //Keep patterns fixed

            //HPGL2 units are 1/720 inch
            int adjustedWidth = (int)(((float)tmConfig.InclusionRegion.Width / 600) * 720);
            int adjustedHeight = (int)(((float)tmConfig.InclusionRegion.Height / 600) * 720);

            //Define the picture window
            PclString += "\u001B*c" + adjustedWidth.ToString() + "x" + adjustedHeight.ToString() + "Y";
            //Set the PCL X,Y PCL position to the X and Y anchors
            PclString += "\u001B*p" + tmConfig.InclusionRegion.XAnchor.ToString() + "x";
            PclString += tmConfig.InclusionRegion.YAnchor.ToString() + "Y";
            PclString += "\u001B*c0T";   //Set Picture Frame Anchor Point

            PclString += "\u001B*c0K";  //Horizontal HP-GL/2 Plot Size
            PclString += "\u001B*c0L";  //Vertical HP-GL/2 Plot Size
            PclString += "\u001B%1B"; //Enter HP-GL/2 mode.
            PclString += "IN;"; //Initialize HP-GL/2 mode.
            PclString += "SP1;";  //Select pen number 1.  Used to enable printing.

            PclString += "SD1," + tmConfig.StandardFontDefinition.SymbolSet.ToString();
            PclString += ",2," + tmConfig.StandardFontDefinition.FontSpacing;
            PclString += ",3," + tmConfig.StandardFontDefinition.Pitch.ToString();
            PclString += ",4," + tmConfig.StandardFontDefinition.Height.ToString();
            PclString += ",5," + tmConfig.StandardFontDefinition.Posture.ToString();
            PclString += ",6," + tmConfig.StandardFontDefinition.StrokeWeight.ToString();
            PclString += ",7," + tmConfig.StandardFontDefinition.Typeface.ToString() + ";"; //Font definition
            PclString += "ES" + tmConfig.ExtraSpacing.ToString() + ";";  //Extra Spacing between characters
            PclString += "TR1;";  //Transparency Mode on
            PclString += "FT22,4999;"; //Fill Type = PCL User Defined,Pattern ID = 4999
            PclString += "CF2;";  //Character fill using the Fill Type (FT command)        
            if (!ReverseDir)
            {
                PclString += "DI1,1;"; // Direction = 45 degrees
            }
            else
            {
                PclString += "DI1,-1;"; // Direction = 45 degrees
            }
            int XAdjustment = 70;
            float SpacingAdj = (float)tmConfig.LineSpacing / 1016;
            float HeightAdj = (float)tmConfig.InclusionRegion.Height / 600;
            int YMovement = (int)(HeightAdj / SpacingAdj);
            if (YMovement == 0)
            {
                YMovement = 1;
            }
            YMovement++; //Add one just to be safe.
            float WidthAdj = (float)tmConfig.InclusionRegion.Width / 600;
            int XMovement = (int)(WidthAdj / SpacingAdj);
            if (XMovement == 0)
            {
                XMovement = 1;
            }
            XMovement++;  //Add one just to be safe

            //Determine a rough estimate on how many times the string need to be printed.
            //  Not looking for an exact number,  trying not to put way too many characters in the print job
            //  Assuming a character is 1/8 inch wide 
            int totalSize;
            int hStrCount;
            int vStrCount;
            int HeightBase;
            int DiagonalLength;
            if (tmConfig.InclusionRegion.Height < tmConfig.InclusionRegion.Width)
            {
                HeightBase = tmConfig.InclusionRegion.Height;
            }
            else
            {
                HeightBase = tmConfig.InclusionRegion.Width;
            }
            DiagonalLength = (HeightBase * HeightBase) + (HeightBase * HeightBase);
            DiagonalLength = Convert.ToInt32(Math.Pow(Convert.ToDouble(DiagonalLength), 0.5f));

            int strCntr = 0;
            for (int cntr = 0; cntr < YMovement; cntr++)
            {
                string str = tmConfig.TROYmarkText[strCntr];
                //int tempcntr = cntr * tmConfig.LineSpacing;
                int tempcntr = cntr * AdjustedLineSpacing;
                PclString += "PU" + XAdjustment.ToString() + "," + tempcntr.ToString() + ";";
                PclString += "LB";

                totalSize = tmConfig.SpacePerCharacter * str.Length;
                vStrCount = DiagonalLength / (totalSize + 1);
                for (int lpcntr = 0; lpcntr < vStrCount + 1; lpcntr++)
                {
                    PclString += str;
                }

                PclString += "\u0003";

                if (++strCntr >= tmConfig.TROYmarkText.Count)
                {
                    strCntr = 0;
                }
            }
            for (int cntr = 1; cntr < XMovement; cntr++)
            {
                string str = tmConfig.TROYmarkText[strCntr];
                //int tempcntr = (cntr * tmConfig.LineSpacing) + XAdjustment;
                int tempcntr = (cntr * AdjustedLineSpacing) + XAdjustment;
                PclString += "PU" + tempcntr.ToString() + ",0;";
                PclString += "LB";
                totalSize = tmConfig.SpacePerCharacter * str.Length;
                hStrCount = DiagonalLength / (totalSize + 1);
                for (int lpcntr = 0; lpcntr < hStrCount + 1; lpcntr++)
                {
                    PclString += str;
                }

                PclString += "\u0003";

                if (++strCntr >= tmConfig.TROYmarkText.Count)
                {
                    strCntr = 0;
                }
            }

            PclString += "\u001B%1A";  //Enter PCL with cursor at current HPGL2 position

            //Exclusion
            foreach (TroyMarkRegionType exc in tmConfig.ExclusionRegion)
            {
                if ((exc.XAnchor > 0) || (exc.YAnchor > 0) ||
                    (exc.Width > 0) || (exc.Height > 0))
                {
                    int XAnchor = tmConfig.InclusionRegion.XAnchor + exc.XAnchor;
                    int YAnchor = tmConfig.InclusionRegion.YAnchor + exc.YAnchor;
                    PclString += "\u001B*p" + XAnchor.ToString() + "x" + YAnchor.ToString() + "Y";
                    PclString += "\u001B*c" + exc.Width.ToString() + "a" + exc.Height.ToString() + "B";
                    PclString += "\u001B*c1P";
                }
            }
            PclString += "\u001B&f1S"; //Pop cursor position
            PclString += "\u001B&f1X"; // Stop Macro
            PclString += "\u001B&f9X"; // Make Macro Temporary
            PclString += "\u001B&f3X"; // Call Macro
            PclString += "\u001B&f8X"; // Delete Macro


            byte[] ptnHdr = new UTF8Encoding(true).GetBytes(PclPattern);
            byte[] tempBytes = new UTF32Encoding().GetBytes(PclString);
            byte[] txtPcl = Convert32To8(tempBytes);

            byte[] PatternBytes;
            if (tmConfig.Pattern == TROYmarkPatternType.tmLight)
            {
                PatternBytes = MediumPatternBytes;
            }
            else if (tmConfig.Pattern == TROYmarkPatternType.tmMedium)
            {
                PatternBytes = DarkPatternBytes;
            }
            else
            {
                PatternBytes = SuperDarkPatternBytes;
            }

            int ArrayLength = ptnHdr.Length + txtPcl.Length + PatternBytes.Length;
            byte[] EndPageReset = new byte[] { 0x1B, 0x45, 0x1B, 0x25, 0x2D, 0x31, 0x32, 0x33, 0x34, 0x35, 0x58 };
            if (tmConfig.InsertEndPageReset)
            {
                ArrayLength += EndPageReset.Length;
            }

            PclData = new byte[ArrayLength];
            Array.Copy(ptnHdr, 0, PclData, 0, ptnHdr.Length);
            Array.Copy(PatternBytes, 0, PclData, ptnHdr.Length, PatternBytes.Length);
            Array.Copy(txtPcl, 0, PclData, ptnHdr.Length + PatternBytes.Length, txtPcl.Length);

            if (tmConfig.InsertEndPageReset)
            {
                Array.Copy(EndPageReset, 0, PclData, ptnHdr.Length + PatternBytes.Length + txtPcl.Length, EndPageReset.Length);
            }

            return true;
        }

        private static byte[] Convert32To8(byte[] inBytes)
        {
            try
            {
                byte[] outbytes = new byte[inBytes.Length / 4];
                int cntr2 = 0;
                for (int cntr = 0; cntr < inBytes.Length; cntr = cntr + 4)
                {
                    outbytes[cntr2++] = inBytes[cntr];
                }
                return outbytes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
