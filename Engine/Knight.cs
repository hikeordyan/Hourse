using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Demo.Engine
{
    public class Knight : Piece
    {
        public Knight(string name)
        {
            Name = name;
            m_location = new Location()
            {
                Row = 0,
                Rank = 0
            };
            Moves = new List<Location>(1);
            Moves.Add(m_location);
        }

        public override List<Location> GetAvailableMoves(Location Loc, Size Dimension)
        {
            List<Location> possibleMoves = new List<Location>(1);
            int newRow = Loc.Row + 2;
            int newRank = Loc.Rank - 1;
            if (Loc.ValidLocation(newRow, newRank, Dimension))
                possibleMoves.Add(new Location() { Row = newRow, Rank = newRank });

            newRow = Loc.Row + 2;
            newRank = Loc.Rank + 1;
            if (Loc.ValidLocation(newRow, newRank, Dimension))
                possibleMoves.Add(new Location() { Row = newRow, Rank = newRank });

            newRow = Loc.Row + 1;
            newRank = Loc.Rank + 2;
            if (Loc.ValidLocation(newRow, newRank, Dimension))
                possibleMoves.Add(new Location() { Row = newRow, Rank = newRank });

            newRow = Loc.Row + 1;
            newRank = Loc.Rank - 2;
            if (Loc.ValidLocation(newRow, newRank, Dimension))
                possibleMoves.Add(new Location() { Row = newRow, Rank = newRank });

            newRow = Loc.Row - 1;
            newRank = Loc.Rank - 2;
            if (Loc.ValidLocation(newRow, newRank, Dimension))
                possibleMoves.Add(new Location() { Row = newRow, Rank = newRank });

            newRow = Loc.Row - 1;
            newRank = Loc.Rank + 2;
            if (Loc.ValidLocation(newRow, newRank, Dimension))
                possibleMoves.Add(new Location() { Row = newRow, Rank = newRank });

            newRow = Loc.Row - 2;
            newRank = Loc.Rank - 1;
            if (Loc.ValidLocation(newRow, newRank, Dimension))
                possibleMoves.Add(new Location() { Row = newRow, Rank = newRank });

            newRow = Loc.Row - 2;
            newRank = Loc.Rank + 1;
            if (Loc.ValidLocation(newRow, newRank, Dimension))
                possibleMoves.Add(new Location() { Row = newRow, Rank = newRank });

            return possibleMoves;
        }
    }
}
