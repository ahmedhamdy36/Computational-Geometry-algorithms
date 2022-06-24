using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
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

            if (points.Count == 1 || points.Count == 2)
            {
                outPoints = points;
                return;
            }

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    Line l;
                    if (points[i] != points[j])
                    {
                        l = new Line(points[i], points[j]);
                        int left = 0, right = 0;

                        for (int k = 0; k < points.Count; k++)
                        {
                            Enums.TurnType e;
                            if (points[k] != points[i] && points[j] != points[k])
                            {
                                e = HelperMethods.CheckTurn(l, points[k]);

                                if (e == Enums.TurnType.Left)
                                    left++;
                                else if (e == Enums.TurnType.Right)
                                    right++;
                            }
                        }

                        if (right == 0 || left == 0)
                        {
                            outPoints.Add(points[i]);
                            outPoints.Add(points[j]);
                        }
                    }
                }
            }

            List<Point> P2 = new List<Point>();

            for (int i = 0; i < outPoints.Count; i++)
            {
                if (!P2.Contains(outPoints[i]))
                    P2.Add(outPoints[i]);
            }
            outPoints = P2;


            for (int i = 0; i < outPoints.Count; i++)
            {
                bool foundPoint = false;
                for (int j = 0; j < outPoints.Count; j++)
                {
                    for (int k = 0; k < outPoints.Count; k++)
                    {
                        if (outPoints[i] != outPoints[j] && outPoints[i] != outPoints[k])
                        {
                            bool pointOnLine = HelperMethods.PointOnSegment(outPoints[i], outPoints[j], outPoints[k]);
                            if (pointOnLine)
                            {
                                outPoints.Remove(outPoints[i]);
                                i--;
                                foundPoint = true;
                                break;
                            }
                        }
                    }
                    if (foundPoint)
                        break;
                }
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}