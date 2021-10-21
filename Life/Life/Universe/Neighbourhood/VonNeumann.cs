using System;
using System.Collections.Generic;
using System.Text;

namespace Life
{
    /// <summary>
    /// A class representing the VonNeumann Neighbourhood
    /// </summary>
    class VonNeumann : Neighbourhood
    {
        /// <summary>
        /// Checks each VonNeumann neighbour if they are alive and increments a counter if they are
        /// Checks if theres a "wrap around" if periodic mode
        /// </summary>
        /// <param name="universe">the current universe being checked</param>
        /// <param name="row">Current row positioned at</param>
        /// <param name="column">Current column positioned at</param>
        /// <param name="settings">contains all the parameters that might be required in validation</param>
        /// <returns>Then numbers of live neighbours</returns>
        public override int NeighbourCheck(int[,] universe, int row, int column, Settings settings)
        {
            int liveNeighbours = 0;
            int deviation = 0;
            for (int r = (row - settings.NeighbourhoodOrder); r <= (row + settings.NeighbourhoodOrder); r++)
            {
                deviation = settings.NeighbourhoodOrder - Math.Abs(row - r);

                for (int c = (column - deviation); c <= (column + deviation); c++)
                {
                    if (!(r == row && c == column))
                    {
                        if (settings.PeriodicMode != true)
                        {
                            if ((r >= 0 && r < settings.Rows) && (c >= 0 && c < settings.Columns))
                            {
                                // if the coordinate being checked is alive, add 1 to live neighbors
                                if (universe[r, c] == 1)
                                {
                                    liveNeighbours++;
                                }
                            }
                        }
                        else
                        {
                            int pRow = r;
                            int pColumn = c;

                            // If the current row or column being checked is past the ends of the grid
                            // change pRow or pColumn so that the cell checks the neighbour
                            // on the opposite side of the grid
                            if (r < 0)
                            {
                                pRow = settings.Rows + r;
                            }
                            else if (r > settings.Rows - 1)
                            {
                                pRow = r % settings.Rows;
                            }

                            if (c < 0)
                            {
                                pColumn = settings.Columns + c;
                            }
                            else if (c > settings.Columns - 1)
                            {
                                pColumn = c % settings.Columns;
                            }

                            if (universe[pRow, pColumn] == 1)
                            {
                                liveNeighbours++;
                            }
                        }
                    }
                }
            }
            return liveNeighbours;
        }
    }
}
