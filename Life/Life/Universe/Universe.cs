using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Life
{
    /// <summary>
    /// A class representing the behavior of the simulation.
    /// </summary>
    class Universe
    {
        private int[][,] memory;
        private bool steadyState = false;
        private int steadyStateIndex = -1;
        private Settings settings;

        /// <summary>
        /// Constructs a Game Behavior instance with the current grid dimensions.
        /// </summary>
        /// <param name="rows">Total number of rows for current simulation</param>
        /// <param name="columns">Total number of columns for current simulation</param>
        /// <param name="settings">contains all the parameters that might be required in validation</param>
        public Universe(Settings setting)
        {
            this.settings = setting;
            memory = new int[settings.GenerationalMemory][,];
            for (int i = 0; i < memory.Length; i++)
            {
                memory[i] = new int[settings.Rows, settings.Columns];
            }
        }

        /// <summary>
        /// Enables to get if steady state has been found.
        /// </summary>
        public bool SteadyState
        {
            get
            {
                return steadyState;
            }
        }

        /// <summary>
        /// Enables to get if steady state has been found.
        /// </summary>
        public int SteadyStateIndex
        {
            get
            {
                return steadyStateIndex;
            }
        }

        /// <summary>
        /// Checks the memory for steady state
        /// </summary>
        /// <param name="universeToCheck">The universe to be checked against the memory</param>
        public void CheckSteadyState(int[,] universeToCheck)
        {
            int match = -1;
            for (int i = 0; i < memory.Length; i++)
            {
                match = 0;
                for (int r = 0; r < settings.Rows; r++)
                {
                    for (int c = 0; c < settings.Columns; c++)
                    {
                        if (memory[i][r,c] != universeToCheck[r,c])
                        {
                            match = 1;
                            break;
                        }
                        else if (r == settings.Rows - 1 && c == settings.Columns - 1)
                        {
                            steadyStateIndex = i;
                            steadyState = true;
                        }
                    }
                    if (match == 1)
                    {
                        break;
                    }
                    else if (match == 2)
                    {
                        break;
                    }
                }
                if (match == 2)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Stores the most recent generation of the universe in the memory to 
        /// a limit specified by settings.GenerationalMemory
        /// </summary>
        /// <param name="universe">The current universe to be stored in the memory</param>
        public void StoreGeneration(int[,] universe)
        {
            //Shifts every generation back before storing the most recent
            for (int j = 0; j < memory.Length - 1; j++)
            {
                memory[j] = memory[j + 1];
            }
            memory[settings.GenerationalMemory - 1] = universe;
        }

        /// <summary>
        /// Decides if the current cell lives or dies based on these:
        /// If the cell is currently alive - check survival for the numbers needed to stay alive
        /// If the cell is not alive - check birth for the numbers needed to live
        /// </summary>
        /// <param name="universe">the current universe being checked</param>
        /// <param name="row">Current row positioned at</param>
        /// <param name="column">Current column positioned at</param>
        /// <returns>grantLife which means that the cell survives, resurrects, or dies</returns>
        private bool CheckNeighbours(int[,] universe, int row, int column)
        {
            int liveNeighbours = 0;
            bool currentlyAlive = false;
            bool grantLife = false;

            if (settings.NeighbourhoodType == "Moore")
            {
                Moore moore = new Moore();
                liveNeighbours = moore.NeighbourCheck(universe, row, column, settings);
            }
            else if (settings.NeighbourhoodType == "VonNeumann")
            {
                VonNeumann vonNeumann = new VonNeumann();
                liveNeighbours = vonNeumann.NeighbourCheck(universe, row, column, settings);
            }
            // Check if currently alive
            if (universe[row, column] == 1)
            {
                currentlyAlive = true;
                if (settings.CountCentre)
                {
                    liveNeighbours++;
                }
            }

            if (currentlyAlive)
            {
                if (settings.Survival.Contains(liveNeighbours))
                {
                    grantLife = true;
                }
            }
            else
            {
                if (settings.Birth.Contains(liveNeighbours))
                {
                    grantLife = true;
                }
            }
            return grantLife;
        }

        /// <summary>
        /// Uses the current generation of cells and processes it 
        /// using rules explained in the CheckNeighboursNP method
        /// to generate the next generation of cells
        /// </summary>
        public int[,] GenerateNextGen(int[,] universe)
        {
            int[,] updatedUniverse = new int[settings.Rows, settings.Columns];

            for (int r = 0; r < settings.Rows; r++)
            {
                for(int c = 0; c < settings.Columns; c++)
                {
                    if (CheckNeighbours(universe, r, c))
                    {
                        updatedUniverse[r, c] = 1;
                    }

                }
            }
            CheckSteadyState(updatedUniverse);
            StoreGeneration(updatedUniverse);
            return updatedUniverse;
        }

        /// <summary>
        /// Generates the first generation of cells
        /// If there is no input file then the it will be randomly generated
        /// </summary>
        /// <returns>The generated primordial universe</returns>
        public int[,] GeneratePrimordialCells()
        {
            int[,] universe = new int[settings.Rows, settings.Columns];

            if (settings.InputFile != "N/A")
            {
                universe = GenerateFromFile();
            }
            else
            {
                universe = GenerateRandom();
            }
            
            return universe;
        }

        /// <summary>
        /// Generates the first generation of cells
        /// using data from the seed file
        /// </summary>
        /// <returns>The generated primordial universe</returns>
        public int[,] GenerateFromFile()
        {
            int[,] universe = new int[settings.Rows, settings.Columns];

            using (StreamReader reader = new StreamReader(settings.InputFile))
            {
                string line = reader.ReadLine();
                if (line == "#version=1.0")
                {
                    universe = GenFileV1();
                }
                else if (line == "#version=2.0")
                {
                    universe = GenFileV2();
                }
            }
            return universe;
        }

        /// <summary>
        /// Generates the first generation of cells
        /// Using data from the seed file, if it is version 1
        /// </summary>
        /// <returns>The generated primordial universe</returns>
        public int[,] GenFileV1()
        {
            int[,] universe = new int[settings.Rows, settings.Columns];

            using (StreamReader reader = new StreamReader(settings.InputFile))
            {
                string line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    string[] coordinates = line.Split(' ');
                    Int32.TryParse(coordinates[0], out int row);
                    Int32.TryParse(coordinates[1], out int column);
                    if (row < settings.Rows && row > 0 && column < settings.Columns && column > 0)
                    {
                        universe[row, column] = 1;
                    }
                }
            }
            return universe;
        }

        /// <summary>
        /// Generates the first generation of cells
        /// Using data from the seed file, if it is version 2
        /// </summary>
        /// <returns>The generated primordial universe</returns>
        public int[,] GenFileV2()
        {
            int[,] universe = new int[settings.Rows, settings.Columns];

            using (StreamReader reader = new StreamReader(settings.InputFile))
            {
                string line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    string[] coordinates = line.Split(' ');

                    for (int i = 2; i < coordinates.Length; i++)
                    {
                        if (coordinates[i].Contains(","))
                        {
                            coordinates[i] = coordinates[i].Replace(",", "");
                        }
                    }

                    if (coordinates[1] == "cell:")
                    {
                        Int32.TryParse(coordinates[2], out int row);
                        Int32.TryParse(coordinates[3], out int column);
                        if (row < settings.Rows && row > 0 && column < settings.Columns && column > 0)
                        {
                            universe[row, column] = 1;
                        }
                    }

                    if (coordinates[1].Contains("rectangle") || coordinates[1].Contains("ellipse"))
                    {
                        // Holds rectangle or ellipse values to make easier to iterate over
                        List<int> holdValues = new List<int>();

                        for (int i = 0; i < coordinates.Length; i++)
                        {
                            if (Int32.TryParse(coordinates[i], out int number))
                            {
                                holdValues.Add(number);
                            }
                        }

                        for (int r = holdValues[0]; r <= holdValues[2]; r++)
                        {
                            for (int c = holdValues[1]; c <= holdValues[3]; c++)
                            {
                                if (coordinates[1].Contains("rectangle"))
                                {
                                    if (coordinates[0].Contains("(o)"))
                                    {
                                        if (r < settings.Rows && r > 0 && c < settings.Columns && c > 0)
                                        {
                                            universe[r, c] = 1;
                                        }

                                    }
                                    else if (coordinates[0].Contains("(x)"))
                                    {
                                        if (r < settings.Rows && r > 0 && c < settings.Columns && c > 0)
                                        {
                                            universe[r, c] = 0;
                                        }
                                    }
                                }
                                else if (coordinates[1].Contains("ellipse"))
                                {
                                    double x = r;
                                    double y = c;

                                    double x1 = holdValues[0];
                                    double y1 = holdValues[1];

                                    double x2 = holdValues[2];
                                    double y2 = holdValues[3];

                                    double x0 = (x1 + x2) / 2;
                                    double y0 = (y1 + y2) / 2;

                                    double dx = (x2 - x1) + 1;
                                    double dy = (y2 - y1) + 1;

                                    double answerX = (4 * Math.Pow(x - x0, 2)) / Math.Pow(dx, 2);
                                    double answerY = (4 * Math.Pow(y - y0, 2)) / Math.Pow(dy, 2);
                                    double answer = answerX + answerY;

                                    if (answer <= 1)
                                    {
                                        if (coordinates[0].Contains("(o)"))
                                        {
                                            if (r < settings.Rows && r > 0 && c < settings.Columns && c > 0)
                                            {
                                                universe[r, c] = 1;
                                            }
                                        }
                                        else if (coordinates[0].Contains("(x)"))
                                        {
                                            if (r < settings.Rows && r > 0 && c < settings.Columns && c > 0)
                                            {
                                                universe[r, c] = 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return universe;
        }

        /// <summary>
        /// Generates the first generation of cells randomly
        /// </summary>
        /// <returns>The generated primordial universe</returns>
        public int[,] GenerateRandom()
        {
            int[,] universe = new int[settings.Rows, settings.Columns];
            
            Random random = new Random();
            float chance = 1 - settings.RandomFactor;
            for (int r = 0; r < settings.Rows; r++)
            {
                for (int c = 0; c < settings.Columns; c++)
                {
                    if (random.NextDouble() >= chance)
                    {
                        if (r < settings.Rows && r > 0 && c < settings.Columns && c > 0)
                        {
                            universe[r, c] = 1;
                        }
                    }
                    else
                    {
                        if (r < settings.Rows && r > 0 && c < settings.Columns && c > 0)
                        {
                            universe[r, c] = 0;
                        }
                    }
                }
            }
            return universe;
        }

        /// <summary>
        /// Generates the output file
        /// </summary>
        public void GenerateOutput(int[,] universe)
        {
            if (settings.OutputFile != "N/A")
            {
                string fullPath = Path.GetFullPath(settings.OutputFile);

                using (StreamWriter writer = new StreamWriter(fullPath))
                {
                    writer.WriteLine("#version=2.0");
                    for (int r = 0; r < settings.Rows; r++)
                    {
                        for (int c = 0; c < settings.Columns; c++)
                        {
                            if (universe[r, c] == 1)
                            {
                                writer.WriteLine($"(o) cell: {r}, {c}");
                            }
                        }
                    }
                }
                Messages.Success($"({settings.time}) Success: Final Generation written to file: {settings.OutputFile}");
            }
        }
    }
}
    
    