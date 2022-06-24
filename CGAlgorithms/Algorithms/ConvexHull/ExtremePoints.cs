using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<Point> P = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {
                if (!P.Contains(points[i]))
                    P.Add(points[i]);
            }
            points = P;


            for (int i = 0; i < points.Count; i++)
            {
                bool foundPoint = false;
                for (int j = 0; j < points.Count; j++)
                {
                    for (int k = 0; k < points.Count; k++)
                    {
                        for (int l = 0; l < points.Count; l++)
                        {
                            if (points[i] != points[j] && points[i] != points[k] && points[i] != points[l])
                            {
                                Enums.PointInPolygon chickPoint = HelperMethods.PointInTriangle(points[i], points[j], points[k], points[l]);
                                if (chickPoint == Enums.PointInPolygon.Inside || chickPoint == Enums.PointInPolygon.OnEdge)
                                {
                                    points.Remove(points[i]);
                                    i--;
                                    foundPoint = true;
                                    break;
                                }
                            }
                            if (foundPoint)
                                break;
                        }
                        if (foundPoint)
                            break;
                    }
                    if (foundPoint)
                        break;
                }
            }
            outPoints = points;
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}