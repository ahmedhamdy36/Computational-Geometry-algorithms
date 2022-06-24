using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGUtilities
{
    public class Polygon
    {
        /// <summary>
        /// Gets or sets the list of lines constructing the current polygon.
        /// The last line in the list is a line connecting the last line with the start of the first line
        /// </summary>
        public List<Line> lines
        {
            get;
            set;
        }

        /// <summary>
        /// Initialize the lines constructing the polygon with an empty list
        /// </summary>
        public Polygon()
        {
            lines = new List<Line>();
        }

        /// <summary>
        /// Initialize the lines constructing the polygon with a given list l
        /// </summary>
        /// <param name="l">List of lines representing a polygon</param>
        public Polygon(List<Line> l)
        {
            lines = l;
        }
    }
}
