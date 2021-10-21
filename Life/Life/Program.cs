using System;
using System.Diagnostics;
using Display;
using System.IO;
using System.Collections.Generic;


namespace Life
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();

            // Construct settings and validate arguments if there are any
            Settings settings;
            if (args.Length != 0)
            {
                settings = ValidateArguments.Validate(args);
            }
            else
            {
                settings = new Settings();
            }

            //Holds most recent 5 generations for ghost mode
            int[][,] memory = new int[5][,];
            for (int i = 0; i < memory.Length; i++)
            {
                memory[i] = new int[settings.Rows, settings.Columns];
            }

            // Write Settings Information to Command Line
            Console.WriteLine($"({settings.time}) {settings}");
            Console.Write($"({settings.time}) Press <space> to start simulation... ");

            WaitSpacebar(watch);

            Grid grid = new Grid(settings.Rows, settings.Columns);

            grid.InitializeWindow();

            Universe universe = new Universe(settings);

            memory[4] = universe.GeneratePrimordialCells();

            bool steadyState = false;

            // Loop to generate and render new generations
            for (int i = 0; i <= settings.Generations; i++)
            {
                watch.Restart();

                if (!steadyState)
                {
                    // Clear all cells.
                    for (int r = 0; r < settings.Rows; r++)
                    {
                        for (int c = 0; c < settings.Columns; c++)
                        {
                            grid.UpdateCell(r, c, CellState.Blank);
                        }
                    }

                    grid.SetFootnote($"Generation {i}");



                    //If ghost mode is active then it will loop through all the memory universes
                    // stored in memory, and adjusts cellStateNo while doing it
                    // If ghost mode is not active then it will only loop through 
                    // the most recent universe
                    int gMode = 4;
                    int beginning = 4;
                    if (settings.GhostMode)
                    {
                        beginning = 0;
                    }
                    int cellStateNo = 0;

                    for (int g = beginning; g <= gMode; g++)
                    {
                        for (int r = 0; r < settings.Rows; r++)
                        {
                            for (int c = 0; c < settings.Columns; c++)
                            {
                                if (settings.GhostMode)
                                {
                                    if (memory[g][r, c] == 1)
                                    {
                                        grid.UpdateCell(r, c, (CellState)cellStateNo);
                                    }
                                }
                                else
                                {
                                    if (memory[g][r, c] == 1)
                                    {
                                        grid.UpdateCell(r, c, (CellState)1);
                                    }
                                }
                            }
                        }
                        if (cellStateNo == 0)
                        {
                            cellStateNo = 4;
                        }
                        else
                        {
                            cellStateNo--;
                        }
                    }
                    
                }

                grid.Render();
                
                steadyState = universe.SteadyState;

                if (steadyState)
                {
                    break;
                }

                if (settings.StepMode)
                {
                    WaitSpacebar(watch);
                }

                //Shifts all universes by one space except the most recent
                for (int j = 0; j <= memory.Length - 2; j++)
                {
                    memory[j] = memory[j + 1];
                }

                memory[4] = universe.GenerateNextGen(memory[4]);

                while (watch.ElapsedMilliseconds < (1000 / settings.UpdateRate));
            }

            grid.IsComplete = true;

            grid.Render();

            WaitSpacebar(watch);

            grid.RevertWindow();

            int periodicity = settings.GenerationalMemory - universe.SteadyStateIndex;

            if(steadyState)
            {
                Console.WriteLine($"({settings.time}) Steady-state detected... periodicity = {periodicity}");
            }
            else
            {
                Console.WriteLine($"({settings.time}) Steady-state not detected...");
            }

            if (settings.OutputFile != "N/A")
            {
                universe.GenerateOutput(memory[3]);
            }

        }

        private static void WaitSpacebar(Stopwatch watch)
        {
            int[] trackSpacebar = new int[5];

            while (true)
            {
                watch.Restart();
                while (watch.ElapsedMilliseconds < 50) ;

                for (int i = 0; i < trackSpacebar.Length - 1; i++)
                {
                    trackSpacebar[i] = trackSpacebar[i + 1];
                }

                if (Console.KeyAvailable)
                {
                    trackSpacebar[trackSpacebar.Length - 1] = 1;
                    while (Console.KeyAvailable)
                    {
                        while (Console.ReadKey().Key != ConsoleKey.Spacebar);
                    }
                }
                else
                {
                    trackSpacebar[trackSpacebar.Length - 1] = 0;
                }
                
                int index = 3;
                if (trackSpacebar[index] == 1 && trackSpacebar[index - 1] == 0 && trackSpacebar[index + 1] == 0)
                {
                    break;
                }
            }
        }
    }
}
