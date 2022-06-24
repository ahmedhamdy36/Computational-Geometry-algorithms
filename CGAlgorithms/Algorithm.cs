using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;
namespace CGAlgorithms
{
    /// <summary>
    /// The primary algorithm abstract class to be used in the CG project.
    /// </summary>
    public abstract class Algorithm
    {
        /// <summary>
        /// Runs the algorithm and fills the ref parameters with the results.
        /// </summary>
        /// <param name="points">The points to use.</param>
        /// <param name="lines">The lines to use.</param>
        /// <param name="outPoints">The point list to populate with results.</param>
        /// <param name="outLines">The line list to populate with results.</param>
        public abstract void Run(
            List<Point> points, List<Line> lines, List<Polygon> polygons,
            ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons
            );

        /// <summary>
        /// Retuens the name of the algorithm (used in UI).
        /// </summary>
        /// <returns>The algorithm name.</returns>
        public abstract override string ToString();
    }
}
