using System;
using System.Collections.Generic;
using System.Text;

namespace Life
{
    /// <summary>
    /// An abstract class, representing neighbourhoods
    /// </summary>
    abstract class Neighbourhood
    {
        // Abstract Method inherited by the different types of neighbourhoods
        public abstract int NeighbourCheck(int[,] universe, int row, int column, Settings settings);
    }
}
