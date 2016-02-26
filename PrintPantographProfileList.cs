                                    if (pmConfig.PrintPantographProfileList != "")
                                    {
                                        string dataFileName = "";
                                        
                                        foreach (string substring in pmConfig.PrintPantographProfileList.Split(','))
                                        {
                                            // only use page 1 pcl because page 2 pcl has problems
                                            // there no real advantage of creating page 2 other than downloading font to printer
                                            dataFileName = PantographDataFilesLocation + "PantographProfile" + substring + "Page1.pcl";
                                              if (File.Exists(dataFileName))
                                            {
                                                FileStream pgFile = new FileStream(dataFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                                                BinaryReader binReader = new BinaryReader(pgFile);
                                                byte[] inBuffer = new Byte[pgFile.Length];
                                                inBuffer = binReader.ReadBytes(Convert.ToInt32(pgFile.Length));
                                                outbuf.Write(inBuffer, 0, inBuffer.Length);
                                                binReader.Close();
                                            }
                                            else
                                            {
                                                pmLogging.LogError("Error.  Can not find Pantograph data file: " + dataFileName, EventLogEntryType.Error, false);
                                            }
                                        }
                                    }
