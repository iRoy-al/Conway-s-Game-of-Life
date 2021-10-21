using System;
using System.Collections.Generic;
using System.Text;

namespace Life
{
    /// <summary>
    /// A class representing and containing all the Settings Options.
    /// </summary>
    class Settings
    {
        public string time = DateTime.Now.ToLongTimeString();

        private int rows = 16;
        private int columns = 16;
        private int minDimension = 4;
        private int maxDimension = 48;

        private float randomFactor = 0.5F;
        private float minrandomFactor = 0F;
        private float maxRandomFactor = 1F;
        
        private float updateRate = 5F;
        private float minUpdateRate = 1F;
        private float maxUpdateRate = 30F;

        private int generations = 50;
        private int minGenerations = 1;

        private int generationalMemory = 16;
        private int minMemory = 4;
        private int maxMemory = 512;

        private string inputFile = "N/A";
        private string outputFile = "N/A";

        private string neighbourhoodType = "Moore";
        private int neighbourhoodOrder = 1;
        private bool countCentre = false;

        private List<int> survival = new List<int>() { 2, 3 };
        public string survivalString;
        private List<int> birth = new List<int>() { 3 };
        public string birthString;

        private bool ghostMode = false;
        private bool stepMode = false;
        private bool periodic = false;

        public Settings()
        {
            survivalString = Survival[0] + "..." + Survival[1];
            birthString = Birth[0] + "";
        }

        /// <summary>
        /// Allows to get and set the value of GhostMode
        /// </summary>
        /// <returns>The current value of ghostMode</returns>
        public bool GhostMode
        {
            get { return ghostMode; }
            set
            {
                if (value == true)
                {
                    Messages.Success($"({time}) Ghost Mode has successfully been turned On.");
                    ghostMode = true;
                }
                else
                {
                    ghostMode = false;
                }
            }
        }

        /// <summary>
        /// Allows to get and set the value of stepMode
        /// </summary>
        /// <returns>The current value of stepMode</returns>
        public bool StepMode
        {
            get { return stepMode; }
            set
            {
                if (value == true)
                {
                    Messages.Success($"({time}) Step Mode has successfully been turned On.");
                    stepMode = true;
                }
                else
                {
                    stepMode = false;
                }
            }
        }

        /// <summary>
        /// Allows to get and set the value of periodic
        /// </summary>
        /// <returns>The current value of periodic</returns>
        public bool PeriodicMode
        {
            get { return periodic; }
            set
            {
                if (value == true)
                {
                    Messages.Success($"({time}) Periodic Mode has successfully been turned On.");
                    periodic = value;
                }
                else
                {
                    periodic = false;
                }
            }
        }

        /// <summary>
        /// Allows the ability to get and set the value of birth
        /// </summary>
        /// <returns>The values of birth</returns>
        public List<int> Birth
        {
            get { return birth; }
            set
            {
                Messages.Success($"({time}) Birth has successfully been changed.");
                birth = value;

            }
        }

        /// <summary>
        /// Allows the ability to get and set the value of survival
        /// </summary>
        /// <returns>The values of survival</returns>
        public List<int> Survival
        {
            get { return survival; }
            set
            {
                Messages.Success($"({time}) Survival has successfully been changed.");
                survival = value;
            }
        }

        /// <summary>
        /// Allows the ability to get and set the value of countCentre
        /// </summary>
        /// <returns>The current value of countCentre</returns>
        public bool CountCentre
        {
            get { return countCentre; }
            set
            {
                Messages.Success($"({time}) Centre Count has successfully been turned On.");
                countCentre = value;
            }
        }

