using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Demo.Engine
{
    public struct Location
    {
        public int Rank;
        public int Row;

        public bool ValidLocation(int Row, int Rank, Size Dimension)
        {
            if (Row >= 0 && Row < Dimension.Height &&
                Rank >= 0 && Rank < Dimension.Width)
                return true;
            else
                return false;
        }
    }

    public struct LocationEnhance
    {
        public Location Loc;
        public int MoveCount;
    }
}
