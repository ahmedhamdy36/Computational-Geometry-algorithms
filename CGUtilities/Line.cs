using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGUtilities
{
    /// <summary>
    /// The primary Line structure to be used in the CG project.
    /// </summary>
    public class Line : ICloneable
    {
        /// <summary>
        /// Creates a line structure that has the specified start/end.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="end">The end point.</param>
        public Line(Point start, Point end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Creates a line structure that has the specified start/end.
        /// </summary>
        /// <param name="X1">The X value for the start point.</param>
        /// <param name="Y1">The Y value for the start point.</param>
        /// <param name="X2">The X value for the end point.</param>
        /// <param name="Y2">The Y value for the end point.</param>
        public Line(double X1, double Y1, double X2, double Y2)
            : this(new Point(X1, Y1), new Point(X2, Y2))
        {
        }

        /// <summary>
        /// Gets or sets the start point.
        /// </summary>
        public Point Start
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        public Point End
        {
            get;
            set;
        }
        /// <summary>
        /// Instantiate Line
        /// </summary>
        /// <returns>new instance of Line</returns>
        public object Clone()
        {
            return new Line((Point)Start.Clone(), (Point)End.Clone());
        }
    }
}