        /// <summary>
        /// Allows the ability to get and set the value of neighbourhoodOrder
        /// Checks if its between 1 and 10 AND less than half the smallest dimension
        /// </summary>
        /// <returns>The current value of neighbourhoodOrder</returns>
        public int NeighbourhoodOrder
        {
            get { return neighbourhoodOrder; }
            set
            {
                if (value >= 1 && value <= 10)
                {
                    if (value < (rows / 2) && value < (columns / 2))
                    {
                        Messages.Success($"({time}) Neighbourhood Order has successfully been changed.");
                        neighbourhoodOrder = value;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("", "Neighbourhood Order must be " +
                            "less than half of the smallest dimensions.");
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("", "Neighbourhood Order must be between 1 and 10.");
                }

            }
        }

        /// <summary>
        /// Allows the ability to get and set the value of neighbourhoodType
        /// Checks if its one of the viable types
        /// </summary>
        /// <returns>The current value of neighbourhoodType</returns>
        public string NeighbourhoodType
        {
            get { return neighbourhoodType; }
            set
            {
                if (value == "moore")
                {
                    Messages.Success($"({time}) Neighbourhood Type has successfully been changed.");
                    neighbourhoodType = "Moore";
                }
                else if (value == "vonneumann")
                {
                    Messages.Success($"({time}) Neighbourhood Type has successfully been changed.");
                    neighbourhoodType = "VonNeumann";
                }
                else
                {
                    throw new ArgumentException($"({value}) is not a valid Neighbourhood Type.");
                }
            }
        }

        /// <summary>
        /// Allows the ability to get and set the value of outputFile
        /// Also checks if it ends with a .seed extension
        /// </summary>
        /// <returns>The current value of outputFile</returns>
        public string OutputFile
        {
            get { return outputFile; }
            set
            {
                if (value.EndsWith(".seed"))
                {
                    Messages.Success($"({time}) Output file has successfully been accepted.");
                    outputFile = value;
                }
                else
                {
                    throw new ArgumentException($"Invalid File. File must be a .seed file.");
                }
            }
        }

        /// <summary>
        /// Allows the ability to get and set the value of inputFile
        /// Also checks if it ends with a .seed extension
        /// </summary>
        /// <returns>The current value of inputFile</returns>
        public string InputFile
        {
            get { return inputFile; }
            set
            {
                if (value.EndsWith(".seed"))
                {
                    Messages.Success($"({time}) Input file has successfully been accepted.");
                    inputFile = value;
                }
                else
                {
                    throw new ArgumentException($"Invalid File. File must be a .seed file.");
                }
            }
        }

        /// <summary>
        /// Allows the ability to get and set the value of generationalMemory
        /// Checks if the value is a an int between 4 and 512
        /// </summary>
        /// <returns>The current value of generationalMemory</returns>
        public int GenerationalMemory
        {
            get { return generationalMemory; }
            set
            {
                if (value >= minMemory && value <= maxMemory)
                {
                    Messages.Success($"({time}) Generational Memory has successfully been changed.");
                    generationalMemory = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("", "Generational Memory must be between 4 and 512.");
                }
            }
        }

        /// <summary>
        /// Allows the ability to get and set the value of generations
        /// Checks if the value is a non-zero positive integer
        /// </summary>
        /// <returns>The current value of generations</returns>
        public int Generations
        {
            get { return generations; }
            set
            {
                if (value >= minGenerations)
                {
                    Messages.Success($"({time}) Generations has successfully been changed.");
                    generations = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("", "Generations parameter must be a non-zero positive integer.");
                }
            }
        }

        /// <summary>
        /// Allows the ability to get and set the value of updateRate
        /// Checks if value is between default values
        /// </summary>
        /// <returns>The current value of updateRate</returns>
        public float UpdateRate
        {
            get { return updateRate; }
            set
            {
                if (value >= minUpdateRate && value <= maxUpdateRate)
                {
                    Messages.Success($"({time}) Update Rate has successfully been changed.");
                    updateRate = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("", "UpdateRate parameter must be between 1 and 30.");
                }
            }
        }

        /// <summary>
        /// Allows the ability to get and set the value of randomFactor
        /// If the value is between 0 and 1, it sets randomFactor.
        /// </summary>
        /// <returns>The current value of randomFactor</returns>
        public float RandomFactor
        {
            get { return randomFactor; }
            set
            {
                if (value >= minrandomFactor && value <= maxRandomFactor)
                {
                    Messages.Success($"({time}) Random Factor has successfully been changed.");
                    randomFactor = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("", "Random parameter must be between 0 and 1.");
                }
            }
        }

        /// <summary>
        /// Allows the ability to get and set the value of rows
        /// Checks if the value is between the defaults
        /// </summary>
        /// <returns>The current value of rows</returns>
        public int Rows
        {
            get { return rows; }
            set
            {
                if (value >= minDimension && value <= maxDimension)
                {
                    Messages.Success($"({time}) Rows has successfully been changed.");
                    rows = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("", "Rows must be between 4 and 48.");
                }
            }
        }

        /// <summary>
        /// Allows the ability to get and set the value of columns
        /// Checks if the value is between the defaults
        /// </summary>
        /// <returns>The current value of columns</returns>
        public int Columns
        {
            get { return columns; }
            set
            {
                if (value >= minDimension && value <= maxDimension)
                {
                    Messages.Success($"({time}) Columns has successfully been changed.");
                    columns = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("", "Columns must be between 4 and 48.");
                }
            }
        }

        /// <summary>
        /// Formats and Writes all the values of the Settings when called.
        /// </summary>
        public override string ToString()
        {

            string periodicSetting;
            string stepSetting;
            string ghostSetting;
            string countCentreSetting;
            const int alignValue = 29;

            if (periodic == false)
            {
                periodicSetting = "Off";
            }
            else
            {
                periodicSetting = "On";
            }

            if (stepMode == false)
            {
                stepSetting = "Off";
            }
            else
            {
                stepSetting = "On";
            }

            if (ghostMode == false)
            {
                ghostSetting = "Off";
            }
            else
            {
                ghostSetting = "On";
            }

            if (countCentre == false)
            {
                countCentreSetting = "";
            }
            else
            {
                countCentreSetting = "(centre-counted)";
            }

            return $"The program will use the following settings:\n\n"
                + $"{"Input File:",alignValue} {inputFile}\n"
                + $"{"Output File:",alignValue} {outputFile}\n"
                + $"{"Dimensions:",alignValue} R{rows} x C{columns}\n"
                + $"{"Generations:",alignValue} {generations}\n"
                + $"{"Generational Memory:",alignValue} {generationalMemory}\n"
                + $"{"Neighbourhood:",alignValue} {neighbourhoodType} ({neighbourhoodOrder}) {countCentreSetting}\n"
                + $"{"Rules:",alignValue} S( {survivalString} ) B( {birthString} )\n"
                + $"{"Step Mode:",alignValue} {stepSetting}\n"
                + $"{"Periodic Mode:",alignValue} {periodicSetting}\n"
                + $"{"Ghost Mode:",alignValue} {ghostSetting}\n"
                + $"{"Random Factor:",alignValue} {randomFactor:P}\n"
                + $"{"Update Rate:",alignValue} {updateRate} updates/s\n"
                ;
        }
    }
}
