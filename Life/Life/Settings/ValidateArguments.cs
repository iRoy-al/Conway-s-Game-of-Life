using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Life
{
    /// <summary>
    /// A class that contains methods to validate arguments the user entered.
    /// </summary>
    static class ValidateArguments
    {
        public static Settings Validate(string[] args)
        {
            Settings settings = new Settings();

            //Creates a list to separate each option
            List<List<string>> separatedSettings = new List<List<string>>();
            int index = 0;

            // Creates new sublist if current args[i] contains "--"
            // Appends to sublist if args[i] is a parameter
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Contains("--"))
                {
                    separatedSettings.Add(new List<string>());
                    if (i != 0)
                    {
                        index++;
                    }
                    separatedSettings[index].Add(args[i]);
                }
                else
                {
                    separatedSettings[index].Add(args[i]);
                }
            }


            int trackNeighbour = -1;
            int trackSurvival = -1;
            int trackBirth = -1;
            // Checks the list if the command line arguments are called. Then validates each argument called
            // For neighbour surival and birth, it tracks the index so neighbour will always be entered before
            // survival and birth
            for (int i = 0; i < separatedSettings.Count; i++)
            {
                try
                {
                    if (separatedSettings[i][0] == "--dimensions")
                    {
                        ValidateDimensions(separatedSettings[i], settings);
                    }
                    else if (separatedSettings[i][0] == "--random")
                    {
                        ValidateRandomFactor(separatedSettings[i], settings);
                    }
                    else if (separatedSettings[i][0] == "--seed")
                    {
                        ValidateInput(separatedSettings[i], settings);
                    }
                    else if (separatedSettings[i][0] == "--output")
                    {
                        ValidateOutput(separatedSettings[i], settings);
                    }
                    else if (separatedSettings[i][0] == "--generations")
                    {
                        ValidateGenerations(separatedSettings[i], settings);
                    }
                    else if (separatedSettings[i][0] == "--memory")
                    {
                        ValidateGenerationalMemory(separatedSettings[i], settings);
                    }
                    else if (separatedSettings[i][0] == "--max-update")
                    {
                        ValidateUpdateRate(separatedSettings[i], settings);
                    }
                    else if (separatedSettings[i][0] == "--periodic")
                    {
                        settings.PeriodicMode = true;
                    }
                    else if (separatedSettings[i][0] == "--step")
                    {
                        settings.StepMode = true;
                    }
                    else if (separatedSettings[i][0] == "--ghost")
                    {
                        settings.GhostMode = true;
                    }
                    else if (separatedSettings[i][0] == "--neighbour")
                    {
                        trackNeighbour = i;
                    }
                    else if (separatedSettings[i][0] == "--survival")
                    {
                        trackSurvival = i;
                    }
                    else if (separatedSettings[i][0] == "--birth")
                    {
                        trackBirth = i;
                    }
                }
                catch(Exception ex)
                {
                    Messages.Error($"({settings.time}) Exception caught - {ex.GetType().Name}: {ex.Message}");
                }
                
            }

            try
            {
                if (trackNeighbour != -1)
                {
                    ValidateNeighbour(separatedSettings[trackNeighbour], settings);
                }
            }
            catch (Exception ex)
            {
                Messages.Error($"({settings.time}) Exception caught - {ex.GetType().Name}: {ex.Message}");
            }

            try
            {
                if (trackSurvival != -1)
                {
                    ValidateRules(separatedSettings[trackSurvival], settings);
                }
            }
            catch (Exception ex)
            {
                Messages.Error($"({settings.time}) Exception caught - {ex.GetType().Name}: {ex.Message}");
            }

            try
            {
                if (trackBirth != -1)
                {
                    ValidateRules(separatedSettings[trackBirth], settings);
                }
            }
            catch (Exception ex)
            {
                Messages.Error($"({settings.time}) Exception caught - {ex.GetType().Name}: {ex.Message}");
            }
            

            return settings;
        }

        /// <summary>
        /// Checks if the argument for dimensions is entered properly
        /// </summary>
        /// <param name="args">The value to be tested</param>
        /// <param name="settings">contains all the parameters that might be required in validation</param>
        private static void ValidateDimensions(List<string> args, Settings settings)
        {
            if (args.Count == 3)
            {
                if (Int32.TryParse(args[1], out int rowsValue))
                {
                    settings.Rows = rowsValue;
                }
                else
                {
                    throw new ArgumentException($"Invalid/Unrecognised Rows parameter ({args[1]})");
                }

                if (Int32.TryParse(args[2], out int columnValue))
                {
                    settings.Columns = columnValue;
                }
                else
                {
                    throw new ArgumentException($"Invalid/Unrecognised Columns parameter ({args[2]})");
                }
            }
            else
            {
                throw new ArgumentException($"Dimensions take only 2 parameters");
            }
        }

        /// <summary>
        /// Checks if randomFactor is entered properly
        /// </summary>
        /// <param name="args">The value to be tested</param>
        /// <param name="settings">contains all the parameters that might be required in validation</param>
        private static void ValidateRandomFactor(List<string> args, Settings settings)
        {
            if (args.Count == 2)
            {
                if (Single.TryParse(args[1], out float randomValue))
                {
                    settings.RandomFactor = randomValue;
                }
                else
                {
                    throw new ArgumentException($"Invalid/Unrecognised Random parameter ({args[1]})");
                }
            }
            else
            {
                throw new ArgumentException($"Random Factor takes only one parameter.");
            }
        }

        /// <summary>
        /// Checks if updateRate is entered properly
        /// </summary>
        /// <param name="args">The value to be tested</param>
        /// <param name="settings">contains all the parameters that might be required in validation</param>
        private static void ValidateUpdateRate(List<string> args, Settings settings)
        {
            if (args.Count == 2)
            {
                if (Single.TryParse(args[1], out float updateValue))
                {
                    settings.UpdateRate = updateValue;
                }
                else
                {
                    throw new ArgumentException($"Invalid/Unrecognised Update Rate parameter ({args[1]})");
                }
            }
            else
            {
                throw new ArgumentException($"Update Rate takes only one parameter.");
            }
        }

        /// <summary>
        /// Checks if generations is entered properly
        /// </summary>
        /// <param name="args">The value to be tested</param>
        /// <param name="settings">contains all the parameters that might be required in validation</param>
        private static void ValidateGenerations(List<string> args, Settings settings)
        {
            if (args.Count == 2)
            {
                if (Int32.TryParse(args[1], out int generationsValue))
                {
                    settings.Generations = generationsValue;
                }
                else
                {
                    throw new ArgumentException($"Invalid/Unrecognised Generations parameter ({args[1]})");
                }
            }
            else
            {
                throw new ArgumentException($"Generations takes only one parameter.");
            }
        }

        /// <summary>
        /// Checks if generational memory is entered properly
        /// </summary>
        /// <param name="args">The value to be tested</param>
        /// <param name="settings">contains all the parameters that might be required in validation</param>
        private static void ValidateGenerationalMemory(List<string> args, Settings settings)
        {
            if (args.Count == 2)
            {
                if (Int32.TryParse(args[1], out int memoryValue))
                {
                    settings.GenerationalMemory = memoryValue;
                }
                else
                {
                    throw new ArgumentException($"Invalid/Unrecognised Memory parameter ({args[1]})");
                }
            }
            else
            {
                throw new ArgumentException($"Memory takes only 1 parameter.");
            }
        }

        /// <summary>
        /// Checks if InputFile is entered properly and checks if the File exists
        /// </summary>
        /// <param name="args">The value to be tested</param>
        /// <param name="settings">contains all the parameters that might be required in validation</param>
        private static void ValidateInput(List<string> args, Settings settings)
        {
            if (args.Count == 2)
            {
                if (File.Exists(args[1]))
                {
                    settings.InputFile = args[1];
                }
                else
                {
                    throw new ArgumentException($"Input file does not exist");
                }
            }
            else
            {
                throw new ArgumentException($"Input File takes only 1 parameter.");
            }
        }

        /// <summary>
        /// Checks if OutputFile is entered properly and
        /// If the args is a valid path
        /// </summary>
        /// <param name="args">The value to be tested</param>
        /// <param name="settings">contains all the parameters that might be required in validation</param>
        private static void ValidateOutput(List<string> args, Settings settings)
        {
            if (args.Count > 1)
            {
                args.RemoveAt(0);
                string path = string.Join(" ", args);
                string fullPath = Path.GetFullPath(path);
                string test1 = Path.GetDirectoryName(path);
                string test2 = Path.GetDirectoryName(fullPath);
                if (Directory.Exists(test1) || Directory.Exists(test2))
                {
                    settings.OutputFile = path;
                }
                else
                {
                    throw new ArgumentException($"File path does not exist");
                }
            }
            else
            {
                throw new ArgumentException($"Please enter parameters for Output File");
            }
        }

        /// <summary>
        /// Checks if Neighbourhood is entered properly
        /// </summary>
        /// <param name="args">The value to be tested</param>
        /// <param name="settings">contains all the parameters that might be required in validation</param>
        private static void ValidateNeighbour(List<string> args, Settings settings)
        {
            if (args.Count == 4)
            {
                settings.NeighbourhoodType = args[1].ToLower();

                if (Int32.TryParse(args[2], out int orderValue))
                {
                    settings.NeighbourhoodOrder = orderValue;
                }
                else
                {
                    throw new ArgumentException("Neighbourhood Order must be a integer value");
                }

                if (args[3].ToLower() == "true")
                {
                    settings.CountCentre = true;
                }
                else if (args[3].ToLower() == "false")
                {
                    settings.CountCentre = false;
                }
                else
                {
                    throw new ArgumentException("Centre Count must be true or false");
                }

            }
            else
            {
                throw new ArgumentException("Please write parameters for Neighbourhood");
            }
        }

        /// <summary>
        /// Checks if Survival/Birth is entered properly
        /// And enters into list to pass to Survival/Birth to set
        /// </summary>
        /// <param name="args">The value to be tested</param>
        /// <param name="settings">contains all the parameters that might be required in validation</param>
        private static void ValidateRules(List<string> args, Settings settings)
        {
            int minRules = 0;
            //Calculate the number of neighbour cells
            int neighbourCells = 0;
            int baseNo = 0;
            int multiplier = 0;

            if (settings.NeighbourhoodType == "Moore")
            {
                baseNo = 8;
            }
            else if (settings.NeighbourhoodType == "VonNeumann")
            {
                baseNo = 4;
            }

            for (int i = 1; i <= settings.NeighbourhoodOrder; i++)
            {
                multiplier += i;
            }
            neighbourCells = baseNo * multiplier;

            if (settings.CountCentre)
            {
                neighbourCells += 1;
            }

            //Validate the args
            List<int> holdValues = new List<int>();
            string holdString = "";
            int countSuccess = 0;

            if (args.Count == 1)
            {
                throw new ArgumentException("Please enter parameters for Survival/Birth");
            }
            else
            {
                for (int i = 0; i < args.Count; i++)
                {
                    if (args[i].Contains("..."))
                    {
                        string DELIM = "...";
                        string[] range = args[i].Split(DELIM);

                        if (Int32.TryParse(range[0], out int minRange) && Int32.TryParse(range[1], out int maxRange))
                        {
                            if (minRange >= minRules && maxRange >= minRules)
                            {
                                if (minRange <= neighbourCells && maxRange <= neighbourCells)
                                {
                                    for (int j = minRange; j <= maxRange; j++)
                                    {
                                        holdValues.Add(j);
                                    }
                                    countSuccess++;
                                }
                                else
                                {
                                    throw new ArgumentOutOfRangeException("", "Survival/Birth parameters must " +
                                    "be less than or equal to the number of neighbouring cells");
                                }
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException("", "Survival/Birth parameters must " +
                                "be a positive integer");
                            }

                        }
                        else
                        {
                            throw new ArgumentException("Survival/Birth parameters must be a single " +
                                "integer or two integers separated by ellipses.");
                        }
                    }
                    else if (Int32.TryParse(args[i], out int rulesValue))
                    {
                        if (rulesValue >= minRules)
                        {
                            if (rulesValue <= neighbourCells)
                            {
                                holdValues.Add(rulesValue);
                                countSuccess++;
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException("", "Survival/Birth parameters must " +
                                "be less than or equal to the number of neighbouring cells");
                            }
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("", "Survival/Birth parameters must " +
                                "be a positive integer");
                        }
                    }
                }
            }

            if(countSuccess == args.Count - 1)
            {
                //Generate string to put into CLI
                for (int i = 1; i < args.Count; i++)
                {
                    holdString += args[i];
                    if (i != args.Count - 1)
                    {
                        holdString += " ";
                    }
                }

                if (args[0] == "--birth")
                {
                    settings.Birth = holdValues;
                    settings.birthString = holdString;
                }
                else if (args[0] == "--survival")
                {
                    settings.Survival = holdValues;
                    settings.survivalString = holdString;
                }
            }
        }
    }
}
