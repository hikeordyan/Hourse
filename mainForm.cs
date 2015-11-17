using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Demo
{
    public partial class mainForm : Form
    {
        private System.Timers.Timer m_timer = new System.Timers.Timer();
        private Engine.Board m_board;

        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            m_timer.Elapsed += new System.Timers.ElapsedEventHandler(m_timer_Elapsed);
            m_timer.Interval = 150;

            m_board = new Demo.Engine.Board();
            m_board.Pieces.Add(new Engine.Knight("Knight"));
            m_board.Dimensions = new Size(20, 20);
            m_board.ParentSize = new Size(700, 500);
            m_board.XOffset = 5;
            m_board.YOffset = 5;
            m_board.RatioPercentage = .99f;
        }

        protected void m_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Strategy();
            Invalidate();
        }

        private void Strategy()
        {
               Engine.Piece piece = m_board.Pieces.Last();
            try
            {
                // Available moves for the current piece
             
                List<Engine.Location> moves = piece.GetAvailableMoves(piece.Loc, m_board.Dimensions);
                List<Engine.Location> locationsToRemove = new List<Engine.Location>(1);
                List<Engine.LocationEnhance> locationsEnhance = new List<Engine.LocationEnhance>(1);

                // First we want to filter out the existing pieces
                for (int x = 0; x < moves.Count; x++)
                {
                    Engine.Location loc = moves[x];

                    if (piece.Moves.Contains(loc))
                    {
                        locationsToRemove.Add(loc);
                        continue;
                    }

                    List<Engine.Location> availableMoves = piece.GetAvailableMoves(loc, m_board.Dimensions);
                    List<Engine.Location> filterMoves = new List<Engine.Location>(1);
                    foreach (Engine.Location nLoc in availableMoves)
                        if (!piece.Moves.Contains(nLoc))
                            filterMoves.Add(nLoc);

                    locationsEnhance.Add(new Demo.Engine.LocationEnhance()
                    {
                        Loc = loc,
                        MoveCount = filterMoves.Count
                    });
                }

                List<Engine.LocationEnhance> lenhance = locationsEnhance.OrderBy(c => c.MoveCount).ToList<Engine.LocationEnhance>();
                piece.Moves.Add(lenhance.First().Loc);
                piece.Loc = lenhance.First().Loc;
            }
            catch (Exception)
            {
                if (piece.Moves.Count == 64)
                {
                    m_timer.Stop();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            m_board.Render(e.Graphics);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            m_board.Dimensions = new Size(trackBar1.Value, m_board.Dimensions.Height);
            m_timer.Stop();
            m_board.Pieces.First().Moves.Clear();
            Invalidate();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            m_board.Dimensions = new Size(m_board.Dimensions.Width, trackBar2.Value);
            m_timer.Stop();
            m_board.Pieces.First().Moves.Clear();
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (m_board != null)
            {
                lblSquare.Text = m_board.GetSquareName(e.Location);
                m_board.AddHighlight(e.Location);
            }

            Invalidate();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                m_board.Pieces.Last().Loc = new Demo.Engine.Location()
                {
                    Row = Convert.ToInt32(textBox1.Text),
                    Rank = Convert.ToInt32(textBox2.Text)
                };

                Invalidate();
            }
            catch (Exception) { }
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            m_board.Pieces.Last().Moves.Clear();
            m_board.Pieces.Last().Loc = new Demo.Engine.Location()
            {
                Row = 0,
                Rank = 0
            };
            m_board.Pieces.Last().Moves.Add(m_board.Pieces.Last().Loc);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_timer.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_timer.Stop();
        }
    }
}
