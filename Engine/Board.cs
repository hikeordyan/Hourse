using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Demo.Engine
{
    public class Board
    {
        #region Variables
        private Size m_dimension = new Size(8, 8);          // Default Chess Board Dimensions
        private Size m_parentSize = new Size(800, 600);     // 800x600 
        private float m_ratioPercentage = 0.8f;             // Percentage amount to take from Parent.Size/Dimension.Size

        private Brush m_defaultDark = new SolidBrush(Color.FromArgb(205, 133, 63));
        private Brush m_defaultLight = new SolidBrush(Color.FromArgb(222, 184, 135));
        private Brush m_defaultHighlight = new SolidBrush(Color.FromArgb(150, 240, 255, 255));

        private List<Rectangle> m_highlighted;
        #endregion

        #region Constructor
        public Board()
        {
            // Defaults
            DefaultPen = new Pen(Color.Black);
            SmoothingMode = SmoothingMode.AntiAlias;
            TextRenderingHint = TextRenderingHint.AntiAlias;
            ParentSize = new Size(800, 600);
            XOffset = 10;
            YOffset = 10;
            Pieces = new List<Piece>(1);

            // Init
            m_highlighted = new List<Rectangle>(1);

            CaculateSquareSize();
        }
        #endregion

        #region Methods
        protected void CaculateSquareSize()
        {
            SquareSize = (int)Math.Min(ParentSize.Height / Dimensions.Height * m_ratioPercentage,
                                       ParentSize.Width / Dimensions.Width * m_ratioPercentage);
        }

        public virtual void Render(Graphics graphics)
        {
            graphics.Clear(Color.White);
            graphics.SmoothingMode = this.SmoothingMode;
            graphics.TextRenderingHint = this.TextRenderingHint;

            bool darkFlag = true; // Hack - maybe there is a better way, but in a chessboard it will always work.
            for (int h = 0; h < Dimensions.Height; h++)
            {
                for (int w = 0; w < Dimensions.Width; w++)
                {
                    // Create Rectangle
                    Rectangle rect = new Rectangle(w * SquareSize + XOffset,
                                                   h * SquareSize + YOffset,
                                                   SquareSize,
                                                   SquareSize);
                    // Fill Rectangle
                    graphics.FillRectangle(darkFlag ? m_defaultDark : m_defaultLight, rect);

                    // Draw Border
                    graphics.DrawRectangle(DefaultPen, rect);

                    // Reverse
                    darkFlag = !darkFlag;
                }

                // New Row Reverse
                darkFlag = !darkFlag;
            }

            // Other Logic
            foreach (Rectangle rect in m_highlighted)
                graphics.FillRectangle(m_defaultHighlight, rect);

            // Pieces
            RenderPieces(graphics);
        }

        private void RenderPieces(Graphics graphics)
        {
            foreach (Piece piece in Pieces)
            {
                int h = piece.Loc.Row * SquareSize + YOffset;
                int w = piece.Loc.Rank * SquareSize + XOffset;
                Rectangle rect = new Rectangle(w,
                                               h,
                                               SquareSize,
                                               SquareSize);
                Point point = new Point(rect.X + SquareSize / 4,
                                        rect.Y + SquareSize / 4);

                piece.Render(graphics, point);

                // Test Code
                //foreach (Location loc in piece.GetAvailableMoves())
                //{
                //    h = loc.Row * SquareSize + YOffset;
                //    w = loc.Rank * SquareSize + XOffset;
                //    rect = new Rectangle(w,
                //                         h,
                //                         SquareSize,
                //                         SquareSize);
                //    graphics.FillRectangle(m_defaultHighlight, rect);
                //}

                // Test Code
                Point prevPoint = new Point(0, 0);
                Pen pen = new Pen(new SolidBrush(Color.Black));
                foreach (Location loc in piece.Moves)
                {
                    h = loc.Row * SquareSize + YOffset;
                    w = loc.Rank * SquareSize + XOffset;
                    rect = new Rectangle(w,
                                         h,
                                         SquareSize,
                                         SquareSize);
                    
                    graphics.FillRectangle(m_defaultHighlight, rect);
                    graphics.DrawRectangle(pen, new Rectangle(rect.X + SquareSize / 2 - 2,
                                                              rect.Y + SquareSize / 2 - 2,
                                                              4,
                                                              4));
                    graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(rect.X + SquareSize / 2 - 2,
                                                              rect.Y + SquareSize / 2 - 2,
                                                              4,
                                                              4));

                    point = new Point(rect.X + SquareSize / 2,
                                     rect.Y + SquareSize / 2);
                    if (prevPoint.X > 0)
                    {
                        graphics.DrawLine(pen, prevPoint, point);
                    }
                    prevPoint = point;
                }
            }
        }

        public int GetRow(Point location)
        {
            return (location.Y - YOffset) / SquareSize;
        }

        public int GetRank(Point location)
        {
            return (location.X - XOffset) / SquareSize;
        }

        public T GetPiece<T>(Point location)
        {
            object piece = null;

            piece = (from p in Pieces
                     where p.Loc.Rank == GetRank(location) && p.Loc.Row == GetRow(location)
                     select p).FirstOrDefault();

            return (T)piece;
        }

        public T GetPiece<T>(int row, int rank)
        {
            object piece = null;

            piece = (from p in Pieces
                     where p.Loc.Rank == rank && p.Loc.Row == row
                     select p).FirstOrDefault();

            return (T)piece;
        }

        public string GetSquareName(Point location)
        {
            return string.Format("Row {0}, Rank {1}", GetRow(location), GetRank(location));
        }

        public void AddHighlight(Point location)
        {
            int h = GetRow(location) * SquareSize + YOffset;
            int w = GetRank(location) * SquareSize + XOffset;

            m_highlighted.Clear();
            m_highlighted.Add(new Rectangle(w,
                                            h,
                                            SquareSize,
                                            SquareSize));
        }
        #endregion

        #region Properties
        public List<Piece> Pieces { get; set; }

        public float RatioPercentage
        {
            get { return m_ratioPercentage; }
            set
            {
                m_ratioPercentage = value;
                CaculateSquareSize();
            }
        }

        public SmoothingMode SmoothingMode { get; set; }

        public TextRenderingHint TextRenderingHint { get; set; }

        public Size Dimensions
        {
            get { return m_dimension; }
            set
            {
                m_dimension = value;
                CaculateSquareSize();
            }
        }

        public Size ParentSize
        {
            get { return m_parentSize; }
            set
            {
                m_parentSize = value;
                CaculateSquareSize();
            }
        }

        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public int SquareSize { get; set; }
        public Pen DefaultPen { get; set; }
        #endregion
    }
}
