using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Demo.Engine
{
    public abstract class Piece : IDisposable
    {
        protected Image m_image;
        protected Location m_location;
        public string Name { get; set; }
        public List<Location> Moves { get; set; }

        public virtual void Render(Graphics graphics, Point point)
        {
            if (m_image == null)
                graphics.DrawString(Name, new Font("Arial", 9f), new SolidBrush(Color.Black), point);
            else
                graphics.DrawImage(m_image, point);
        }

        public virtual List<Location> GetAvailableMoves(Location location, Size dimension)
        {
            return null;
        }

        public Location Loc
        {
            get { return m_location; }
            set { m_location = value; }
        }

        public void Dispose()
        {
            if (m_image != null)
                m_image.Dispose();
        }
    }
}
